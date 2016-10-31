namespace Alpha.Reports.DataAccess.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Data;
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
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            ReportData<T> reportData = null;
            IEnumerable<T> dataSource = null;
            T total = null;

            using (var obj = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = obj.QueryMultiple(criteria))
                {
                    dataSource = multi.Read<T>();
                    total = multi.Read<T>().FirstOrDefault();
                }

                reportData = new ReportData<T>(
                    dataSource,
                    criteria.PageIndex,
                    criteria.PageSize,
                    criteria.TotalRecords,
                    total);
            }

            return reportData;
        }
    }
}