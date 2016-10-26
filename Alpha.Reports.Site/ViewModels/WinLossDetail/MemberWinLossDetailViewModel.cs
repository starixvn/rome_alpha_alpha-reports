namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public string MinDate { get; set; }

        public string MaxDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CurrentDate { get; set; }

        public string Products { get; set; }

        public bool HasData { get; set; }

        public string Language { get; set; }

        public IEnumerable<string> NetTurnoverToolTips { get; set; }
    }
}