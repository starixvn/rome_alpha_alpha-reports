namespace Alpha.Reports.Site.Excels.Impl
{
    using System.IO;
    using Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Requests;

    public abstract class BaseExcelExporter
    {
        protected const int StartRow = 0;
        protected const int StartColumn = 0;

        protected const int TitleRow = 1;

        private const int MaxRowPerSheet = 3000;

        protected ISheet TemplateSheet { get; set; }

        protected HSSFWorkbook LoadTemplate(string templatePath)
        {
            FileStream file = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
            var workbook = new HSSFWorkbook(file);

            return workbook;
        }

        protected byte[] Export<T>(string templatePath, ExportExcelQuery<T> query) where T : class
        {
            var workbook = LoadTemplate(templatePath);

            var reportTitle = $"{query.Tilte} {query.FromDate.ToShortDateWithSlash()} --> {query.ToDate.ToShortDateWithSlash()}";

            TemplateSheet = workbook.GetSheet("Template");

            var numberOfRecords = query.ReportData.TotalRecords;
            var numberOfSheet = (numberOfRecords / MaxRowPerSheet) + (numberOfRecords % MaxRowPerSheet != 0 ? 1 : 0);
            if (query.ReportData.Any())
            {
            }
            else
            {
                var resultSheet = workbook.GetSheet("ReportSheet");
                ExportHeader(resultSheet, reportTitle);
            }

            return ConvertExcelFileToByteData(workbook);
        }

        protected void CopyRow(ISheet sourceSheet, ISheet targetSheet, int sourceRowIndex, int destinateRowIndex)
        {
            AppendMoreRows(targetSheet, destinateRowIndex);
        }

        protected abstract void ExportHeader(ISheet sheet, string title);

        private byte[] ConvertExcelFileToByteData(HSSFWorkbook workbook)
        {
            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);

                return memoryStream.GetBuffer();
            }
        }

        private void AppendMoreRows(ISheet sheet, int destinateRowIndex)
        {
            if (destinateRowIndex <= sheet.LastRowNum)
            {
                return;
            }

            for (var i = sheet.LastRowNum + 1; i <= destinateRowIndex; i++)
            {
                sheet.CreateRow(i);
            }
        }
    }
}