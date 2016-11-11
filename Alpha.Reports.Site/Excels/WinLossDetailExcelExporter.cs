namespace Alpha.Reports.Site.Excels
{
    using System.IO;

    public interface WinLossDetailExcelExporter
    {
        MemoryStream Member(string templatePath);
    }
}