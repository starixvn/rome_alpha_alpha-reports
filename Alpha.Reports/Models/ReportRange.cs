namespace Alpha.Reports.Models
{
    using System.Collections.Generic;

    public class ReportRange
    {
        public ReportRange(byte range, IEnumerable<string> reports)
        {
            Range = range;
            Reports = reports;
        }

        public byte Range { get; }

        public IEnumerable<string> Reports { get; }
    }
}
