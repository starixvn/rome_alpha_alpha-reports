namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AppResources;

    public class DateRangePickerViewModel
    {
        public DateRangePickerViewModel(
            DateTime minDate,
            DateTime maxDate,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedDateRangeOption = userSelectedDateRangeOption;

            // Cannot use static here because Localization.
            RangeOptions = (new List<SelectListItem>
            {
                new SelectListItem { Text = Resources.Today, Value = "0", Selected = true },
                new SelectListItem { Text = Resources.Yesterday, Value = "1" },
                new SelectListItem { Text = Resources.ThisWeek, Value = "2" },
                new SelectListItem { Text = Resources.LastWeek, Value = "3" },
                new SelectListItem { Text = Resources.ThisMonth, Value = "4" },
                new SelectListItem { Text = Resources.LastMonth, Value = "5" },
                new SelectListItem { Text = Resources.SinceLastMonth, Value = "6" },
                new SelectListItem { Text = Resources.SpecificDate, Value = "7" }
            }).Select(item =>
            {
                item.Selected = item.Value == userSelectedDateRangeOption.ToString();
                return item;
            });
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int UserSelectedDateRangeOption { get; }

        public IEnumerable<SelectListItem> RangeOptions { get; }
    }
}