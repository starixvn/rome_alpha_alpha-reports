namespace Alpha.Reports.Tests.TestCases.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.Helpers;
    using System.Collections.Generic;

    public static class BetChoiceMatchingTestCases
    {
        public static List<BetChoiceMatchingTicketModel> Tickets { get; } = FileHelper.ReadJson<List<BetChoiceMatchingTicketModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingTickets.json");

        public static List<BetChoiceSharedIPModel> IPs { get; } = FileHelper.ReadJson<List<BetChoiceSharedIPModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingIPs.json");

        public static string EventsJson { get; } = FileHelper.ReadJson("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingEvents.json");
    }
}
