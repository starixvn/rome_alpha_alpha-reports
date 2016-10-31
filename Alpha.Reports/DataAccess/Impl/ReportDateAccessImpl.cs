namespace Alpha.Reports.DataAccess.Impl
{
    using System;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class ReportDateAccessImpl : ReportDateAccess
    {
        public DateRange GetReportDateRange(ICriteria criteria)
        {
            DateRange dateRange = null;

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var minDate = Converter.ToDateTime(reader["StartDate"], DateTime.MinValue);
                        var maxDate = Converter.ToDateTime(reader["EndDate"], DateTime.MaxValue);
                        dateRange = new DateRange(minDate, maxDate);
                    }
                }
            }

            return dateRange;
        }
    }
}
