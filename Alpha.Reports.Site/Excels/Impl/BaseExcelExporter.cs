namespace Alpha.Reports.Site.Excels.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Reports.Extensions;
    using Requests;

    public abstract class BaseExcelExporter
    {
        protected const int StartRowIndex = 1;
        protected const int StartColIndex = 0;

        protected const int OriginTemplateRowIndex = 1;
        protected const int ResultTemplateRowIndex = 0;
        protected const int TitleRowIndex = 1;

        protected const int TitleColIndex = 0;
        protected const int HeaderColIndex = 1;
        protected const int EvenRowNumberIndex = 3;
        protected const int OddRowNumberIndex = 4;
        protected const int EvenRowTextIndex = 5;
        protected const int OddRowTextIndex = 6;
        protected const int TotalRowNumberIndex = 7;
        protected const int TotalRowTextIndex = 8;
        protected const int EvenRowTotalNumberIndex = 9;
        protected const int OddRowTotalNumberIndex = 10;

        private const int MaxRowPerSheet = 1;
        private const string DefaultSheetName = "ReportSheet";

        protected ISheet TemplateSheet { get; set; }

        protected IRow TemplateRow { get; set; }

        protected HSSFWorkbook LoadTemplate(string templatePath)
        {
            using (FileStream file = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(file);

                return workbook;
            }
        }

        protected byte[] Export<T>(ExportExcelQuery<T> query) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var workbook = LoadTemplate(query.TemplatePath);

            TemplateSheet = workbook.GetSheet("Template");

            var numberOfRecords = query.ReportData.TotalRecords;
            var numberOfSheet = (numberOfRecords / MaxRowPerSheet) + (numberOfRecords % MaxRowPerSheet != 0 ? 1 : 0);

            if (numberOfSheet == 0)
            {
                ExportSheet(workbook, numberOfSheet, 0, query);
            }
            else
            {
                for (var sheetIndex = 1; sheetIndex <= numberOfSheet; sheetIndex++)
                {
                    ExportSheet(workbook, numberOfSheet, sheetIndex, query);
                }
            }

            workbook.RemoveSheetAt(workbook.GetSheetIndex(TemplateSheet));
            workbook.SetActiveSheet(0);

            return ConvertExcelFileToByteData(workbook);
        }

        protected abstract void ExportHeader(ISheet sheet, int maxColNumber);

        protected virtual void ExportBody<T>(ISheet sheet, int sheetIndex, ExportExcelQuery<T> query) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var range = GetExportRange(query.DataLength, sheetIndex);
            var startIndex = range[0];
            var endIndex = range[1];

            var currentRowIndex = StartRowIndex + 3;

            var isEvenRow = false;
            var listData = query.ReportData.DataSource.ToList();

            IRow currentRow;

            for (var i = startIndex; i < endIndex; i++, currentRowIndex++)
            {
                currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
                ExportRow(currentRow, listData[i], i, query.MaxColNumber, isEvenRow);

                isEvenRow = !isEvenRow;
            }

            currentRow = sheet.GetRow(currentRowIndex) ?? sheet.CreateRow(currentRowIndex);
            ExportTotalRow(currentRow, query.ReportData.TotalData, query.MaxColNumber);
        }

        protected abstract void ExportRow<T>(IRow row, T genericData, int dataIndex, int maxColNumber, bool isEvenRow = false);

        protected abstract void ExportTotalRow<T>(IRow row, T genericData, int maxColNumber);

        private byte[] ConvertExcelFileToByteData(HSSFWorkbook workbook)
        {
            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);

                return memoryStream.GetBuffer();
            }
        }

        private void ExportTitle<T>(ISheet sheet, ExportExcelQuery<T> query) where T : class
        {
            var reportTitle = $"{query.Title} {query.FromDate.ToShortDateWithSlash()} --> {query.ToDate.ToShortDateWithSlash()}";
            var templateCell = TemplateRow.GetCell(TitleColIndex);

            templateCell.CopyCell(TitleRowIndex, StartColIndex, query.MaxColNumber);
            sheet.GetRow(TitleRowIndex).WriteCell(StartColIndex, reportTitle);
            templateCell.Sheet.MergeCells(TitleRowIndex, StartColIndex, TitleRowIndex, StartColIndex + query.MaxColNumber - 1);
        }

        private void ExportSheet<T>(HSSFWorkbook workbook, int numberOfSheet, int sheetIndex, ExportExcelQuery<T> query) where T : class
        {
            ISheet resultSheet = workbook.GetSheet(DefaultSheetName);
            var sheetNameByPage = numberOfSheet == 1 ? query.SheetName : $"{query.SheetName} - {sheetIndex}";
            workbook.SetSheetName(sheetIndex, sheetNameByPage);

            if (sheetIndex != numberOfSheet)
            {
                workbook.CloneSheet(sheetIndex);
                workbook.SetSheetName(sheetIndex + 1, DefaultSheetName);
            }

            TemplateSheet.CopyRow(resultSheet, OriginTemplateRowIndex, ResultTemplateRowIndex);
            TemplateRow = resultSheet.GetRow(ResultTemplateRowIndex);

            ExportTitle(resultSheet, query);
            ExportHeader(resultSheet, query.MaxColNumber);

            if (query.ReportData.Any())
            {
                ExportBody(resultSheet, sheetIndex, query);
            }
            else
            {
                var noInfoStyleIndex = 2;
                var templateCell = TemplateRow.GetCell(noInfoStyleIndex);
                var dataRowIndex = StartRowIndex + 3;

                templateCell.CopyCell(dataRowIndex, StartColIndex, query.MaxColNumber);
                resultSheet.MergeCells(dataRowIndex, StartColIndex, dataRowIndex, StartColIndex + query.MaxColNumber - 1);
                resultSheet.GetRow(dataRowIndex).WriteCell(StartColIndex, Resources.NoInformationIsAvailable);
            }

            // Remove template row
            // resultSheet.RemoveRow(TemplateRow); : not work
            resultSheet.ShiftRows(1, resultSheet.LastRowNum, -1);
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