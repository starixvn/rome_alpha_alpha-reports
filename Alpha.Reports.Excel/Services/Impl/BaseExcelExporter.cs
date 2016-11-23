namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOIExtensions;
    using Params;

    public abstract class BaseExcelExporter
    {
        protected const int TitleRowIndex = 0;
        protected const int HeaderRowIndex = 1;

        protected const int StartRowIndex = 0;
        protected const int StartColIndex = 0;
        protected const string TemplateSheetName = "Template";

        private const string DefaultSheetName = "Report";

        private int dataLength = 0;
        private List<object> dataSource;

        protected int EndColIndex { get; set; }

        protected IRow EvenRowTemplate { get; set; }

        protected IRow OddRowTemplate { get; set; }

        protected IRow TotalRowTemplate { get; set; }

        protected Func<dynamic, IEnumerable<object>> GetRowValuesFunc { get; set; }

        protected byte[] Export<T>(ExportExcelParam<T> param) where T : class
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var workbook = HSSFWorkbookExtension.Load(param.TemplatePath);

            LoadTemplate(workbook);

            dataLength = param.ReportData.DataSource.Count();
            dataSource = param.ReportData.DataSource.Select(t => (object)t).ToList();

            FillSheet(workbook, param);

            return workbook.ToBytes();
        }

        protected abstract void FillHeader(ISheet sheet);

        protected virtual void LoadTemplate(HSSFWorkbook workbook)
        {
            var templateSheet = workbook.GetSheet(TemplateSheetName);

            OddRowTemplate = templateSheet.GetRow(3);
            EvenRowTemplate = templateSheet.GetRow(4);
            TotalRowTemplate = templateSheet.GetRow(5);

            workbook.RemoveSheetAt(1);

            EndColIndex = OddRowTemplate.LastCellNum - 1;
        }

        protected virtual void FillBody<T>(ISheet sheet, ExportExcelParam<T> param) where T : class
        {
            if (param == null)
            {
                throw new ArgumentNullException("query");
            }

            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var currentRowIndex = StartRowIndex + 3;
            var isEvenRow = false;
            IRow currentRow;

            for (var i = 0; i < dataLength; i++, currentRowIndex++)
            {
                currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
                FillRow(sheet, currentRow, dataSource[i], i, isEvenRow);
                isEvenRow = !isEvenRow;
            }

            if (param.ReportData.TotalData != null)
            {
                currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
                FillTotalRow(sheet, currentRow, param.ReportData.TotalData);
            }
        }

        protected virtual void FillRow<T>(ISheet sheet, IRow row, T genericData, int dataIndex, bool isEvenRow = false)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var rowTemplate = isEvenRow ? EvenRowTemplate : OddRowTemplate;
            rowTemplate.CopyCells(row);
            row.Height = rowTemplate.Height;

            row.SetCellValuesByValueList(StartColIndex, GetRowValuesFunc(genericData));
        }

        protected virtual void FillTotalRow<T>(ISheet sheet, IRow row, T genericData)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            TotalRowTemplate.CopyCells(row);
            row.Height = TotalRowTemplate.Height;

            row.SetCellValuesByValueList(StartColIndex, GetRowValuesFunc(genericData));
            row.SetCellValue(StartColIndex, Resources.Total);
        }

        protected virtual void FreezePanel(ISheet sheet)
        {
        }

        private void FillSheet<T>(HSSFWorkbook workbook, ExportExcelParam<T> param) where T : class
        {
            var resultSheet = workbook.GetSheet(DefaultSheetName);

            workbook.SetSheetName(0, param.SheetName);

            FillTitle(resultSheet, param);
            FillHeader(resultSheet);

            if (param.ReportData.Any())
            {
                FillBody(resultSheet, param);
            }
            else
            {
                var dataRowIndex = StartRowIndex + 3;
                resultSheet.SetCellRangeValue(dataRowIndex, StartColIndex, dataRowIndex, StartColIndex + EndColIndex, Resources.NoInformationIsAvailable);
                resultSheet.GetCell(dataRowIndex, StartColIndex).CellStyle.Alignment = HorizontalAlignment.Center;
            }

            FreezePanel(resultSheet);
        }

        private void FillTitle<T>(ISheet sheet, ExportExcelParam<T> param) where T : class
        {
            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}";

            sheet.SetCellValue(TitleRowIndex, StartColIndex, reportTitle);
        }
    }
}
