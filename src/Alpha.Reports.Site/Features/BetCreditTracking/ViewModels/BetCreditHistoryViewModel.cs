namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Web.Helpers;
    using System;
    using System.Collections.Generic;

    public class BetCreditHistoryViewModel
    {
        private readonly IList<string> optionValues = new List<string> {
            DateRangePickerViewModel.TodayOptionValue,
            DateRangePickerViewModel.YesterdayOptionValue,
            DateRangePickerViewModel.ThisWeekOptionValue,
            DateRangePickerViewModel.LastWeekOptionValue,
            DateRangePickerViewModel.SpecificDateOptionValue
        };

        public BetCreditHistoryViewModel(BetCreditHistoryQuery message) : this(null, message)
        {
        }

        public BetCreditHistoryViewModel(IEnumerable<BetCreditHistoryModel> data, BetCreditHistoryQuery message, string errMsg = "", int totalRows = 0)
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
            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
            ErrMsg = errMsg;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, totalRows);
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

        public int PageIndex { get; }

        public int PageSize { get; }

        public string ErrMsg { get; }

        public int UserSelectedDateRangeOption { get; }

        public PagerViewModel PagerViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
         => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption, optionValues: optionValues);

        public string GetUserGuideLink(bool isCookieless, string host, string sessionId)
        {
            if (SiteSettings.SupportedNewHandbook)
            {
                var handbookPath = "Default/GetBySlug?slug=bet-credit-tracking";
                return isCookieless && SiteSettings.IsStandaloneReport
                    ? $"//{host}/site-howtouse/(S({sessionId}))/{handbookPath}"
                    : MicroSites.HowToUseUrl($"~/{handbookPath}");
            }

            if (isCookieless && SiteSettings.IsStandaloneReport)
            {
                return SiteSettings.IsSiteLabel789y
                    ? $"//{host}/site-messages/(S({sessionId}))/StaticMessage/Guide?id=167296"
                    : $"//{host}/site-howtouse/(S({sessionId}))/Index.aspx?linktomessagesite=167296";
            }

            return SiteSettings.IsSiteLabel789y
                ? MicroSites.MessageUrl("~/StaticMessage/Guide?id=167296")
                : MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=167296");
        }
    }
}