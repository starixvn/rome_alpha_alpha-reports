namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System.Collections.Generic;

    public class BetChoiceMatchingUserModel
    {
        private readonly List<BetChoiceMatchingTicketModel> tickets;

        public BetChoiceMatchingUserModel(BetChoiceMatchingTicketModel ticket)
        {
            CustId = ticket.CustId;
            Username = ticket.Username;
            Stake = ticket.Stake;
            MemberWinLoss = ticket.MemberWinLoss;
            SuperWinLoss = ticket.SuperWinLoss;
            Company = ticket.Company;
            IsRunning = ticket.IsRunning;
            tickets = new List<BetChoiceMatchingTicketModel> { ticket };
        }

        public int CustId { get; }

        public string Username { get; }

        public decimal Stake { get; private set; }

        public decimal MemberWinLoss { get; private set; }

        public decimal SuperWinLoss { get; private set; }

        public decimal Company { get; private set; }

        public bool IsRunning { get; }

        public IEnumerable<BetChoiceMatchingTicketModel> Tickets => tickets;

        public void AddTicket(BetChoiceMatchingTicketModel ticket)
        {
            tickets.Add(ticket);
            Stake += ticket.Stake;
            MemberWinLoss += ticket.MemberWinLoss;
            SuperWinLoss += ticket.SuperWinLoss;
            Company += ticket.Company;
        }
    }
}
