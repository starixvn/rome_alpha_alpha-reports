namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System;
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using ViewModels.WinLossDetail;

    public class WinLossDetailQuery<T> :
        IRequest<WinLossDetailViewModel<T>> where T : MemberWinLossDetail
    {
        public int CustId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public string SelectedProducts { get; set; }

        public string SortedColumn { get; set; }

        public int SortDirection { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public int SelectedDateRange { get; set; }
 public UserLevel UserLevel
        {
            get; set;
        }
    }
}