namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using ViewModels.WinLossDetail;

    public class MasterWinLossDetailQuery : 
        WinLossDetailQuery, 
        IRequest<WinLossDetailViewModel<MasterWinLossDetail>>
    {
        public override UserLevel UserLevel => UserLevel.Super;
    }
}