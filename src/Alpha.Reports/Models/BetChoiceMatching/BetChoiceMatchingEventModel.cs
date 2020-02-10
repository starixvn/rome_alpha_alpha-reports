namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BetChoiceMatchingEventModel
    {
        public BetChoiceMatchingEventModel(BetChoiceMatchingModel model)
        {
            LeagueId = model.LeagueId;
            SportType = model.SportType;
            BetType = model.BetType;
            GlobalShowTime = model.GlobalShowTime;
            Hdp1 = model.Hdp1;
            Hdp2 = model.Hdp2;
            BetTeam = model.BetTeam;
            LiveHomeScore = model.LiveHomeScore;
            LiveAwayScore = model.LiveAwayScore;
            HomeId = model.HomeId;
            AwayId = model.AwayId;
            IsRunning = model.IsRunning;
            Key = $"{model.MatchId}_{BetType}_{Hdp1}_{Hdp2}_{BetTeam}";
            Users = new List<BetChoiceMatchingUserModel> { new BetChoiceMatchingUserModel(model) };
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

        public int LiveHomeScore { get; }

        public int LiveAwayScore { get; }

        public bool IsRunning { get; }

        public List<BetChoiceMatchingUserModel> Users { get; }

        public void Add(BetChoiceMatchingModel item)
        {
            var selectedUser = Users.FirstOrDefault(g => g.Username == item.Username);
            if (selectedUser == null)
            {
                Users.Add(new BetChoiceMatchingUserModel(item));
            }
            else
            {
                selectedUser.Add(item);
            }
        }
    }
}
