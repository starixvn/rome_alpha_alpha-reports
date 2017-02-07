namespace Alpha.Reports.Site.Requests.WinLossByGameType
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossByGameType;

    public class WinLossByGameTypeQuery :
         ReportDefaultDateRangeQuery,
        IRequest<WinLossByGameTypeViewModel>
    {
        public WinLossByGameTypeQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedDateRangeOption { get; set; }

        public string Title { get; set; }

        public int ProductId { get; set; }

        public bool IsStackMode { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public bool IsSetDefaultStackMode => false;
    }
}