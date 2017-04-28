namespace Alpha.Reports.Site.Requests.WinLossAnalysisChart
{
    using Constants;
    using Extensions;
    using MediatR;
    using Reports.Models;
    using System;
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
            CurrencyName = string.Equals(viewer.Language, LanguageCode.Korean, StringComparison.OrdinalIgnoreCase)
                && string.Equals(viewer.CurrencyName, "Won", StringComparison.OrdinalIgnoreCase)
                ? "원"
                : viewer.CurrencyName;
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public string CurrencyName { get; set; }
    }
}