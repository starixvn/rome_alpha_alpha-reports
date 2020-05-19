namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Components;
    using App_GlobalResources;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;

    public class WinLossViewModel : WinLossReportBaseViewModel<WinLossModel>
    {
        public static readonly string NetWinLossWinLoss = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.WinLoss)}";
        public static readonly string NetWinLossComm = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Comm)}";
        public static readonly string NetWinLossTotal = $"{nameof(WinLossModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Total)}";

        public WinLossViewModel(
            IEnumerable<WinLossModel> reportData,
            WinLossModel grandTotal,
            WinLossQuery message,
            WinLossParam param,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<GroupProductViewModel> groupProducts) : base(message, param, breadcrumbLevel, groupProducts)
        {
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildResources();
            BetListPath = Helpers.UrlHelper.MapBetListPath("~/WinLoss/Index");

            var canSort = reportData != null && reportData.Count() > 1;
            SortPropertyName = canSort ? GetSortedProperty(message.CustLevel, message.SortPropertyName) : DefaultVariables.SortPropertyName;
            IsDescendingSort = canSort && message.IsDescendingSort;
        }

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrandTotal { get; }

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string SortPropertyName { get; }

        public bool IsDescendingSort { get; }

        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                Title = Resources.MasterWinLoss;
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
            }
            else if (CustLevel.IsMasterLevel())
            {
                Title = Resources.AgentWinLoss;
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
            }
            else
            {
                Title = Resources.MemberWinLoss;
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
            }
        }

        protected override IDictionary<string, object> GetDownlineLinkRouteValues(WinLossModel model)
            => new Dictionary<string, object>(base.GetDownlineLinkRouteValues(model))
            {
                ["IsDescendingSort"] = IsDescendingSort,
                ["SortPropertyName"] = SortPropertyName,
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName
            };

        protected override IDictionary<string, object> GetBetListLinkRouteValues(WinLossModel model)
            => new Dictionary<string, object>(base.GetBetListLinkRouteValues(model))
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.CustName
            };
    }
}