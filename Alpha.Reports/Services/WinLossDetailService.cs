namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinlossDetail;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<T> GetWinLossDetail<T>(WinLossDetailParam param) where T : MemberWinLossDetail;
    }
}
