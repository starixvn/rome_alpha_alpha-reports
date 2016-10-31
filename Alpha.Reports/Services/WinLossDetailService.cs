namespace Alpha.Reports.Services
{
    using Models;
    using Models.WinlossDetail;
    using Params;

    public interface WinLossDetailService
    {
        ReportData<MasterWinLossDetail> MasterWinLossDetail(WinLossDetailParam param);

        ReportData<AgentWinLossDetail> AgentWinLossDetail(WinLossDetailParam param);

        ReportData<MemberWinLossDetail> MemberWinLossDetail(WinLossDetailParam param);
    }
}
