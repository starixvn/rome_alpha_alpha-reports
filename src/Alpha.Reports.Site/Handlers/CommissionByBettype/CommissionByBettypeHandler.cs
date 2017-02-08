namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using MediatR;
    using Requests.CommissionByBettype;
    using System;
    using ViewModels.CommissionByBettype;

    public class CommissionByBettypeHandler : BaseHandler, IRequestHandler<CommissionByBettypeQuery, CommissionByBettypeViewModel>
    {
        public CommissionByBettypeViewModel Handle(CommissionByBettypeQuery message)
        {
            throw new NotImplementedException();
        }
    }
}