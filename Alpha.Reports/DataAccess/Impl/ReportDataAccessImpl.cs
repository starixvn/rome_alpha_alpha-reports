namespace Alpha.Reports.DataAccess.Impl
{
    using System.Collections.Generic;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;

    public class ReportDataAccessImpl : ReportDataAccess
    {
        private readonly IDynamicRepository repository;

        public ReportDataAccessImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public ReportData<T> GetData<T>(ReportCriteriaBase criteria) where T : class
        {
            var dataSource = repository.Get<List<T>>(criteria);
            var reportData = new ReportData<T>(dataSource, criteria.PageIndex, criteria.TotalRecords, criteria.FromDate, criteria.ToDate);

            return reportData;
        }
    }
}