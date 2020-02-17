namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface BetChoiceMatchingWorkbook
    {
        Workbook<BetChoiceMatchingTicketModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<BetChoiceMatchingTicketModel> Sheet(string sheetName);

        Workbook<BetChoiceMatchingTicketModel> Title(string titleText);

        Workbook<BetChoiceMatchingTicketModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        BetChoiceMatchingWorkbook Body(IEnumerable<BetChoiceMatchingEventModel> models);
    }

    public class BetChoiceMatchingWorkbookImpl : NpoiWorkbook<BetChoiceMatchingTicketModel>, BetChoiceMatchingWorkbook
    {
        private int currentRowIndex;
        private const int TemplateSheetIndex = 0;
        private const int ReportSheetIndex = 1;
        private const int BodyStartIndex = 2;
        private readonly IBetListExcelBuilder excelBuilder;
        private readonly ITicketHelper ticketHelper;
        private readonly CacheRefService cacheRefService;

        public BetChoiceMatchingWorkbookImpl(
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            CacheRefService cacheRefService)
        {
            this.excelBuilder = excelBuilder;
            this.ticketHelper = ticketHelper;
            this.cacheRefService = cacheRefService;
        }

        public BetChoiceMatchingWorkbook Body(IEnumerable<BetChoiceMatchingEventModel> models)
        {
            Models = models;
            decimal totalStake = 0;
            decimal totalMemberWinLoss = 0;
            decimal totalSuperWinLoss = 0;
            decimal totalCompany = 0;
            bool isRunning = true;
            foreach (var model in models)
            {
                foreach(var user in model.Users)
                {
                    totalStake += user.Stake;
                    if (!user.IsRunning)
                    {
                        totalMemberWinLoss += user.MemberWinLoss;
                        totalSuperWinLoss += user.SuperWinLoss;
                        totalCompany += user.Company;
                        isRunning = false;
                    }
                }
            }

            Total = new BetChoiceMatchingTicketModel(totalStake, totalMemberWinLoss, totalSuperWinLoss, totalCompany, isRunning);
            return this;
        }

        public BetChoiceMatchingTicketModel Total { get; private set; }

        public IEnumerable<BetChoiceMatchingEventModel> Models { get; private set; }

        public Row TemplateEventRow { get; private set; }

        public Row TemplateEvenUserTotalRow { get; private set; }

        public Row TemplateEvenUserDataRow { get; private set; }

        public Row TemplateOddUserTotalRow { get; private set; }

        public Row TemplateOddUserDataRow { get; private set; }

        public Row TemplateTotalRow { get; private set; }

        public Row TemplateNoInfoRow { get; private set; }

        public ISheet NpoiReportSheet { get; private set; }

        public RTFHelper RTFHelper { get; private set; }

        protected override Sheet LoadTemplateSheet()
        {
            var templateSheet = GetSheetAt(TemplateSheetIndex);
            var reportSheet = GetSheetAt(ReportSheetIndex);
            var rowIndex = 0;
            NpoiReportSheet = Workbook.GetSheetAt(ReportSheetIndex);
            TemplateEventRow = templateSheet.GetRow(rowIndex++);
            TemplateEvenUserTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateEvenUserDataRow = templateSheet.GetRow(rowIndex++);
            TemplateOddUserTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateOddUserDataRow = templateSheet.GetRow(rowIndex++);
            TemplateTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateNoInfoRow = templateSheet.GetRow(rowIndex);
            var npoiTemplateSheet = Workbook.GetSheetAt(TemplateSheetIndex);
            var templateCellStyle = npoiTemplateSheet.GetRow(rowIndex).GetCell(0).CellStyle;
            var templateFont = templateCellStyle.GetFont(Workbook);
            RTFHelper = new RTFHelper
            {
                NegFont = templateFont,
                NormalFont = templateFont,
                PosFont = templateFont,
                NegFontCrossed = templateFont,
                NormalFontCrossed = templateFont,
                PosFontCrossed = templateFont,
                RTFRenderer = new RtfTextRender()
            };

            return reportSheet;
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet));
            }

            RemoveTemplateRows(sheet);
            currentRowIndex = BodyStartIndex;
            if (Models.Any())
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(TemplateNoInfoRow);
            row.SetCellValue(Column.GetColIndex(TemplateInfo.ColumnStartName), TemplateInfo.NoInfomationText);
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            foreach (var model in Models)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(TemplateEventRow);
                FillEventRow(row, model);
                var userIndex = 0;
                foreach(var user in model.Users)
                {
                    var isEvenRow = userIndex % 2 == 0;
                    var totalRowIndex = currentRowIndex++;
                    var totalRow = sheet.GetRow(totalRowIndex);
                    totalRow.CopyCellsFrom(isEvenRow ? TemplateEvenUserTotalRow : TemplateOddUserTotalRow);
                    FillUserTotalRow(totalRow, user);

                    var templateUserDataRow = isEvenRow ? TemplateEvenUserDataRow : TemplateOddUserDataRow;
                    foreach(var ticket in user.Tickets)
                    {
                        row = sheet.GetRow(currentRowIndex++);
                        row.CopyCellsFrom(templateUserDataRow);
                        FillTicketRow(row, ticket);
                    }

                    NpoiReportSheet.AddMergedRegion(new CellRangeAddress(totalRowIndex, totalRowIndex + user.Tickets.Count(), 0, 0));
                    userIndex++;
                }
            }
        }

        private static void FillUserTotalRow(Row row, BetChoiceMatchingUserModel user)
        {
            row.SetCellValue(Column.GetColIndex(Column.A), user.Username);
            row.SetCellValue(Column.GetColIndex(Column.B), Resources.Total);
            row.SetCellValue(Column.GetColIndex(Column.F), user.Stake);
            if (!user.IsRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), user.MemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), user.SuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), user.Company);
            }
        }

        private static void FillTicketRow(Row row, BetChoiceMatchingTicketModel ticket)
        {
            row.SetCellValue(Column.GetColIndex(Column.A), ticket.Username);
            row.SetCellValue(Column.GetColIndex(Column.B), ticket.TransDate);
            row.SetCellValue(Column.GetColIndex(Column.C), ticket.TransId);
            row.SetCellValue(Column.GetColIndex(Column.D), ticket.Odds);
            row.SetCellValue(Column.GetColIndex(Column.E), ticket.IP);
            row.SetCellValue(Column.GetColIndex(Column.F), ticket.Stake);
            if (!ticket.IsRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), ticket.MemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), ticket.SuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), ticket.Company);
            }
        }

        private void FillEventRow(Row row, BetChoiceMatchingEventModel model)
        {
            var ticket = new Ticket
            {
                LeagueId = model.LeagueId,
                SportTypeId = model.SportType,
                BetTypeId = model.BetType,
                Handicap1 = model.Hdp1,
                Handicap2 = model.Hdp2,
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                BetTeam = model.BetTeam,
                IsLive = model.IsLive,
                LiveHomeScore = model.LiveHomeScore,
                LiveAwayScore = model.LiveAwayScore
            };

            row.SetCellValue(Column.GetColIndex(Column.A), BuildChoice(ticket));
            row.SetCellValue(Column.GetColIndex(Column.B), BuildMatch(ticket));
            row.SetCellValue(Column.GetColIndex(Column.H), model.GlobalShowTime);
        }

        private IRichTextString BuildChoice(ITicket ticket)
        {
            var choiceBuilder = excelBuilder.CreateChoiceBuilder(ticket.BetTypeId);
            return choiceBuilder.RenderRTF(ticket, ticketHelper, null, false, RTFHelper).FirstOrDefault();
        }

        private string BuildMatch(ITicket ticket)
        {
            var line1 = $"{cacheRefService.GetTeamName((int)ticket.HomeId)} - vs - {cacheRefService.GetTeamName((int)ticket.AwayId)}";
            var line2 = $"{cacheRefService.GetSportName(ticket.SportTypeId)} {cacheRefService.GetLeagueName(ticket.LeagueId)}";
            return $"{line1}\n{line2}";
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(TemplateTotalRow);
            row.SetCellValue(Column.GetColIndex(Column.B), Resources.Total);
            row.SetCellValue(Column.GetColIndex(Column.F), Total.Stake);
            if (!Total.IsRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), Total.MemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), Total.SuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), Total.Company);
            }
        }
    }
}