namespace Alpha.Reports.Site.Requests.WinLossAnalysisChart
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartQuery :
        FromLast18MonthDateRangeQuery,
        IRequest<WinLossAnalysisChartViewModel>
    {
        public WinLossAnalysisChartQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }
    }
}