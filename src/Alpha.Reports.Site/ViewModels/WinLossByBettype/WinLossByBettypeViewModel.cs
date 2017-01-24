namespace Alpha.Reports.Site.ViewModels.WinLossByBettype
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLossByBettype;

    public class WinLossByBettypeViewModel 
    {
        private readonly string sortPropertyNameDefault = "Bettype";

        private readonly WinLossByBettypeQuery message;
        private readonly WinLossByBetTypeParam winLossByBettypeParam;

        public WinLossByBettypeViewModel(
            IEnumerable<WinLossByBetTypeModel> reportData,
            WinLossByBetTypeModel grandTotal,
            WinLossByBettypeQuery message,
            WinLossByBetTypeParam winLossByBettypeParam,
            DateTime balanceUpDay)
        {
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            MinDate = balanceUpDay;
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildResources();
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate { get; }

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossByBettypeParam.FromDate;

        public DateTime ToDate => winLossByBettypeParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossByBetTypeModel> ReportData { get; }

        public WinLossByBetTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title { get; private set; }

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        public string CustLevelNetWinLossName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public WinLossByBettypeFilterViewModel FilterViewModel =>
            new WinLossByBettypeFilterViewModel(
                CustId,
                CustName,
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
                Title = Resources.MasterWinLoss;
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
                CustLevelNetWinLossName = "SuperNetWinLoss";
            }
            else if (CustLevel.IsMasterLevel())
            {
                Title = Resources.AgentWinLoss;
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
                CustLevelNetWinLossName = "MasterNetWinLoss";
            }
            else
            {
                Title = Resources.MemberWinLoss;
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
                CustLevelNetWinLossName = "AgentNetWinLoss";
            }
        }
    }
}