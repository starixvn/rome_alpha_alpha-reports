namespace Alpha.Reports.Models.BetChoiceMatching
{
    using Alpha.Reports.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetChoiceMatchingUserModel
    {
        public BetChoiceMatchingUserModel(IEnumerable<BetChoiceMatchingTicketModel> tickets)
        {
            if(tickets == null || !tickets.Any())
            {
                throw new ArgumentNullException(nameof(tickets));
            }

            Tickets = tickets;
            var firstTicket = tickets.First();
            CustId = firstTicket.CustId;
            Username = firstTicket.Username;
            IsRunning = firstTicket.IsRunning;
            foreach (var ticket in tickets)
            {
                Stake += ticket.Stake;
                if (!IsRunning)
                {
                    MemberWinLoss += ticket.MemberWinLoss;
                    SuperWinLoss += ticket.SuperWinLoss;
                    Company += ticket.Company;
                }
            }

            MemberWinLoss = MemberWinLoss.RoundDown();
        }

        public int CustId { get; }

        public string Username { get; }

        public decimal Stake { get; private set; }

        public decimal MemberWinLoss { get; private set; }

        public decimal SuperWinLoss { get; private set; }

        public decimal Company { get; private set; }

        public bool IsRunning { get; }

        public IEnumerable<BetChoiceMatchingTicketModel> Tickets { get; }
    }
}
