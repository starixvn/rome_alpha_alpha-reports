namespace Alpha.Reports.Services.WinLossDetail
{
    using Models;
    using Models.WinLossDetail;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<WinLossDetailModel> GetWinLossDetail(WinLossDetailParam param);
    }
}