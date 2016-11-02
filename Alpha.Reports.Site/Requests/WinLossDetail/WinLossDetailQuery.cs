namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using Models;

    public class WinLossDetailQuery : BaseReportQuery
    {
        public string Language { get; set; }

        public string SelectedProducts { get; set; }

        public string SortedColumn { get; set; }

        public int SortDirection { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public int SelectedDateRange { get; set; }

        public virtual UserLevel UserLevel { get; }
    }
}