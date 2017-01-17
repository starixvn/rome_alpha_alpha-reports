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
            Mapper<BetTypeWinLossViewModel> totalRowMapper);
    }

    public class MatchWinLossDetailWorkbookImpl :
        NpoiWorkbook<BetTypeWinLossViewModel>,
        MatchWinLossDetailWorkbook
    {
        private const int ReportSheetIndex = 1;

        private IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail;
        private BetTypeWinLossViewModel noParlayWinLossTotal;
        private BetTypeWinLossViewModel mixParlay;
        private BetTypeWinLossViewModel systemParlay;
        private BetTypeWinLossViewModel grandTotal;

        private Row matchTitleRowTemplate;
        private Row oddRowTemplate;
        private Row evenRowTemplate;
        private Row subTotalRowTemplate;
        private Row totalRowTemplate;
        private Row mixParlayRowTemplate;
        private Row systemParlayRowTemplate;
        private Row grandTotalRowTemplate;

        private Mapper<BetTypeWinLossViewModel> betTypeRowMapper;
        private Mapper<BetTypeWinLossViewModel> subTotalRowMapper;
        private Mapper<BetTypeWinLossViewModel> totalRowMapper;

        private int currentRowIndex;

        public MatchWinLossDetailWorkbook SetData(
            IEnumerable<MatchWinLossDetailViewModel> noParlayMatchWinLossDetail,
            BetTypeWinLossViewModel noParlayWinLossTotal,
            BetTypeWinLossViewModel mixParlay,
            BetTypeWinLossViewModel systemParlay,
            BetTypeWinLossViewModel grandTotal)
        {
            this.noParlayMatchWinLossDetail = noParlayMatchWinLossDetail;
            this.noParlayWinLossTotal = noParlayWinLossTotal;
            this.mixParlay = mixParlay;
            this.systemParlay = systemParlay;
            this.grandTotal = grandTotal;

            return this;
        }

        public MatchWinLossDetailWorkbook SetRowMappers(
            Mapper<BetTypeWinLossViewModel> betTypeRowMapper,
            Mapper<BetTypeWinLossViewModel> subTotalRowMapper,
            Mapper<BetTypeWinLossViewModel> totalRowMapper)
        {
            this.betTypeRowMapper = betTypeRowMapper;
            this.subTotalRowMapper = subTotalRowMapper;
            this.totalRowMapper = totalRowMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            matchTitleRowTemplate = sheet.GetRow(3);
            oddRowTemplate = sheet.GetRow(4);
            evenRowTemplate = sheet.GetRow(5);
            subTotalRowTemplate = sheet.GetRow(6);
            totalRowTemplate = sheet.GetRow(7);
            mixParlayRowTemplate = sheet.GetRow(8);
            systemParlayRowTemplate = sheet.GetRow(9);
            grandTotalRowTemplate = sheet.GetRow(10);

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
                matchTitleRowTemplate.LastCellIndex - 1,
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

            var hasData = (noParlayMatchWinLossDetail != null && noParlayMatchWinLossDetail.Any())
                || mixParlay != null
                || systemParlay != null;

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

            foreach (var data in noParlayMatchWinLossDetail)
            {
                FillMatchTitle(sheet, currentRowIndex++, data);

                var isEvenRow = false;
                foreach (var betTypeWinLoss in data.BetTypeWinLoss)
                {
                    FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betTypeWinLoss);
                    isEvenRow = !isEvenRow;
                }

                FillMatchTotal(sheet, currentRowIndex++, data.Total);
            }

            if (noParlayWinLossTotal != null &&
                (mixParlay != null || systemParlay != null))
            {
                FillTotal(sheet, currentRowIndex++, noParlayWinLossTotal);
            }

            if (mixParlay != null)
            {
                FillMixParlayWinLoss(sheet, currentRowIndex++);
            }

            if (systemParlay != null)
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
            row.CopyCellsFrom(grandTotalRowTemplate);
            FillCells(row, grandTotal, totalRowMapper.Mappings);
        }

        private void FillMatchTitle(Sheet sheet, int rowIndex, MatchWinLossDetailViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(matchTitleRowTemplate);

            var title = $"{data.LeagueName} >> {data.MatchName}".ToUpper(CultureInfo.InvariantCulture);
            sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, rowIndex, row.LastCellIndex - 1, title);
        }

        private void FillBetTypeWinLoss(
            Sheet sheet,
            int rowIndex,
            bool isEvenRow,
            BetTypeWinLossViewModel betTypeWinLoss)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? evenRowTemplate : oddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, betTypeWinLoss, betTypeRowMapper.Mappings);
        }

        private void FillMatchTotal(Sheet sheet, int rowIndex, BetTypeWinLossViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(subTotalRowTemplate);
            FillCells(row, data, subTotalRowMapper.Mappings);
        }

        private void FillTotal(Sheet sheet, int rowIndex, BetTypeWinLossViewModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(totalRowTemplate);
            FillCells(row, data, totalRowMapper.Mappings);
        }

        private void FillMixParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(mixParlayRowTemplate);
            FillCells(row, mixParlay, betTypeRowMapper.Mappings);
        }

        private void FillSystemParlayWinLoss(Sheet sheet, int rowIndex)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(systemParlayRowTemplate);
            FillCells(row, systemParlay, betTypeRowMapper.Mappings);
        }
    }
}