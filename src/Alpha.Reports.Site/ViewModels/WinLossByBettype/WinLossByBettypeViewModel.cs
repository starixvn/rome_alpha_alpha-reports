namespace Alpha.Reports.Site.ViewModels.WinLossByBettype
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByBettype;
    using Requests.WinLossByBettype;

    public class WinLossByBettypeViewModel 
    {
        private readonly string sortPropertyNameDefault = "Bettype";

        private readonly WinLossByBettypeQuery message;
        private readonly WinLossByBettypeParam winLossByBettypeParam;

        public WinLossByBettypeViewModel(
            IEnumerable<WinlossByBettypeModel> reportData,
            WinlossByBettypeModel grandTotal,
            WinLossByBettypeQuery message,
            WinLossByBettypeParam winLossByBettypeParam)
        { 
            this.message = message;
            this.winLossByBettypeParam = winLossByBettypeParam;
            ReportData = reportData;
            GrandTotal = grandTotal;
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossByBettypeParam.FromDate;

        public DateTime ToDate => winLossByBettypeParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinlossByBettypeModel> ReportData { get; }

        public WinlossByBettypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public string SortPropertyName => GetSortedProperty(message.CustLevel, message.SortPropertyName);

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title { get; }

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
    }
}