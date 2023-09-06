namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Site.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using BetListNPOI.SS.UserModel;
    using BetListNPOI.SS.Util;
    using ONEbook.SpreadSheets.BetListNPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ONEbook.SpreadSheets;

    public interface BetChoiceMatchingWorkbook
    {
        BetChoiceMatchingWorkbook SetData(IEnumerable<BetChoiceMatchingEventModel> events);

        Workbook<BetChoiceMatchingTicketModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<BetChoiceMatchingTicketModel> Sheet(string sheetName);

        Workbook<BetChoiceMatchingTicketModel> Title(string titleText);

        Workbook<BetChoiceMatchingTicketModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();
    }

    public class BetChoiceMatchingWorkbookImpl : NpoiWorkbook<BetChoiceMatchingTicketModel>, BetChoiceMatchingWorkbook
    {
        private int currentRowIndex;
        private const int TemplateSheetIndex = 0;
        private const int ReportSheetIndex = 1;
        private const int BodyStartIndex = 2;
        private readonly IBetListExcelBuilder excelBuilder;
        private readonly ITicketHelper ticketHelper;

        public BetChoiceMatchingWorkbookImpl(
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper)
        {
            this.excelBuilder = excelBuilder;
            this.ticketHelper = ticketHelper;
        }

        public BetChoiceMatchingWorkbook SetData(IEnumerable<BetChoiceMatchingEventModel> events)
        {
            Events = events;
            return this;
        }

        public IEnumerable<BetChoiceMatchingEventModel> Events { get; private set; }

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
            if (Events.Any())
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
            foreach (var eventModel in Events)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(TemplateEventRow);
                FillEventRow(row, eventModel);
                var userIndex = 0;
                foreach(var user in eventModel.Users)
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
            row.SetCellValue(Column.GetColIndex(Column.C), ticket.TransId.ToString());
            row.SetCellValue(Column.GetColIndex(Column.D), BuildOdds(ticket));
            row.SetCellValue(Column.GetColIndex(Column.E), ticket.IP ?? string.Empty);
            row.SetCellValue(Column.GetColIndex(Column.F), ticket.Stake);
            if (!ticket.IsRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), ticket.MemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), ticket.SuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), ticket.Company);
            }
        }

        private static string BuildOdds(BetChoiceMatchingTicketModel ticket)
        {
            if(ticket.Odds == 0)
            {
                return "-";
            }

            return string.IsNullOrEmpty(ticket.DisplayOddsType)
                    ? ticket.DisplayOdds
                    : $"({ticket.DisplayOddsType}) {ticket.DisplayOdds}";
        }

        private void FillEventRow(Row row, BetChoiceMatchingEventModel eventModel)
        {
            var choice = BuildChoice(eventModel);
            row.SetCellValue(Column.GetColIndex(Column.A), choice);
            row.SetCellValue(Column.GetColIndex(Column.H), eventModel.GlobalShowTime);
        }

        private IRichTextString BuildChoice(BetChoiceMatchingEventModel eventModel)
        {
            var ticket = new Ticket
            {
                LeagueId = eventModel.LeagueId,
                SportTypeId = eventModel.SportType,
                BetTypeId = eventModel.BetType,
                Handicap1 = eventModel.Hdp1,
                Handicap2 = eventModel.Hdp2,
                HomeId = eventModel.HomeId,
                AwayId = eventModel.AwayId,
                BetTeam = eventModel.BetTeam,
                IsLive = eventModel.IsLive,
                LiveHomeScore = eventModel.LiveHomeScore,
                LiveAwayScore = eventModel.LiveAwayScore
            };
            var choiceBuilder = excelBuilder.CreateChoiceBuilder(ticket.BetTypeId);
            return choiceBuilder.RenderRTF(ticket, ticketHelper, null, false, RTFHelper).FirstOrDefault();
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            decimal totalStake = 0;
            decimal totalMemberWinLoss = 0;
            decimal totalSuperWinLoss = 0;
            decimal totalCompany = 0;
            bool isRunning = true;
            foreach (var eventModel in Events)
            {
                foreach (var user in eventModel.Users)
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

            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(TemplateTotalRow);
            row.SetCellValue(Column.GetColIndex(Column.A), Resources.Total);
            row.SetCellValue(Column.GetColIndex(Column.F), totalStake);
            if (!isRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), totalMemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), totalSuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), totalCompany);
            }
        }
    }
}