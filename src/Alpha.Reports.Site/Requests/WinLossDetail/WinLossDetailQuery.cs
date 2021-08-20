namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System;
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLossDetail;
    using WinLoss;

    public class WinLossDetailQuery :
        WinLossQuery,
        IRequest<WinLossDetailViewModel>
    {
        public WinLossDetailQuery(IUser viewer) : base(viewer)
        {
            IsProtectedMode = viewer.IsProtectedMode;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsProtectedMode { get; }
    }
}