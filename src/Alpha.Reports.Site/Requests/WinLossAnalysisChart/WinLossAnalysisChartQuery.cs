namespace Alpha.Reports.Site.Requests.WinLossAnalysisChart
{
    using System;
    using App_GlobalResources;
    using Extensions;
    using MediatR;
    using Reports.Models;
    using Utils;
    using ViewModels.WinLossAnalysisChart;

    public class WinLossAnalysisChartQuery :
        FromLast18MonthDateRangeQuery,
        IRequest<WinLossAnalysisChartViewModel>
    {
        private const string WonInEnglish = "Won";

        public WinLossAnalysisChartQuery(IUser viewer) : base(viewer)
        {
            if (SystemTime.Today.IsEndOfMonth())
            {
                FromDate = SystemTime.Today.AddMonths(-2).FirstDayOfMonth();
                ToDate = SystemTime.Today;
            }
            else
            {
                FromDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
                ToDate = SystemTime.Today.AddMonths(-1).LastDayOfMonth();
            }

            CurrencyName = string.Equals(viewer.CurrencyName, WonInEnglish, StringComparison.OrdinalIgnoreCase)
                ? Resources.KoreanWon
                : viewer.CurrencyName;
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public string CurrencyName { get; set; }

        public string UserSelectedExcludeProductIds { get; set; }
    }
}