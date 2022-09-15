namespace Alpha.Reports.Site.Features.EnteredResult
{
    using Alpha.Reports.Models.Winloss;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface EnteredSportResultWorkbook
    {
        Workbook<EnteredSportResult> UseTemplate(TemplateInfo templateInfo);

        Workbook<EnteredSportResult> Sheet(string sheetName);

        Workbook<EnteredSportResult> Title(string titleText);

        Workbook<EnteredSportResult> Header(Func<string, string> headerMapping);

        Workbook<EnteredSportResult> Body(
            IEnumerable<EnteredSportResult> rows,
            Mapper<EnteredSportResult> mapper);

        byte[] ExportToBytes();

        EnteredSportResultWorkbook SetData(
            IEnumerable<EnteredSportResult> reportData,
            EnteredSportResult totalEnteredSportResult);

        EnteredSportResultWorkbook SetRowMappers(Mapper<EnteredSportResult> mapper);
    }

    public class EnteredSportResultWorkbookImpl :
        NpoiWorkbook<EnteredSportResult>,
        EnteredSportResultWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<EnteredSportResult> ReportData { get; set; }

        internal EnteredSportResult TotalEnteredSportResult { get; set; }

        internal Mapper<EnteredSportResult> Mapper { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public EnteredSportResultWorkbook SetData(
            IEnumerable<EnteredSportResult> reportData,
            EnteredSportResult totalEnteredSportResult)
        {
            ReportData = reportData;
            TotalEnteredSportResult = totalEnteredSportResult;

            return this;
        }

        public EnteredSportResultWorkbook SetRowMappers(Mapper<EnteredSportResult> mapper)
        {
            Mapper = mapper;

            return this;
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            TotalRowTemplate = sheet.GetRow(currentRowIndex++);
            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);

            return reportSheet;
        }

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
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            FillTotalRow(sheet, currentRowIndex++, TotalEnteredSportResult);
            
            var isEvenRow = false;
            foreach (var data in ReportData)
            {
                FillRow(sheet, currentRowIndex++, isEvenRow, data);
                isEvenRow = !isEvenRow;
            }
        }

        internal void FillRow(Sheet sheet, int rowIndex, bool isEvenRow, EnteredSportResult data)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, data, Mapper.Mappings);
        }

        internal void FillTotalRow(Sheet sheet, int rowIndex, EnteredSportResult data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(TotalRowTemplate);
            FillCells(row, data, Mapper.Mappings);
        }
    }
}