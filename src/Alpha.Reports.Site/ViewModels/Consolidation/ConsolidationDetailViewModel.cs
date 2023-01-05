namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            BreadcrumbActions = GetBreadcrumbActions(breadcrumbLevel);
        }

        public string DrilldownFromType => message.DrilldownFromType;

        public string SortPropertyName => message.SortPropertyName;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsStackMode => message.IsStackMode;

        public int UserSelectedCurrencyId => message.UserSelectedCurrencyId;

        public string UserSelectedProductIds => message.UserSelectedProductIds;

        public string UserSelectedResourceIds => message.UserSelectedResourceIds;

        public DateTime ConsolidationFromDate => message.ConsolidationFromDate;

        public DateTime ConsolidationToDate => message.ConsolidationToDate;

        public int ConsolidationUserSelectedDateRangeOption => message.ConsolidationUserSelectedDateRangeOption;

        public string ConsolidationProductIds => message.ConsolidationProductIds;

        public string ConsolidationResourceIds => message.ConsolidationResourceIds;

        public IEnumerable<string> BreadcrumbActions { get; }

        public ConsolidationFilterViewModel FilterViewModel { get; }

        public WinLossDetailViewModel WinLossDetailViewModel { get; }

        public DateTime MinDate => message.MinDate;

        public bool IsSupportHistoricReport => message.IsSupportHistoricReport;

        public bool IsHistoricReport => message.IsHistoricReport;

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossDetailModel model)
            => new Dictionary<string, object>();

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossDetailModel model)
            => new Dictionary<string, object>();

        private IEnumerable<string> GetBreadcrumbActions(IEnumerable<CustInfo> breadcrumbLevel)
        {
            List<string> actions = null;

            if (breadcrumbLevel != null)
            {
                actions = breadcrumbLevel.Select(b => "~/Consolidation/ConsolidationDetail").ToList();

                if (actions.Any())
                {
                    actions[0] = $"~/Consolidation/ConsolidationBy{message.DrilldownFromType}";
                }
            }

            return actions;
        }
    }
}