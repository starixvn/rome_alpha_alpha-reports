namespace Alpha.Reports.Site.Features.BonusDetail
{
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BonusDetail;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BonusDetail.Requests;
    using Alpha.Reports.Site.Features.BonusDetail.ViewModels;
    using MediatR;

    public class BonusDetailHandler :
        IAsyncRequestHandler<BonusDetailQuery, BonusDetailViewModel>,
        IAsyncRequestHandler<ConfirmPaymentCommand, Result>
    {
        private readonly BonusDetailService service;

        public BonusDetailHandler(BonusDetailService service)
        {
            this.service = service;
        }

        public async Task<BonusDetailViewModel> Handle(BonusDetailQuery message)
        {
            var report = await service.GetBonusDetailAsync(GetBonusDetailParam(message));

            return new BonusDetailViewModel(report, message);
        }

        private static BonusDetailParam GetBonusDetailParam(BonusDetailQuery message)
            => new BonusDetailParam(
                message.ViewerId,
                message.ViewerLevel,
                message.Member,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                message.StatusId,
                message.IsComplained,
                message.PageIndex,
                message.PageSize);

        public async Task<Result> Handle(ConfirmPaymentCommand message)
        {
            var result = await service.ConfirmPaymentAsync(GetConfirmPayment(message));

            if (result.IsOk)
            {
                result.Message = Resources.ConfirmPaymentSuccessfully;
            }

            return result;
        }

        private static ConfirmPaymentParam GetConfirmPayment(ConfirmPaymentCommand message)
            => new ConfirmPaymentParam(
                message.ViewerId,
                message.SubAccId,
                message.CustId,
                message.TransId,
                message.PaymentDate,
                message.IP,
                message.Language);
    }
}