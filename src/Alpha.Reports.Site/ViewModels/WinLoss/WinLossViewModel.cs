namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;

    public class WinLossViewModel
    {
        public WinLossViewModel(List<WinLossModel> reportData)
        {
            if (reportData != null && reportData.Any())
            {
                ReportData = reportData.OrderBy(x => x.CustName).ToList();
                int index, count;
                for (index = 1, count = reportData.Count, GrantTotal = reportData[0]; index < count; index++)
                {
                    GrantTotal = WinLossModel.Add(GrantTotal, reportData[index]);
                }
            }
            else
            {
                ReportData = Enumerable.Empty<WinLossModel>();
                GrantTotal = new WinLossModel();
            }
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public string UserSelectedProductIds { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public IEnumerable<WinLossModel> ReportData { get; set; }

        public WinLossModel GrantTotal { get; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }

        private MonthPickerViewModel MonthPickerViewModel =>
            new MonthPickerViewModel(MinDate, FromDate, ToDate);

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(Products, UserSelectedProductIds, Language);

        public WinLossFilterViewModel FilterViewModel =>
            new WinLossFilterViewModel(
                CustId,
                CustName,
                UserLevel,
                IsHistoricReport,
                DateRangePickerViewModel,
                MonthPickerViewModel,
                ProductMultiSelectionViewModel,
                IsFilterVisible,
                MinDate,
                ((WinLossQuery)BreadcrumbLevels.Last().RouteValues).BreadcrumbLevels);

        public WinLossActionLinkViewModel ActionLinkViewModel(WinLossModel winLoss)
        {
            if (winLoss == null)
            {
                throw new ArgumentNullException("winLoss", "winLoss cannot be a null reference");
            }

            return new WinLossActionLinkViewModel(
                    winLoss.CustId,
                    winLoss.CustName,
                    (byte)(UserLevel.Value - 1),
                    FromDate,
                    ToDate,
                    UserSelectedDateRangeOption,
                    UserSelectedProductIds,
                    IsHistoricReport,
                    IsFilterVisible,
                    IsStackMode,
                    BreadcrumbLevels);
        }

        public UserLevel UserLevel { get; set; }

        public string Title
        {
            get
            {
                if (UserLevel.IsSuperLevel())
                {
                    return Resources.MasterWinLoss;
                }

                if (UserLevel.IsMasterLevel())
                {
                    return Resources.AgentWinLoss;
                }

                return Resources.MemberWinLoss;
            }
        }

        public IEnumerable<ActionLinkViewModel> BreadcrumbLevels { get; set; }
    }
}