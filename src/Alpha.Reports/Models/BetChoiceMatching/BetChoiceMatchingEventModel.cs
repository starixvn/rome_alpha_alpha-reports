namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetChoiceMatchingEventModel
    {
        private readonly List<BetChoiceMatchingUserModel> users;

        public BetChoiceMatchingEventModel(BetChoiceMatchingTicketModel ticket, IEnumerable<BetChoiceMatchingUserModel> users)
        {
            LeagueId = ticket.LeagueId;
            SportType = ticket.SportType;
            BetType = ticket.BetType;
            GlobalShowTime = ticket.GlobalShowTime;
            Hdp1 = ticket.Hdp1;
            Hdp2 = ticket.Hdp2;
            BetTeam = ticket.BetTeam;
            IsLive = ticket.IsLive;
            LiveHomeScore = ticket.LiveHomeScore;
            LiveAwayScore = ticket.LiveAwayScore;
            HomeId = ticket.HomeId;
            AwayId = ticket.AwayId;
            IsRunning = ticket.IsRunning;
            Key = $"{ticket.MatchId}_{BetType}_{Hdp1}_{Hdp2}_{BetTeam}_{IsLive}_{LiveHomeScore}_{LiveAwayScore}";
            this.users = users.ToList();
        }

        public BetChoiceMatchingEventModel(BetChoiceMatchingTicketModel ticket)
            : this(ticket, new List<BetChoiceMatchingUserModel> { new BetChoiceMatchingUserModel(ticket) })
        {
        }

        public string Key { get; }

        public int LeagueId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public decimal Hdp1 { get; }

        public decimal Hdp2 { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public string BetTeam { get; }

        public DateTime GlobalShowTime { get; }

        public bool IsLive { get; }

        public int LiveHomeScore { get; }

        public int LiveAwayScore { get; }

        public bool IsRunning { get; }

        public IEnumerable<BetChoiceMatchingUserModel> Users => users;

        public void AddTicket(BetChoiceMatchingTicketModel ticket)
        {
            var selectedUser = users.FirstOrDefault(g => g.Username == ticket.Username);
            if (selectedUser == null)
            {
                users.Add(new BetChoiceMatchingUserModel(ticket));
            }
            else
            {
                selectedUser.AddTicket(ticket);
            }
        }
    }
}
