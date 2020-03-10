namespace Alpha.Reports.Tests.TestCases.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.Helpers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections.Generic;

    public static class BetChoiceMatchingTestCases
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            FloatParseHandling = FloatParseHandling.Decimal,
            ContractResolver = new PrivateSetterContractResolver()
        };

        public static List<BetChoiceMatchingTicketModel> Tickets =>
            FileHelper.ReadJson<List<BetChoiceMatchingTicketModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingTickets.json", settings);

        public static List<BetChoiceMatchingSharedIPModel> IPs =>
            FileHelper.ReadJson<List<BetChoiceMatchingSharedIPModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingIPs.json", settings);

        public static List<BetChoiceMatchingEventModel> Events =>
            FileHelper.ReadJson<List<BetChoiceMatchingEventModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingEvents.json", settings);
    }
}
