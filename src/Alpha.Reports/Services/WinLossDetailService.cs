namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinlossDetails;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<WinLossDetail> GetWinLossDetail(WinLossDetailParam param);
    }
}
