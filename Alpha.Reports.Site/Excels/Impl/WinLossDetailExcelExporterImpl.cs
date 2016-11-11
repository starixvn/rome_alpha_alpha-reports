namespace Alpha.Reports.Site.Excels.Impl
{
    using System.IO;
    using NPOI.SS.UserModel;

    public class WinLossDetailExcelExporterImpl : BaseExcelExporter, WinLossDetailExcelExporter
    {
        public MemoryStream Member(string templatePath)
        {
            return null;
        }

        protected override void ExportHeader<T>(ISheet sheet, string title)
        {
            //sheet.CopyRow(TemplateSheet, TitleRow,)
            //var titleStyle = TemplateSheet.GetRow(TitleRow).GetCell(0);
            //titleStyle.CopyCellTo()
        }
    }
}