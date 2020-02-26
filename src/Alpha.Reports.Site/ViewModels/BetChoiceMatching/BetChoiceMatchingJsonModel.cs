namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using System.Collections.Generic;

    public class BetChoiceMatchingJsonModel
    {
        public BetChoiceMatchingJsonModel(
            IEnumerable<BetChoiceMatchingEventModel> events,
            IEnumerable<KeyValuePair<int, string>> teams,
            IEnumerable<KeyValuePair<int, string>> leagues,
            IEnumerable<KeyValuePair<int, string>> sportTypes,
            IEnumerable<KeyValuePair<int, string>> betTypes,
            IEnumerable<BetChoiceMatchingSharedIPModel> ips
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

        public IEnumerable<KeyValuePair<int, string>> Teams { get; }

        public IEnumerable<KeyValuePair<int, string>> Leagues { get; }

        public IEnumerable<KeyValuePair<int, string>> SportTypes { get; }

        public IEnumerable<KeyValuePair<int, string>> BetTypes { get; }

        public IEnumerable<BetChoiceMatchingSharedIPModel> IPs { get; set; }
    }
}