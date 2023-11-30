namespace Alpha.Reports.Models.BetChoiceMatching
{
    using Newtonsoft.Json;
    using System;

    public class BetChoiceMatchingTicketModel
    {
        public BetChoiceMatchingTicketModel()
        {
        }

        public BetChoiceMatchingTicketModel(
            int leagueId,
            int leagueGroupId,
            DateTime globalShowTime,
            int sportType,
            long matchId,
            int custId,
            string username,
            int betType,
            string betTeam,
            decimal hdp1,
            decimal hdp2,
            string ip,
            decimal odds,
            byte oddsType,
            DateTime transDate,
            long transId,
            decimal stake,
            decimal memberWinLoss,
            decimal superWinLoss,
            decimal company,
            int homeId,
            int awayId,
            bool isLive,
            int liveHomeScore,
            int liveAwayScore,
            bool isRunning)
        {
            LeagueId = leagueId;
            LeagueGroupId = leagueGroupId;
            GlobalShowTime = globalShowTime;
            SportType = sportType;
            MatchId = matchId;
            CustId = custId;
            Username = username;
            BetType = betType;
            BetTeam = betTeam;
            Hdp1 = hdp1;
            Hdp2 = hdp2;
            IP = ip;
            Odds = odds;
            OddsType = oddsType;
            TransDate = transDate;
            TransId = transId;
            HomeId = homeId;
            AwayId = awayId;
            IsLive = isLive;
            LiveHomeScore = liveHomeScore;
            LiveAwayScore = liveAwayScore;
            Stake = stake;
            MemberWinLoss = memberWinLoss;
            SuperWinLoss = superWinLoss;
            Company = company;
            IsRunning = isRunning;
        }

        [JsonIgnore]
        public string EventKey
        {
            get
            {
                return $"{MatchId}_{BetType}_{Hdp1}_{Hdp2}_{BetTeam}_{IsLive}_{LiveHomeScore}_{LiveAwayScore}";
            }
        }

        public void HideIP()
        {
            IP = "--";
        }

        public int LeagueId { get; set; }

        public int LeagueGroupId { get; private set; }

        public DateTime GlobalShowTime { get; set; }

        public int SportType { get; set; }

        public long MatchId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public int BetType { get; set; }

        public string BetTeam { get; set; }

        public decimal Hdp1 { get; set; }

        public decimal Hdp2 { get; set; }

        public string IP { get; set; }

        public decimal Odds { get; set; }

        public string DisplayOdds { get; set; } = string.Empty;

        public byte OddsType { get; set; }

        public string DisplayOddsType { get; set; } = string.Empty;

        public DateTime TransDate { get; set; }

        public long TransId { get; set; }

        public string TransIdString => TransId.ToString();

        public decimal Stake { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal SuperWinLoss { get; set; }

        public decimal Company { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsLive { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }

        public bool IsRunning { get; set; }
    }
}
