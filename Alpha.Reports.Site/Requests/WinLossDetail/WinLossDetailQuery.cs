namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using Models;

    public class WinLossDetailQuery : ReportBaseQuery
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ReportRange> ReportRanges { get; set; }

        public string Language { get; set; }

        public string SelectedProducts { get; set; }

        public string SortedColumn { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public string OldMainAppPath { get; set; }

        public virtual UserLevel UserLevel { get; }
    }
}