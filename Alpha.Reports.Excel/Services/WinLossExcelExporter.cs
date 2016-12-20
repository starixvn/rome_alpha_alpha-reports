namespace Alpha.Reports.Excel.Services
{
    using Models.Winloss;
    using Params;

    public interface WinLossExcelExporter
    {
        byte[] ExportWinLossExcel(ExportWinLossParam reportParam);

        byte[] ExportEnteredResultExcel(ExportEnteredResultParam param);
    }
}