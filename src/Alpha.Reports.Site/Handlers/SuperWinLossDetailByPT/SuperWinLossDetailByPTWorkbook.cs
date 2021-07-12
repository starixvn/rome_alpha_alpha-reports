namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using Alpha.Reports.Models.SuperWinLossDetailByPT;
    using App_GlobalResources;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface SuperWinLossDetailByPTWorkbook
    {
        Workbook<SuperWinLossDetailModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<SuperWinLossDetailModel> Sheet(string sheetName);

        Workbook<SuperWinLossDetailModel> Title(string titleText);

        Workbook<SuperWinLossDetailModel> Header(Func<string, string> headerMapping);

        Workbook<SuperWinLossDetailModel> Body(
            IEnumerable<SuperWinLossDetailModel> rows,
            Mapper<SuperWinLossDetailModel> mapper);

        byte[] ExportToBytes();

        SuperWinLossDetailByPTWorkbook SetData(
            IEnumerable<SuperWinLossDetailByPTModel> reportData,
            SuperWinLossDetailModel total);

        SuperWinLossDetailByPTWorkbook SetRowMappers(
            Mapper<SuperWinLossDetailModel> mapper,
            Mapper<SuperWinLossDetailModel> totalMapper);
    }

    public class SuperWinLossDetailByPTWorkbookImpl :
        NpoiWorkbook<SuperWinLossDetailModel>,
        SuperWinLossDetailByPTWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<SuperWinLossDetailByPTModel> ReportData { get; set; }

        internal SuperWinLossDetailModel Total { get; set; }

        internal Mapper<SuperWinLossDetailModel> Mapper { get; set; }

        internal Mapper<SuperWinLossDetailModel> TotalMapper { get; set; }

        internal Row WinLossDetailByPTRowTemplate { get; set; }

        internal Row WinLossDetailByProductRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public SuperWinLossDetailByPTWorkbook SetData(
            IEnumerable<SuperWinLossDetailByPTModel> reportData,
            SuperWinLossDetailModel total)
        {
            ReportData = reportData;
            Total = total;

            return this;
        }

        public SuperWinLossDetailByPTWorkbook SetRowMappers(
            Mapper<SuperWinLossDetailModel> mapper,
            Mapper<SuperWinLossDetailModel> totalMapper)
        {
            Mapper = mapper;
            TotalMapper = totalMapper;

            return this;
        }

        internal void FillWinLossDetailByPT(Sheet sheet, int rowIndex, SuperWinLossDetailByPTModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(WinLossDetailByPTRowTemplate);
            FillCells(row, data.Total, TotalMapper.Mappings);
            row.SetCellValue(row.FirstCellIndex, string.Format(data.PositionTakingText, Resources.PT));
        }

        internal void FillWinLossDetailByProduct(Sheet sheet, int rowIndex, SuperWinLossDetailModel item)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(WinLossDetailByProductRowTemplate);
            FillCells(row, item, Mapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            WinLossDetailByPTRowTemplate = sheet.GetRow(currentRowIndex++);
            WinLossDetailByProductRowTemplate = sheet.GetRow(currentRowIndex++);
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.EvenRowTemplate = WinLossDetailByPTRowTemplate;
            reportSheet.OddRowTemplate = WinLossDetailByProductRowTemplate;
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

            if (ReportData.Any())
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

            foreach (var data in ReportData)
            {
                FillWinLossDetailByPT(sheet, currentRowIndex++, data);

                foreach (var winLossDetail in data.SuperWinLossDetails)
                {
                    FillWinLossDetailByProduct(sheet, currentRowIndex++, winLossDetail);
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