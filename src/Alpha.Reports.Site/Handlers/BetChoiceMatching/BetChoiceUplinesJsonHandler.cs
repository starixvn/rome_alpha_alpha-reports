namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using MediatR;

    public class BetChoiceUplinesJsonHandler :
        BaseHandler,
        IRequestHandler<BetChoiceUplinesJsonQuery, BetChoiceMatchingUplinesModel>
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;

        public BetChoiceUplinesJsonHandler(BetChoiceMatchingService betChoiceMatchingService)
        {
            this.betChoiceMatchingService = betChoiceMatchingService;
        }

        public BetChoiceMatchingUplinesModel Handle(BetChoiceUplinesJsonQuery message)
        {
            var queryParam = new BetChoiceUplinesParam(message.SearchAccountId);
            return betChoiceMatchingService.QueryUplines(queryParam);
        }
    }
}