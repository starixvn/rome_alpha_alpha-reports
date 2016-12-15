namespace Alpha.Reports.Excel.Services
{
    using Models.Winloss;
    using Params;

    public interface WinLossExcelExporter
    {
        byte[] ExportWinLossExcel(ExportExcelParam<WinLoss> param, ExportWinLossParam reportParam);

        byte[] ExportEnteredResultExcel(ExportExcelParam<EnteredSportResult> param);
    }
}