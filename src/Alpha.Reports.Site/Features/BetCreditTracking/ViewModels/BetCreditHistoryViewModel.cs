namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;
    using System.Collections.Generic;

    public class BetCreditHistoryViewModel
    {
        public BetCreditHistoryViewModel(BetCreditHistoryQuery message) : this(null, message, false)
        {
        }

        public BetCreditHistoryViewModel(IEnumerable<BetCreditHistoryModel> data, BetCreditHistoryQuery message, bool isSearch = true, bool isUsernameValid = true)
        {
            Data = data;
            CustLevel = message.ViewerLevel.DisplayName;
            Member = message.Member;
            RefNo = message.RefNo;
            IsFilterVisible = message.IsFilterVisible;
            MinDate = message.MinDate;
            MaxDate = DateTime.MaxValue;
            FromDate = message.FromDate;
            ToDate = message.ToDate;
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
            IsSearch = isSearch;
            IsUsernameValid = isUsernameValid;
        }

        public IEnumerable<BetCreditHistoryModel> Data { get; }

        public string CustLevel { get; }

        public string Member { get; }

        public long? RefNo { get; }

        public bool IsFilterVisible { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsSearch { get; }

        public bool IsUsernameValid { get; }

        public int UserSelectedDateRangeOption { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
         => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);
    }
}