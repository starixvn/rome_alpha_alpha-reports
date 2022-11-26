namespace Alpha.Reports.Site.Requests.WinLossDetail
{
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
            SRecommendId = viewer.SRecommendId;
            SubAccId = viewer.SubAccId;
            OnUserId = viewer.OnUserId;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SSGUID { get; set; }

        public bool IsProtectedMode { get; }

        public int SRecommendId { get; }

        public int SubAccId { get; }

        public int OnUserId { get; }
    }
}