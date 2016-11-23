namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Constants;
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

        private const int MaxRowPerSheet = DefaultVariables.MaxRowExcelFile - 10;
        private const string DefaultSheetName = "Report";

        private int dataLength = 0;
        private List<object> dataSource;

        protected int EndColIndex { get; set; }

        protected IRow EvenRowTemplate { get; set; }

        protected IRow OddRowTemplate { get; set; }

        protected IRow TotalRowTemplate { get; set; }

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

            var numberOfSheet = (dataLength / MaxRowPerSheet) + (dataLength % MaxRowPerSheet != 0 ? 1 : 0);

            if (numberOfSheet == 0)
            {
                ExportSheet(workbook, 1, 0, param);
            }
            else
            {
                for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
                {
                    ExportSheet(workbook, numberOfSheet, sheetIndex, param);
                }
            }

            workbook.SetActiveSheet(0);

            return workbook.ToBytes();
        }

        protected abstract void ExportHeader(ISheet sheet);

        protected abstract IEnumerable<object> GetRowValues(dynamic genericData);

        protected virtual void LoadTemplate(HSSFWorkbook workbook)
        {
            var templateSheet = workbook.GetSheet(TemplateSheetName);

            OddRowTemplate = templateSheet.GetRow(3);
            EvenRowTemplate = templateSheet.GetRow(4);
            TotalRowTemplate = templateSheet.GetRow(5);

            workbook.RemoveSheetAt(1);

            EndColIndex = OddRowTemplate.LastCellNum - 1;
        }

        protected virtual void ExportRow<T>(ISheet sheet, IRow row, T genericData, int dataIndex, bool isEvenRow = false)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var rowTemplate = isEvenRow ? EvenRowTemplate : OddRowTemplate;
            rowTemplate.CopyCells(row);
            row.Height = rowTemplate.Height;

            row.SetCellValuesByValueList(StartColIndex, GetRowValues(genericData));
        }

        protected virtual void ExportTotalRow<T>(ISheet sheet, IRow row, T genericData)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            TotalRowTemplate.CopyCells(row);
            row.Height = TotalRowTemplate.Height;

            row.SetCellValuesByValueList(StartColIndex, GetRowValues(genericData));
            row.SetCellValue(StartColIndex, Resources.Total);
        }

        private void ExportSheet<T>(HSSFWorkbook workbook, int numberOfSheet, int sheetIndex, ExportExcelParam<T> param) where T : class
        {
            var resultSheet = workbook.GetSheet(DefaultSheetName);

            var sheetNameByPage = numberOfSheet == 1 ? param.SheetName : $"{param.SheetName} - {sheetIndex + 1}";
            workbook.SetSheetName(sheetIndex, sheetNameByPage);

            if (sheetIndex < numberOfSheet - 1)
            {
                workbook.CloneSheet(sheetIndex);
                workbook.SetSheetName(sheetIndex + 1, DefaultSheetName);
            }

            ExportTitle(resultSheet, param);
            ExportHeader(resultSheet);

            if (param.ReportData.Any())
            {
                ExportBody(resultSheet, sheetIndex, param);
            }
            else
            {
                var dataRowIndex = StartRowIndex + 3;
                resultSheet.SetCellRangeValue(dataRowIndex, StartColIndex, dataRowIndex, StartColIndex + EndColIndex, Resources.NoInformationIsAvailable);
                resultSheet.GetCell(dataRowIndex, StartColIndex).CellStyle.Alignment = HorizontalAlignment.Center;
            }
        }

        private void ExportTitle<T>(ISheet sheet, ExportExcelParam<T> param) where T : class
        {
            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}";

            sheet.SetCellValue(TitleRowIndex, StartColIndex, reportTitle);
        }

        protected virtual void ExportBody<T>(ISheet sheet, int sheetIndex, ExportExcelParam<T> param) where T : class
        {
            if (param == null)
            {
                throw new ArgumentNullException("query");
            }

            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var range = GetExportRange(dataLength, sheetIndex);
            var startIndex = range[0];
            var endIndex = range[1];

            var currentRowIndex = StartRowIndex + 3;
            var isEvenRow = false;
            IRow currentRow;

            for (var i = startIndex; i < endIndex; i++, currentRowIndex++)
            {
                currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
                ExportRow(sheet, currentRow, dataSource[i], i, isEvenRow);
                isEvenRow = !isEvenRow;
            }

            if (param.ReportData.TotalData != null)
            {
                currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
                ExportTotalRow(sheet, currentRow, param.ReportData.TotalData);
            }
        }

        private int[] GetExportRange(int dataLength, int sheetIndex)
        {
            var startIndex = sheetIndex * MaxRowPerSheet;
            var endIndex = startIndex + MaxRowPerSheet;

            if (endIndex > dataLength)
            {
                endIndex = dataLength;
            }

            return new int[] { startIndex, endIndex };
        }
    }
}
