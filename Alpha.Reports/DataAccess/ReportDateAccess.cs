namespace Alpha.Reports.DataAccess
{
    using Fanex.Data.Repository;
    using Models;

    public interface ReportDateAccess
    {
        DateRange GetReportDateRange(ICriteria criteria);
    }
}
