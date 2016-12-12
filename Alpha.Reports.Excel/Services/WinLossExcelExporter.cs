namespace Alpha.Reports.Excel.Services
{
    using Models.Winloss;
    using Params;

    public interface WinLossExcelExporter
    {
        byte[] Export(ExportExcelParam<WinLoss> param, ExportWinLossParam exportWinLossParam);
    }
}