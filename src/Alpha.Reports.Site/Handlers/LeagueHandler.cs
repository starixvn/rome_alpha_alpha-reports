namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Linq;
    using MediatR;
    using Params;
    using Requests;
    using Services;
    using ViewModels;

    public class LeagueHandler :
        BaseHandler,
        IRequestHandler<LeaguesByCustomerQuery, LeaguesByCustomerViewModel>
    {
        private readonly LeagueService leagueService;

        public LeagueHandler(LeagueService leagueService)
        {
            this.leagueService = leagueService;
        }

        public LeaguesByCustomerViewModel Handle(LeaguesByCustomerQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var leagues = leagueService.GetLeaguesByCustomer(new GetLeaguesByCustomerParam(
                message.CustId,
                message.Products,
                message.FromDate,
                message.ToDate,
                message.LanguageKey));

            return new LeaguesByCustomerViewModel(
                message.Language,
                BuildLeagueSelectListItems(leagues.Select(league => league.Id), leagues));
        }
    }
}