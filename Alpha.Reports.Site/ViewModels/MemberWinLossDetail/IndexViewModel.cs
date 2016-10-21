namespace Alpha.Reports.Site.ViewModels.MemberWinLossDetail
{
    using System;

    public class IndexViewModel
    {
        public string MinDate { get; set; }

        public string MaxDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CurrentDate { get; set; }
    }
}