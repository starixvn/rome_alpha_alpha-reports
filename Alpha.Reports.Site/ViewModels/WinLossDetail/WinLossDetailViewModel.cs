namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.WinlossDetail;

    public class WinLossDetailViewModel<T> where T : MemberWinLossDetail
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

        public ReportData<T> ReportData { get; set; }
        
        public int CustId { get; set; }

        public string SortedColumn { get; set; }

        public int SortDirection { get; set; }
    }
}