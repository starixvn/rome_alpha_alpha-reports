namespace Alpha.Reports.DataAccess
{
    using Criteria;
    using Criteria.WinLossDetail;
    using Models;

    public interface WinLossDetailDataAccess
    {
        ReportData<T> GetData<T>(WinLossDetailCriteria criteria) where T : class;
    }
}
