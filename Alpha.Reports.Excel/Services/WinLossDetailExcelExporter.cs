namespace Alpha.Reports.Excel.Services
{
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossDetails;

    public interface WinLossDetailExcelExporter
    {
        byte[] Export(ExportExcelParam<WinLossDetail> param, UserLevel userLevel);
    }
}