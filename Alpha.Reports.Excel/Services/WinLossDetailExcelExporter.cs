namespace Alpha.Reports.Excel.Services
{
    using Models.WinlossDetails;
    using Params;

    public interface WinLossDetailExcelExporter
    {
        byte[] Export(ExportExcelParam<WinLossDetail> param, ExportWinLossDetailParam exportWinLossDetailParam);
    }
}