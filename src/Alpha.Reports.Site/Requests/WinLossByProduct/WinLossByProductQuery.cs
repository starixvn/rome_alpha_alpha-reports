namespace Alpha.Reports.Site.Requests.WinLossByProduct
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossByProduct;

    public class WinLossByProductQuery :
        FromBalanceUpDateDateRangeQuery,
        IRequest<WinLossByProductViewModel>
    {
        public WinLossByProductQuery(IUser viewer) : base(viewer)
        {
        }

        public string UserSelectedProductIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
    }
}