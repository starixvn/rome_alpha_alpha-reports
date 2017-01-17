namespace Alpha.Reports.Site.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Components;

    public class LeaguesByCustomerViewModel
    {
        public LeaguesByCustomerViewModel(string language, IEnumerable<SelectListItem> leagues)
        {
            Language = language;
            Leagues = leagues;
        }

        public string Language { get; }

        public IEnumerable<SelectListItem> Leagues { get; }

        public string LeagueIds => string.Join(",", Leagues.Select(league => league.Value));

        public LeagueMultiSelectionViewModel LeagueMultiSelectionViewModel =>
            new LeagueMultiSelectionViewModel(Leagues, LeagueIds, Language);
    }
}