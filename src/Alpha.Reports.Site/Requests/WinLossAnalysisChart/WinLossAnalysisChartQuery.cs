namespace Alpha.Reports.Site.Requests.WinLossAnalysisChart
{
    using Extensions;
    using MediatR;
    using Reports.Models;
    using Utils;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartQuery :
        FromLast18MonthDateRangeQuery,
        IRequest<WinLossAnalysisChartViewModel>
    {
        public WinLossAnalysisChartQuery(IUser viewer) : base(viewer)
        {
            FromDate = SystemTime.Today.AddMonths(-2).FirstDayOfMonth();
            ToDate = SystemTime.Today;
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }
    }
}