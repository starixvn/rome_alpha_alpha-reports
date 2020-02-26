namespace Alpha.Reports.Tests.TestCases.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.Helpers;
    using System.Collections.Generic;
    using System.Linq;

    public static class BetChoiceMatchingTestCases
    {
        public static List<BetChoiceMatchingTicketModel> Tickets { get; } = FileHelper.ReadJson<List<BetChoiceMatchingTicketModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingTickets.json");

        public static List<BetChoiceMatchingSharedIPModel> IPs { get; } = FileHelper.ReadJson<List<BetChoiceMatchingSharedIPModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingIPs.json");

        public static List<BetChoiceMatchingEventModel> Events { get; } = FileHelper.ReadJson<List<BetChoiceMatchingEventModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingEvents.json");
    }
}
