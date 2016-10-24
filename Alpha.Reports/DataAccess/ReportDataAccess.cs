namespace Alpha.Reports.DataAccess
{
    using Criteria;
    using Models;

    public interface ReportDataAccess
    {
        ReportData<T> GetData<T>(ReportCriteriaBase criteria) where T : class;
    }
}
