namespace Alpha.Reports.DataAccess.Impl
{
    using System;
    using System.Linq;
    using Criteria;
    using Fanex.Data;
    using Models;

    public class WinLossDetailDataAccessImpl : WinLossDetailDataAccess
    {
        public ReportData<T> GetData<T>(ReportCriteriaBase criteria) where T : class
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            ReportData<T> reportData;

            T total = null;

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataSource = Enumerable.Empty<T>();

                using (var reader = objectDb.QueryMultiple(criteria))
                {
                    dataSource = reader.Read<T>();                    
                    total = reader.Read<T>().FirstOrDefault();
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