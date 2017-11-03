namespace Alpha.Reports.Models.MatchWinLoss
{
    public class MatchWinLossModel
    {
        public MatchWinLossModel()
        {

        }

        public MatchWinLossModel(
            int matchId,
            int homeId,
            int awayId,
            int betType,
            bool liveIndicator,
            bool isNeutral,
            string matchCode,
            string showTime,
            decimal grossComm,
            decimal comm,
            decimal positionTaking)
        {
            MatchId = matchId;
            HomeId = homeId;
            AwayId = awayId;
            BetType = betType;
            LiveIndicator = liveIndicator;
            IsNeutral = isNeutral;
            MatchCode = matchCode;
            ShowTime = showTime;
            GrossComm = grossComm;
            Commission = comm;
            PositionTaking = positionTaking;
        }

        public int MatchId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public int BetType { get; }

        public bool LiveIndicator { get; }

        public bool IsNeutral { get; }

        public string MatchCode { get; }

        public string ShowTime { get; }

        public decimal GrossComm { get; }

        public decimal Commission { get; }

        public decimal PositionTaking { get; }

        public decimal Total => Commission + PositionTaking;
    }
}
