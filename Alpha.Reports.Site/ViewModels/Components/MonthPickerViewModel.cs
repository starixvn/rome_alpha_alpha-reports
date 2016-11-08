namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;
    using static Resources.SiteResources;    

    public class MonthPickerViewModel
    {
        public MonthPickerViewModel(DateTime minDate, DateTime fromDate, DateTime toDate)
        {
            MinDate = minDate;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime MinDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string[] MonthNames => new string[] 
        {
            January, February, March, April, May, June, July, August, September, October, November, December
        };
    }
}