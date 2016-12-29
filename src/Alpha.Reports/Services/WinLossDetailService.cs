namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinLossDetail;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);
    }
}
