namespace Alpha.Reports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class ReportData<T> where T : class
    {
        public ReportData(
            IEnumerable<T> dataSource,
            T totalData)
        {
            DataSource = dataSource ?? Enumerable.Empty<T>();
            TotalData = totalData;
        }

        public IEnumerable<T> DataSource { get; }

        public T TotalData { get; }

        public virtual bool Any() => DataSource.Any();

        public virtual bool MoreThanOne() => DataSource.Count() > 1;
    }
}