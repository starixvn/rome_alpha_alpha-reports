namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using ViewModels.WinLossDetail;

    public class MemberWinLossDetailQuery : ReportBaseQuery, IRequest<MemberWinLossDetailViewModel>
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ReportRange> ReportRanges { get; set; }

        public string Language { get; set; }

        public string SelectedProducts { get; set; }

        public string SortedColumn { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool HideFilter { get; set; }

        public string OldMainAppPath { get; set; }
    }
}