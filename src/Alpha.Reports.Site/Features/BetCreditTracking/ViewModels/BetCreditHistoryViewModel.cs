namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;
    using System.Collections.Generic;

    public class BetCreditHistoryViewModel
    {
        private readonly IList<string> optionValues = new List<string> { "0", "1", "2", "3", "7" };

        public BetCreditHistoryViewModel(BetCreditHistoryQuery message) : this(null, message)
        {
        }

        public BetCreditHistoryViewModel(IEnumerable<BetCreditHistoryModel> data, BetCreditHistoryQuery message, bool isError = false)
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
            ErrMsg = isError ? Resources.InvalidUsername : string.Empty;
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

        public string ErrMsg { get; }

        public int UserSelectedDateRangeOption { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
         => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption, optionValues: optionValues);
    }
}