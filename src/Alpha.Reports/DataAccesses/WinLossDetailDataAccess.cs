namespace Alpha.Reports.DataAccesses
{
    using Criteria.WinLossDetail;
    using Models;
    using Models.WinLossDetail;

    public interface WinLossDetailDataAccess
    {
        ReportData<WinLossDetailModel> GetData(WinLossDetailCriteria criteria);
    }
}