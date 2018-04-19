namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Reports.Models.ProgressiveContribution;

    public interface ProgressiveContributionWorkbook
    {
        Workbook<ProgressiveContributionByBetTypeModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<ProgressiveContributionByBetTypeModel> Sheet(string sheetName);

        Workbook<ProgressiveContributionByBetTypeModel> Title(string titleText);

        Workbook<ProgressiveContributionByBetTypeModel> Header(Func<string, string> headerMapping);

        Workbook<ProgressiveContributionByBetTypeModel> Body(
            IEnumerable<ProgressiveContributionByBetTypeModel> rows,
            Mapper<ProgressiveContributionByBetTypeModel> mapper);

        byte[] ExportToBytes();

        ProgressiveContributionWorkbook SetData(
            IEnumerable<ProgressiveContributionByProductModel> progressiveContributions,
            ProgressiveContributionByBetTypeModel total);

        ProgressiveContributionWorkbook SetRowMappers(
            Mapper<ProgressiveContributionByBetTypeModel> mapper,
            Mapper<ProgressiveContributionByBetTypeModel> totalMapper);
    }

    public class ProgressiveContributionWorkbookImpl :
        NpoiWorkbook<ProgressiveContributionByBetTypeModel>,
        ProgressiveContributionWorkbook
    {
        private const int ReportSheetIndex = 1;
        private int currentRowIndex;

        internal IEnumerable<ProgressiveContributionByProductModel> ProgressiveContributions { get; set; }

        internal ProgressiveContributionByBetTypeModel Total { get; set; }

        internal Mapper<ProgressiveContributionByBetTypeModel> Mapper { get; set; }

        internal Mapper<ProgressiveContributionByBetTypeModel> TotalMapper { get; set; }

        internal Row ProgressiveContributionByProductRowTemplate { get; set; }

        internal Row OddRowTemplate { get; set; }

        internal Row EvenRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public ProgressiveContributionWorkbook SetData(
            IEnumerable<ProgressiveContributionByProductModel> progressiveContributions,
            ProgressiveContributionByBetTypeModel total)
        {
            ProgressiveContributions = progressiveContributions;
            Total = total;

            return this;
        }

        public ProgressiveContributionWorkbook SetRowMappers(
            Mapper<ProgressiveContributionByBetTypeModel> mapper,
            Mapper<ProgressiveContributionByBetTypeModel> totalMapper)
        {
            Mapper = mapper;
            TotalMapper = totalMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            ProgressiveContributionByProductRowTemplate = sheet.GetRow(currentRowIndex++);
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

            if (ProgressiveContributions.Any())
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

            foreach (var data in ProgressiveContributions)
            {
                FillProductTitle(sheet, currentRowIndex++, data);

                var isEvenRow = false;
                foreach (var progressiveContribution in data.ProgressiveContributions)
                {
                    FillProgressiveContribution(sheet, currentRowIndex++, isEvenRow, progressiveContribution);
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

        internal void FillProductTitle(Sheet sheet, int rowIndex, ProgressiveContributionByProductModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(ProgressiveContributionByProductRowTemplate);
            FillCells(row, data.Total, TotalMapper.Mappings);
            row.SetCellValue(row.FirstCellIndex, data.ProductName.ToUpperInvariant());
        }

        internal void FillProgressiveContribution(Sheet sheet, int rowIndex, bool isEvenRow, ProgressiveContributionByBetTypeModel data)
        {
            var row = sheet.GetRow(rowIndex);
            var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

            row.CopyCellsFrom(templateRow);
            FillCells(row, data, Mapper.Mappings);
        }
    }
}