namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;

    public class WinLossDetailViewModel
    {
        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime CurrentDate { get; set; }

        public string Language { get; set; }

        public IEnumerable<string> NetTurnoverTooltips { get; set; }

        public string Products { get; set; }

        public string SelectedProducts { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public string OldMainAppPath { get; set; }
    }
}