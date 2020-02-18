namespace Alpha.Reports.Tests.TestCases.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.Helpers;
    using System.Collections.Generic;
    using System.Linq;

    public static class BetChoiceMatchingTestCases
    {
        public static List<BetChoiceMatchingTicketModel> Tickets { get; } = FileHelper.ReadJson<List<BetChoiceMatchingTicketModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingTickets.json");

        public static List<BetChoiceSharedIPModel> IPs { get; } = FileHelper.ReadJson<List<BetChoiceSharedIPModel>>("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingIPs.json");

        public static string EventsJson { get; } = FileHelper.ReadJson("\\TestCases\\BetChoiceMatching\\BetChoiceMatchingEvents.json");

        public static List<BetChoiceMatchingEventModel> Events { get; }

        static BetChoiceMatchingTestCases()
        {
            var events = new List<BetChoiceMatchingEventModel>();
            foreach (var ticket in Tickets)
            {
                var eventModel = new BetChoiceMatchingEventModel(ticket);
                var selectedEvent = events.FirstOrDefault(x => x.Key == eventModel.Key);
                if(selectedEvent != null)
                {
                    selectedEvent.AddTicket(ticket);
                }
                else
                {
                    events.Add(eventModel);
                }
            }
            Events = events;
        }
    }
}
