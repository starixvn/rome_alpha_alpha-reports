namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using Constants;
    using Extensions;
    using Models;
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

        private const int MaxRowPerSheet = DefaultVariables.MaxRowExcelFile - 10;
        private const string DefaultSheetName = "Report";

        private int dataLength = 0;
        private List<object> dataSource;

        protected int EndColIndex { get; set; }

        protected TemplateSheetModel TemplateSheetModel { get; set; }

        protected byte[] Export<T>(ExportExcelParam<T> param) where T : class
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            var workbook = HSSFWorkbookExtension.Load(param.TemplatePath);

            dataLength = param.ReportData.DataSource.Count();
            dataSource = param.ReportData.DataSource.Select(t => (object)t).ToList();

            var numberOfSheet = (dataLength / MaxRowPerSheet) + (dataLength % MaxRowPerSheet != 0 ? 1 : 0);

            EndColIndex = StartColIndex + param.MaxColNumber - 1;
            TemplateSheetModel = new TemplateSheetModel(workbook);

            if (numberOfSheet == 0)
            {
                ExportSheet(workbook, 1, 1, param);
            }
            else
            {
                for (var sheetIndex = 1; sheetIndex <= numberOfSheet; sheetIndex++)
                {
                    ExportSheet(workbook, numberOfSheet, sheetIndex, param);
                }
            }

            workbook.RemoveSheetAt(0);
            workbook.SetActiveSheet(0);

            return workbook.ToBytes();
        }

        protected abstract void ExportHeader(ISheet sheet);

        protected abstract void ExportRow<T>(ISheet sheet, IRow row, T genericData, int dataIndex, bool isEvenRow = false);

        protected abstract void ExportTotalRow<T>(ISheet sheet, IRow row, T genericData);

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

        private void ExportSheet<T>(HSSFWorkbook workbook, int numberOfSheet, int sheetIndex, ExportExcelParam<T> param) where T : class
        {
            var resultSheet = workbook.GetSheet(DefaultSheetName);
            var sheetNameByPage = numberOfSheet == 1 ? param.SheetName : $"{param.SheetName} - {sheetIndex}";
            workbook.SetSheetName(sheetIndex, sheetNameByPage);

            if (sheetIndex != numberOfSheet)
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
                TemplateSheetModel.NoInfo.CopyToMergedCells(resultSheet, dataRowIndex, StartColIndex, EndColIndex);
                resultSheet.SetCellValue(dataRowIndex, StartColIndex, Resources.NoInformationIsAvailable);
            }
        }

        private void ExportTitle<T>(ISheet sheet, ExportExcelParam<T> param) where T : class
        {
            var reportTitle = $"{param.Title} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}";

            sheet.SetCellValue(TitleRowIndex, StartColIndex, reportTitle);
        }

        private int[] GetExportRange(int dataLength, int sheetIndex)
        {
            var startIndex = (sheetIndex - 1) * MaxRowPerSheet;
            var endIndex = startIndex + MaxRowPerSheet;

            if (endIndex > dataLength)
            {
                endIndex = dataLength;
            }

            return new int[] { startIndex, endIndex };
        }
    }
}
