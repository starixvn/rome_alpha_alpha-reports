namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels.MatchWinLossDetail;

    public interface MatchWinLossDetailWorkbook
    {
        Workbook<BetTypeWinLossViewModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<BetTypeWinLossViewModel> Sheet(string sheetName);

        Workbook<BetTypeWinLossViewModel> Title(string titleText);

        Workbook<BetTypeWinLossViewModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        MatchWinLossDetailWorkbook SetData(
            IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail,
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay,
            BetTypeWinLossViewModel grandTotal);

        MatchWinLossDetailWorkbook SetRowMappers(
            Mapper<BetTypeWinLossViewModel> betTypeRowMapper,
            Mapper<BetTypeWinLossViewModel> subTotalRowMapper,
            Mapper<BetTypeWinLossViewModel> totalRowMapper,
            Mapper<BetTypeWinLossViewModel> grandTotalRowMapper);
    }

    public class MatchWinLossDetailWorkbookImpl :
        NpoiWorkbook<BetTypeWinLossViewModel>,
        MatchWinLossDetailWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<MatchWinLossDetailViewModel> NoParlayMatchWinLossDetail { get; set; }

        internal BetTypeWinLossViewModel NoParlayWinLossTotal { get; set; }

        internal BetTypeWinLossViewModel MixParlay { get; set; }

        internal BetTypeWinLossViewModel SystemParlay { get; set; }

        internal BetTypeWinLossViewModel GrandTotal { get; set; }

        internal Mapper<BetTypeWinLossViewModel> BetTypeRowMapper { get; set; }

        internal Mapper<BetTypeWinLossViewModel> SubTotalRowMapper { get; set; }

        internal Mapper<BetTypeWinLossViewModel> TotalRowMapper { get; set; }

        internal Mapper<BetTypeWinLossViewModel> GrandTotalRowMapper { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row SubTotalRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        internal Row MixParlayRowTemplate { get; set; }

        internal Row SystemParlayRowTemplate { get; set; }

        internal Row GrandTotalRowTemplate { get; set; }

        public MatchWinLossDetailWorkbook SetData(
            IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail,
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay,
            BetTypeWinLossViewModel grandTotal)
        {
            NoParlayMatchWinLossDetail = noParlayMatchWinLossDetail;
            NoParlayWinLossTotal = noParlayWinLossTotal;
            MixParlay = mixParlay;
            SystemParlay = systemParlay;
            GrandTotal = grandTotal;

            return this;
        }

        public MatchWinLossDetailWorkbook SetRowMappers(
            Mapper<BetTypeWinLossViewModel> betTypeRowMapper,
            Mapper<BetTypeWinLossViewModel> subTotalRowMapper,
            Mapper<BetTypeWinLossViewModel> totalRowMapper,
            Mapper<BetTypeWinLossViewModel> grandTotalRowMapper)
        {
            BetTypeRowMapper = betTypeRowMapper;
            SubTotalRowMapper = subTotalRowMapper;
            TotalRowMapper = totalRowMapper;
            GrandTotalRowMapper = grandTotalRowMapper;

            return this;
        }

        internal void FillMatchTitle(Sheet sheet, int rowIndex, MatchWinLossDetailViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MatchTitleRowTemplate);

            var title = (data.LeagueName + (string.IsNullOrEmpty(data.MatchName) ? string.Empty : $" >> {data.MatchName}"))
                .ToUpper(CultureInfo.InvariantCulture);
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, rowIndex, row.LastCellIndex - 1, title);
        }

        internal void FillBetTypeWinLoss(
            Sheet sheet,
            int rowIndex,
            bool isEvenRow,
            BetTypeWinLossViewModel betTypeWinLoss)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, betTypeWinLoss, BetTypeRowMapper.Mappings);
        }

        internal void FillMatchTotal(Sheet sheet, int rowIndex, BetTypeWinLossViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SubTotalRowTemplate);
            FillCells(row, data, SubTotalRowMapper.Mappings);
        }

        internal void FillTotal(Sheet sheet, int rowIndex, BetTypeWinLossViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(TotalRowTemplate);
            FillCells(row, data, TotalRowMapper.Mappings);
        }

        internal void FillMixParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MixParlayRowTemplate);
            FillCells(row, MixParlay, BetTypeRowMapper.Mappings);
        }

        internal void FillSystemParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(SystemParlayRowTemplate);
            FillCells(row, SystemParlay, BetTypeRowMapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            MatchTitleRowTemplate = sheet.GetRow(currentRowIndex++);
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            SubTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);
            MixParlayRowTemplate = sheet.GetRow(currentRowIndex++);
            SystemParlayRowTemplate = sheet.GetRow(currentRowIndex++);
            GrandTotalRowTemplate = sheet.GetRow(currentRowIndex + 1);

            return GetSheetAt(ReportSheetIndex);
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
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                MatchTitleRowTemplate.LastCellIndex - 1,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            var hasData = (NoParlayMatchWinLossDetail != null && NoParlayMatchWinLossDetail.Any())
                || MixParlay != null
                || SystemParlay != null;

            if (hasData)
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(GrandTotalRowTemplate);
            FillCells(row, GrandTotal, GrandTotalRowMapper.Mappings);

            foreach (var data in NoParlayMatchWinLossDetail)
            {
                FillMatchTitle(sheet, currentRowIndex++, data);

                var isEvenRow = false;
                foreach (var betTypeWinLoss in data.BetTypeWinLoss)
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betTypeWinLoss);
                    isEvenRow = !isEvenRow;
                }

                if (data.RenderSubTotal)
                {
                    FillMatchTotal(sheet, currentRowIndex++, data.Total);
                }
            }

            if (NoParlayWinLossTotal != null &&
                (MixParlay != null || SystemParlay != null))
            {
                FillTotal(sheet, currentRowIndex++, NoParlayWinLossTotal);
            }

            if (MixParlay != null)
            {
                FillMixParlayWinLoss(sheet, currentRowIndex++);
            }

            if (SystemParlay != null)
            {
                FillSystemParlayWinLoss(sheet, currentRowIndex++);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (!SpreadSheet.FooterRows.Any())
            {
                return;
            }

            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(GrandTotalRowTemplate);
            FillCells(row, GrandTotal, GrandTotalRowMapper.Mappings);
        }
    }
}