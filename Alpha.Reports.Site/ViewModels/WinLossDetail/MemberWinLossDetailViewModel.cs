namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.WinlossDetail;

    public class IndexViewModel
    {
        public string MinDate { get; set; }

        public string MaxDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CurrentDate { get; set; }

        public string Language { get; set; }

        public IEnumerable<string> NetTurnoverTooltips { get; set; }

        public ReportData<MemberWinlossDetail> ReportData { get; set; }

        public string Products { get; set; }

        public string SelectedProducts { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public string OldMainAppPath { get; set; }
    }
}