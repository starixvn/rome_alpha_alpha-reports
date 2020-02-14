namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using System.Collections.Generic;

    public class BetChoiceMatchingJsonModel
    {
        public BetChoiceMatchingJsonModel(
            IEnumerable<BetChoiceMatchingEventModel> events,
            IEnumerable<KeyValueModel> teams,
            IEnumerable<KeyValueModel> leagues,
            IEnumerable<KeyValueModel> sportTypes,
            IEnumerable<KeyValueModel> betTypes,
            IEnumerable<BetChoiceSharedIPModel> ips
            )
        {
            Events = events;
            Teams = teams;
            Leagues = leagues;
            SportTypes = sportTypes;
            BetTypes = betTypes;
            IPs = ips;
        }

        public IEnumerable<BetChoiceMatchingEventModel> Events { get; }

        public IEnumerable<KeyValueModel> Teams { get; }

        public IEnumerable<KeyValueModel> Leagues { get; }

        public IEnumerable<KeyValueModel> SportTypes { get; }

        public IEnumerable<KeyValueModel> BetTypes { get; }

        public IEnumerable<BetChoiceSharedIPModel> IPs { get; set; }
    }
}