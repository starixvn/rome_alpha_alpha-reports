namespace Alpha.Reports.Site.Requests.WinLossAnalysis
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossAnalysis;

    public class WinLossAnalysisQuery :
         FromLast18MonthDateRangeQuery,
         IRequest<WinLossAnalysisViewModel>
    {
        public WinLossAnalysisQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public int ReportTypeId { get; set; } = 1;

        public string ReportTypeName { get; set; }

        public string ReportTypeTitle { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}