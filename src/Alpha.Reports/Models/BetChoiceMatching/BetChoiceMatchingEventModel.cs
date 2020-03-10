namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetChoiceMatchingEventModel
    {
        public BetChoiceMatchingEventModel(IEnumerable<BetChoiceMatchingUserModel> users)
        {
            if (users == null || !users.Any())
            {
                throw new ArgumentNullException(nameof(users));
            }

            var ticket = users.First().Tickets.First();
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
            Users = users;
        }

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

        public IEnumerable<BetChoiceMatchingUserModel> Users { get; }
    }
}
