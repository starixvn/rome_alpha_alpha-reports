namespace Alpha.Reports.DataAccesses
{
    using Criteria.WinLossDetail;
    using Models;
    using Models.WinlossDetails;

    public interface WinLossDetailDataAccess
    {
        ReportData<WinLossDetailModel> GetData(WinLossDetailCriteria criteria);
    }
}