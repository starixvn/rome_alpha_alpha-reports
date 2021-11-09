namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Consolidation;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using static ONEbook.SpreadSheets.Column;

    public interface ConsolidationByUsernameWorkbook
    {
        ConsolidationByUsernameWorkbook SetData(IEnumerable<ConsolidationByUsernameViewModel> reportData);

        Workbook<ConsolidationByUsernameViewModel> UseTemplate(TemplateInfo templateInfo);
    }

    public class WinLossByUsernameWorkbookImpl :
        NpoiWorkbook<ConsolidationByUsernameViewModel>,
        ConsolidationByUsernameWorkbook
    {
        private const int ReportSheetIndex = 1;
        private const string PaddingToProductName = "    ";

        internal IEnumerable<ConsolidationByUsernameViewModel> ReportData { get; private set; }

        internal Mapper<ConsolidationByUsernameViewModel> GroupByUsernameMapper { get; }
            = new Mapper<ConsolidationByUsernameViewModel>()
                .Map(A, winLoss => winLoss.CustLevelName)
                .Map(B, winLoss => winLoss.Username)
                .Map(C, winLoss => winLoss.CurrencyName)
                .Map(D, winLoss => winLoss.Turnover)
                .Map(E, winLoss => winLoss.GrossComm)
                .Map(F, winLoss => winLoss.MemberWinLoss)
                .Map(G, winLoss => winLoss.MemberComm)
                .Map(H, winLoss => winLoss.MemberTotal)
                .Map(I, winLoss => winLoss.OwnerTurnover)
                .Map(J, winLoss => winLoss.OwnerWinLoss)
                .Map(K, winLoss => winLoss.OwnerComm)
                .Map(L, winLoss => winLoss.OwnerTotal)
                .Map(M, winLoss => winLoss.Company)
                .Map(N, winLoss => winLoss.BetCount);
        internal Mapper<ConsolidationByUsernameViewModel> GroupByPTMapper { get; }
            = new Mapper<ConsolidationByUsernameViewModel>()
                .Map(A, winLoss => string.Empty)
                .Map(B, winLoss => winLoss.PositionTakingText)
                .Map(C, winLoss => string.Empty)
                .Map(D, winLoss => winLoss.Turnover)
                .Map(E, winLoss => winLoss.GrossComm)
                .Map(F, winLoss => winLoss.MemberWinLoss)
                .Map(G, winLoss => winLoss.MemberComm)
                .Map(H, winLoss => winLoss.MemberTotal)
                .Map(I, winLoss => winLoss.OwnerTurnover)
                .Map(J, winLoss => winLoss.OwnerWinLoss)
                .Map(K, winLoss => winLoss.OwnerComm)
                .Map(L, winLoss => winLoss.OwnerTotal)
                .Map(M, winLoss => winLoss.Company)
                .Map(N, winLoss => winLoss.BetCount);
        internal Mapper<ConsolidationByUsernameViewModel> DataMapper { get; }
            = new Mapper<ConsolidationByUsernameViewModel>()
                .Map(A, winLoss => string.Empty)
                .Map(B, winLoss => $"{PaddingToProductName}{winLoss.ProductName}")
                .Map(C, winLoss => string.Empty)
                .Map(D, winLoss => winLoss.Turnover)
                .Map(E, winLoss => winLoss.GrossComm)
                .Map(F, winLoss => winLoss.MemberWinLoss)
                .Map(G, winLoss => winLoss.MemberComm)
                .Map(H, winLoss => winLoss.MemberTotal)
                .Map(I, winLoss => winLoss.OwnerTurnover)
                .Map(J, winLoss => winLoss.OwnerWinLoss)
                .Map(K, winLoss => winLoss.OwnerComm)
                .Map(L, winLoss => winLoss.OwnerTotal)
                .Map(M, winLoss => winLoss.Company)
                .Map(N, winLoss => winLoss.BetCount);

        internal Row GroupByUsernameRowTemplate { get; private set; }
        internal Row GroupByPTRowTemplate { get; private set; }
        internal Row DataRowTemplate { get; private set; }
        internal Row NoInfoRowTemplate { get; private set; }

        public ConsolidationByUsernameWorkbook SetData(IEnumerable<ConsolidationByUsernameViewModel> reportData)
        {
            ReportData = reportData;
            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);
            var currentRowIndex = TemplateInfo.BodyStartIndex;

            GroupByUsernameRowTemplate = sheet.GetRow(currentRowIndex++);
            GroupByPTRowTemplate = sheet.GetRow(currentRowIndex++);
            DataRowTemplate = sheet.GetRow(currentRowIndex++);
            NoInfoRowTemplate = sheet.GetRow(currentRowIndex);

            return GetSheetAt(ReportSheetIndex);
        }

        protected override void RemoveTemplateRows(Sheet sheet)
            => Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (ReportData.Any())
            {
                FillBodyRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            Row row;
            var currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var dataByUsername in ReportData)
            {
                row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(GroupByUsernameRowTemplate);
                FillCells(row, dataByUsername, GroupByUsernameMapper.Mappings);

                foreach (var dataByPT in dataByUsername.Children)
                {
                    row = sheet.GetRow(currentRowIndex++);
                    row.CopyCellsFrom(GroupByPTRowTemplate);
                    FillCells(row, dataByPT, GroupByPTMapper.Mappings);

                    foreach (var data in dataByPT.Children)
                    {
                        row = sheet.GetRow(currentRowIndex++);
                        row.CopyCellsFrom(DataRowTemplate);
                        FillCells(row, data, DataMapper.Mappings);
                    }
                }
            }
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var row = sheet.GetRow(TemplateInfo.BodyStartIndex);
            row.CopyCellsFrom(NoInfoRowTemplate);
            row.SetCellValue(GetColIndex(TemplateInfo.ColumnStartName), TemplateInfo.NoInfomationText);
        }
    }
}