namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using System.Collections.Generic;

    public class BetChoiceMatchingQueryResult
    {
        public BetChoiceMatchingQueryResult(
            IEnumerable<BetChoiceMatchingModel> items,
            IEnumerable<KeyValueModel> teams,
            IEnumerable<KeyValueModel> leagues,
            IEnumerable<KeyValueModel> sportTypes,
            IEnumerable<KeyValueModel> betTypes
            )
        {
            Items = items;
            Teams = teams;
            Leagues = leagues;
            SportTypes = sportTypes;
            BetTypes = betTypes;
        }

        public IEnumerable<BetChoiceMatchingModel> Items { get; }

        public IEnumerable<KeyValueModel> Teams { get; }

        public IEnumerable<KeyValueModel> Leagues { get; }

        public IEnumerable<KeyValueModel> SportTypes { get; }

        public IEnumerable<KeyValueModel> BetTypes { get; }
    }
}