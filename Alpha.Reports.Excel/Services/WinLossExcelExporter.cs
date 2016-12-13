namespace Alpha.Reports.Excel.Services
{
    using Models.Winloss;
    using Params;

    public interface WinLossExcelExporter
    {
        byte[] ExportWinLossExcel(ExportExcelParam<WinLoss> param, ExportWinLossParam exportWinLossParam);

        byte[] ExportEnteredResultExcel(ExportExcelParam<EnteredSportResult> param);
    }
}