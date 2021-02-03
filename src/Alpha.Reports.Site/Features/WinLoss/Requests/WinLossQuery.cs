namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLoss;

    public class WinLossQuery :
         ReportDefaultDateRangeQuery,
         IRequest<WinLossViewModel>
    {
        public WinLossQuery(IUser viewer) : base(viewer)
        {
            this.IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
        }

        public string UserSelectedProductIds { get; set; }

        public string UserSelectedResourceIds { get; set; }

        public bool IsStackMode { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

        public bool IsEnableSabaPromotion { get; }
    }
}