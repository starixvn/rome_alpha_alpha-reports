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

        public int LeagueId { get; private set; }

        public DateTime GlobalShowTime { get; private set; }

        public int SportType { get; private set; }

        public long MatchId { get; private set; }

        public int CustId { get; private set; }

        public string Username { get; private set; }

        public int BetType { get; private set; }

        public string BetTeam { get; private set; }

        public decimal Hdp1 { get; private set; }

        public decimal Hdp2 { get; private set; }

        public string IP { get; private set; }

        public decimal Odds { get; set; }

        public string DisplayOdds { get; set; } = string.Empty;

        public byte OddsType { get; private set; }

        public string DisplayOddsType { get; set; } = string.Empty;

        public DateTime TransDate { get; private set; }

        public long TransId { get; private set; }

        public string TransIdString => TransId.ToString();

        public decimal Stake { get; private set; }

        public decimal MemberWinLoss { get; private set; }

        public decimal SuperWinLoss { get; private set; }

        public decimal Company { get; private set; }

        public int HomeId { get; private set; }

        public int AwayId { get; private set; }

        public bool IsLive { get; private set; }

        public int LiveHomeScore { get; private set; }

        public int LiveAwayScore { get; private set; }

        public bool IsRunning { get; private set; }
    }
}
