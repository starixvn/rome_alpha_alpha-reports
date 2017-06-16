namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Requests.Consolidation;
    using WinLossDetail;

    public class ConsolidationDetailViewModel : DrillDownableReportViewModel<WinLossDetailModel>
    {
        private readonly ConsolidationDetailQuery message;

        public ConsolidationDetailViewModel(
            ConsolidationDetailQuery message,
            IEnumerable<CustInfo> breadcrumbLevel,
            ConsolidationFilterViewModel filterViewModel,
            WinLossDetailViewModel winLossDetailViewModel) : base(message.CustLevel, breadcrumbLevel)
        {
            this.message = message;

            FilterViewModel = filterViewModel;
            WinLossDetailViewModel = winLossDetailViewModel;
        }

        public string DrilldownFromType => message.DrilldownFromType;

        public string SortPropertyName => message.SortPropertyName;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsStackMode => message.IsStackMode;

        public ConsolidationFilterViewModel FilterViewModel { get; }

        public WinLossDetailViewModel WinLossDetailViewModel { get; }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossDetailModel model)
            => new Dictionary<string, object>();

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossDetailModel model)
            => new Dictionary<string, object>();
    }
}