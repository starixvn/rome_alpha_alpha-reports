namespace Alpha.Reports.Site.Requests.WinLossAnalysisChart
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartQuery :
        ReportDefaultDateRangeQuery,
        IRequest<WinLossAnalysisChartViewModel>
    {
        public WinLossAnalysisChartQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}