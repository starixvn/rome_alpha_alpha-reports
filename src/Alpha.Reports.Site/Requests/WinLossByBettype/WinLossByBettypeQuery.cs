namespace Alpha.Reports.Site.Requests.WinLossByBettype
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossByBettype;

    public class WinLossByBettypeQuery :
        ReportDefaultDateRangeQuery,
        IRequest<WinLossByBettypeViewModel>
    {
        public WinLossByBettypeQuery(IUser viewer) : base(viewer)
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