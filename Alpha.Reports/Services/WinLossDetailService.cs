namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinlossDetail;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<MemberWinlossDetail> MemberWinLossDetail(WinLossDetailParam param);
    }
}
