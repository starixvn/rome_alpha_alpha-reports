namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LeagueMultiSelectionViewModel
    {
        public LeagueMultiSelectionViewModel(
            IEnumerable<SelectListItem> leagues,
            string userSelectedLeagueIds,
            string language)
        {
            Leagues = leagues;
            UserSelectedLeagueIds = userSelectedLeagueIds;
            Language = language;
        }

        public IEnumerable<SelectListItem> Leagues { get; }

        public string Language { get; }

        public string UserSelectedLeagueIds { get; }
    }
}