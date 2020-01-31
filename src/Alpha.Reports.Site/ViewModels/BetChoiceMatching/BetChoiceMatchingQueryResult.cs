namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using System.Collections.Generic;

    public class BetChoiceMatchingQueryResult
    {
        public BetChoiceMatchingQueryResult(
            BetChoiceMatchingViewResource resource,
            IEnumerable<BetChoiceMatchingModel> items,
            IEnumerable<KeyValueModel> leagues,
            IEnumerable<KeyValueModel> sportTypes,
            IEnumerable<KeyValueModel> betTypes
            )
        {
            Resource = resource;
            Items = items;
            Leagues = leagues;
            SportTypes = sportTypes;
            BetTypes = betTypes;
        }

        public BetChoiceMatchingViewResource Resource { get; }

        public IEnumerable<BetChoiceMatchingModel> Items { get; }

        public IEnumerable<KeyValueModel> Leagues { get; }

        public IEnumerable<KeyValueModel> SportTypes { get; }

        public IEnumerable<KeyValueModel> BetTypes { get; }
    }
}