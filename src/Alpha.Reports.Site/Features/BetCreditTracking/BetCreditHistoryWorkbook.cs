namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface BetCreditHistoryWorkbook
    {
        Workbook<BetCreditHistoryModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<BetCreditHistoryModel> Sheet(string sheetName);

        Workbook<BetCreditHistoryModel> Title(string titleText);

        Workbook<BetCreditHistoryModel> Header(Func<string, string> headerMapping);

        Workbook<BetCreditHistoryModel> Body(
            IEnumerable<BetCreditHistoryModel> rows,
            Mapper<BetCreditHistoryModel> mapper);

        byte[] ExportToBytes();

        BetCreditHistoryWorkbook SetData(IEnumerable<BetCreditHistoryModel> reportData);

        BetCreditHistoryWorkbook SetRowMappers(Mapper<BetCreditHistoryModel> mapper);
    }

    public class BetCreditHistoryWorkbookImpl :
        NpoiWorkbook<BetCreditHistoryModel>,
        BetCreditHistoryWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<BetCreditHistoryModel> ReportData { get; set; }

        internal Mapper<BetCreditHistoryModel> Mapper { get; set; }

        internal Row RowTemplate { get; set; }

        internal Row HighlightRowTemplate { get; set; }

        internal Row RedHighlightRowTemplate { get; set; }

        public BetCreditHistoryWorkbook SetData(IEnumerable<BetCreditHistoryModel> reportData)
        {
            ReportData = reportData;

            return this;
        }

        public BetCreditHistoryWorkbook SetRowMappers(Mapper<BetCreditHistoryModel> mapper)
        {
            Mapper = mapper;

            return this;
        }

        internal void FillHighlightRow(Sheet sheet, int rowIndex, BetCreditHistoryModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(HighlightRowTemplate);
            FillCells(row, data, Mapper.Mappings);
        }

        internal void FillRow(Sheet sheet, int rowIndex, BetCreditHistoryModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(RowTemplate);
            FillCells(row, data, Mapper.Mappings);
        }

        internal void FillRedHighlightRow(Sheet sheet, int rowIndex, BetCreditHistoryModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(RedHighlightRowTemplate);
            FillCells(row, data, Mapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            HighlightRowTemplate = sheet.GetRow(currentRowIndex++);
            RowTemplate = sheet.GetRow(currentRowIndex++);
            RedHighlightRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.EvenRowTemplate = HighlightRowTemplate;
            reportSheet.OddRowTemplate = RowTemplate;

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

            foreach (var data in ReportData)
            {
                if (data.BetCredit == null)
                {
                    FillHighlightRow(sheet, currentRowIndex++, data);
                    continue;
                }

                if (data.BetCredit < (decimal)0)
                {
                    FillRedHighlightRow(sheet, currentRowIndex++, data);
                    continue;
                }

                FillRow(sheet, currentRowIndex++, data);
            }
        }
    }
}