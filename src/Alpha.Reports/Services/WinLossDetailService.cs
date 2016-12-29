namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinlossDetails;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);
    }
}
