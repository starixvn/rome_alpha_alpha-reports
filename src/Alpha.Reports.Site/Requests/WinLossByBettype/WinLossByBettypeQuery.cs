namespace Alpha.Reports.Site.Requests.WinLossByBetType
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossByBetType;

    public class WinLossByBetTypeQuery :
        FromBalanceUpDateDateRangeQuery,
        IRequest<WinLossByBetTypeViewModel>
    {
        public WinLossByBetTypeQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedDateRangeOption { get; set; }

        public bool IsStackMode { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public bool IsSetDefaultStackMode => false;
    }
}