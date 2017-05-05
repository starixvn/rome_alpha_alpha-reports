namespace Alpha.Reports.Site.Handlers.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;

    public interface WinLossByProductWorkbook
    {
        Workbook<WinLossByBetTypeModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<WinLossByBetTypeModel> Sheet(string sheetName);

        Workbook<WinLossByBetTypeModel> Title(string titleText);

        Workbook<WinLossByBetTypeModel> Header(Func<string, string> headerMapping);

        Workbook<WinLossByBetTypeModel> Body(IEnumerable<WinLossByBetTypeModel> rows, Mapper<WinLossByBetTypeModel> mapper);

        byte[] ExportToBytes();

        WinLossByProductWorkbook SetData(
            IEnumerable<WinLossByProductModel> winLossByProductData,
            WinLossByBetTypeModel total);

        WinLossByProductWorkbook SetRowMappers(
            Mapper<WinLossByBetTypeModel> winLossByBetTypeMapper,
            Mapper<WinLossByBetTypeModel> totalMapper);
    }

    public class WinLossByProductWorkbookImpl :
        NpoiWorkbook<WinLossByBetTypeModel>,
        WinLossByProductWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<WinLossByProductModel> WinLossByProductData { get; set; }

        internal WinLossByBetTypeModel Total { get; set; }

        internal Mapper<WinLossByBetTypeModel> WinLossByBetTypeMapper { get; set; }

        internal Mapper<WinLossByBetTypeModel> TotalMapper { get; set; }

        internal Row WinLossByProductRowTemplate { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public WinLossByProductWorkbook SetData(
            IEnumerable<WinLossByProductModel> winLossByProductData,
            WinLossByBetTypeModel total)
        {
            WinLossByProductData = winLossByProductData;
            Total = total;

            return this;
        }

        public WinLossByProductWorkbook SetRowMappers(
            Mapper<WinLossByBetTypeModel> winLossByBetTypeMapper,
            Mapper<WinLossByBetTypeModel> totalMapper)
        {
            WinLossByBetTypeMapper = winLossByBetTypeMapper;
            TotalMapper = totalMapper;

            return this;
        }

        internal void FillProductTitle(Sheet sheet, int rowIndex, WinLossByProductModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(WinLossByProductRowTemplate);
            FillCells(row, data.Total, TotalMapper.Mappings);
            row.SetCellValue(row.FirstCellIndex, data.ProductName.ToUpperInvariant());
        }

        internal void FillWinLossByBetType(
            Sheet sheet,
            int rowIndex,
            bool isEvenRow,
            WinLossByBetTypeModel winLossByBetTypeModel)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, winLossByBetTypeModel, WinLossByBetTypeMapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            WinLossByProductRowTemplate = sheet.GetRow(currentRowIndex++);
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.OddRowTemplate = OddRowTemplate;
            reportSheet.EvenRowTemplate = EvenRowTemplate;
            reportSheet.FooterRowsTemplate = new List<Row> { TotalRowTemplate };
            return reportSheet;
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (WinLossByProductData.Any())
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

            foreach (var data in WinLossByProductData)
            {
                FillProductTitle(sheet, currentRowIndex++, data);

                var isEvenRow = false;
                foreach (var betTypeWinLoss in data.WinLossByBetTypeData)
                {
                    FillWinLossByBetType(sheet, currentRowIndex++, isEvenRow, betTypeWinLoss);
                    isEvenRow = !isEvenRow;
                }
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
            row.CopyCellsFrom(TotalRowTemplate);
            FillCells(row, Total, TotalMapper.Mappings);
        }
    }
}