namespace Alpha.Reports.Site.Excels
{
    using ViewModels.WinLossDetails;

    public interface WinLossDetailExcelExporter
    {
        byte[] Export(string templatePath, WinLossDetailViewModel viewModel);
    }
}