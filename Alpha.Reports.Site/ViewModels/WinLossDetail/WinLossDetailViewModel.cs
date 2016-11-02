namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Models;
    using Models.WinlossDetail;
    using Newtonsoft.Json;

    public class WinLossDetailViewModel<T> where T : MemberWinLossDetail
    {
        public DateTime MinDate => IsHistoricReport
            ? DateTime.Now.AddYears(-3).FirstDayOfMonth()
            : DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime CurrentDate { get; set; }

        public string Language { get; set; }

        public IEnumerable<string> NetTurnoverTooltips { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public string JsonProducts => JsonConvert.SerializeObject(Products);

        public string SelectedProducts { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public string OldMainAppPath { get; set; }

        public ReportData<T> ReportData { get; set; }

        public int CustId { get; set; }

        public string SortedColumn { get; set; }

        public int SortDirection { get; set; }

        public int SelectedDateRange { get; set; }
    }
}