namespace Alpha.Reports.Site.ViewModels.WinLossByGameType
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByGameType;
    using Requests.WinLossByGameType;

    public class WinLossByGameTypeViewModel
    {
        private readonly string sortPropertyNameDefault = "GameTypeName";

        private readonly WinLossByGameTypeQuery message;
        private readonly WinLossByGameTypeParam winLossByGameTypeParam;

        public WinLossByGameTypeViewModel(
            IEnumerable<WinLossByGameTypeModel> reportData,
            WinLossByGameTypeModel grandTotal,
            WinLossByGameTypeQuery message,
            WinLossByGameTypeParam winLossByGameTypeParam)
        { 
            this.message = message;
            this.winLossByGameTypeParam = winLossByGameTypeParam;
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildResources();
        }

        public int CustId => message.CustId;

        public int ProductId => message.ProductId;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossByGameTypeParam.FromDate;

        public DateTime ToDate => winLossByGameTypeParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossByGameTypeModel> ReportData { get; }

        public WinLossByGameTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => message.Title;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public WinLossByGameTypeFilterViewModel FilterViewModel =>
            new WinLossByGameTypeFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
                IsFilterVisible,
                MinDate);

        private string GetSortedProperty(UserLevel custLevel, string sortedProperty)
        {
            if (string.IsNullOrEmpty(sortedProperty))
            {
                return sortPropertyNameDefault;
            }

            var isSortedPropertyInRange =
                custLevel.IsSuperLevel()
                || (custLevel.IsMasterLevel() && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase))
                || (custLevel.IsAgentLevel()
                        && !sortedProperty.StartsWith("Super", StringComparison.OrdinalIgnoreCase)
                        && !sortedProperty.StartsWith("Master", StringComparison.OrdinalIgnoreCase));

            return isSortedPropertyInRange ? sortedProperty : sortPropertyNameDefault;
        }

        ///// <summary>
        ///// TODO refactor Title
        ///// </summary>
        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
                CustLevelNetWinLossName = "SuperNetWinLoss";
            }
            else if (CustLevel.IsMasterLevel())
            {
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
                CustLevelNetWinLossName = "MasterNetWinLoss";
            }
            else
            {
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
                CustLevelNetWinLossName = "AgentNetWinLoss";
            }
        }
    }
}