namespace Alpha.Reports.Site.Features.BonusDetail.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Features.BonusDetail.Constants;
    using Alpha.Reports.Features.BonusDetail.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BonusDetail.Requests;
    using Alpha.Reports.Site.ViewModels.Components;

    public class BonusDetailViewModel
    {
        public BonusDetailViewModel(PagedReportData<GroupBonus> reportData, BonusDetailQuery message)
        {
            ReportData = reportData;
            Member = message.Member;
            MinDate = message.MinDate;
            MaxDate = DateTime.MaxValue;
            FromDate = message.FromDate;
            ToDate = message.ToDate;
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
            StatusId = message.StatusId;
            IsComplained = message.IsComplained;
            IsFilterVisible = message.IsFilterVisible;
            ViewerLevel = message.ViewerLevel;
        }

        public PagedReportData<GroupBonus> ReportData { get; }

        public string Member { get; set; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int UserSelectedDateRangeOption { get; }

        public byte? StatusId { get; }

        public bool? IsComplained { get; }

        public bool IsFilterVisible { get; }

        public UserLevel ViewerLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
            => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public PagerViewModel PagerViewModel
            => new PagerViewModel(ReportData.CurrentPageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public IEnumerable<SelectListItem> BonusStatus
              => new[]
              {
                GetSelectListItem(Resources.All, "-1", !StatusId.HasValue),
                GetSelectListItem(Resources.Pending, BonusStatusIdConst.Pending.ToString(), StatusId == BonusStatusIdConst.Pending),
                GetSelectListItem(Resources.Cancelled, BonusStatusIdConst.Cancelled.ToString(), StatusId == BonusStatusIdConst.Cancelled),
                GetSelectListItem(Resources.Completed, BonusStatusIdConst.Completed.ToString(), StatusId == BonusStatusIdConst.Completed)
              };

        public IEnumerable<SelectListItem> MemberFeedback
            => new[]
            {
                GetSelectListItem(Resources.All, "-1", !IsComplained.HasValue),
                GetSelectListItem(Resources.NotReceived, true.ToString(), IsComplained == true),
                GetSelectListItem(Resources.Received, false.ToString(), IsComplained == false)
            };

        private static SelectListItem GetSelectListItem(string text, string value, bool selected)
            => new SelectListItem { Text = text, Value = value, Selected = selected };

        public string GuidelineURL() =>Helpers.UrlHelper.MapMessagePath("~/staticmessage/guide?id=147430");
    }
}