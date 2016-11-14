namespace Alpha.Reports.DataAccesses
{
    using Criteria.WinLossDetail;
    using Models;
    using Models.WinlossDetails;

    public interface WinLossDetailDataAccess
    {
        ReportData<WinLossDetail> GetReport(WinLossDetailCriteria criteria);
    }
}