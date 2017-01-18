namespace Alpha.Reports.Site.ViewModels.MatchWinLoss
{
    using Reports.Models.MatchWinLoss;

    public class MatchWinLossViewModel
    {
        public MatchWinLossViewModel(
            string eventInfo,
            string betTypeName,
            MatchWinLossModel matchWinLossModel)
        {
            EventInfo = eventInfo;
            BetTypeName = betTypeName;
            MatchId = matchWinLossModel.MatchId;
            BetTypeId = matchWinLossModel.BetType;
            LiveIndicator = matchWinLossModel.LiveIndicator;
            GrossComm = matchWinLossModel.GrossComm;
            Commission = matchWinLossModel.Commission;
            PositionTaking = matchWinLossModel.PositionTaking;
        }

        public MatchWinLossViewModel(
            string eventInfo,
            string betTypeName,
            int matchId,
            int betTypeId,
            bool liveIndicator,
            decimal grossComm,
            decimal commission,
            decimal positionTaking)
        {
            EventInfo = eventInfo;
            BetTypeName = betTypeName;
            MatchId = matchId;
            BetTypeId = betTypeId;
            LiveIndicator = liveIndicator;
            GrossComm = grossComm;
            Commission = commission;
            PositionTaking = positionTaking;
        }

        public int MatchId { get; }

        public string EventInfo { get; }

        public int BetTypeId { get; }

        public string BetTypeName { get; }

        public bool LiveIndicator { get; }

        public decimal GrossComm { get; }

        public decimal Commission { get; }

        public decimal PositionTaking { get; }

        public decimal Total => Commission + PositionTaking;

        public static MatchWinLossViewModel Add(MatchWinLossViewModel winlossA, MatchWinLossViewModel winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new MatchWinLossViewModel(
                eventInfo: string.Empty,
                betTypeName: string.Empty,
                matchId: 0,
                betTypeId: 0,
                liveIndicator: false,
                grossComm: winlossA.GrossComm + winlossB.GrossComm,
                commission: winlossA.Commission + winlossB.Commission,
                positionTaking: winlossA.PositionTaking + winlossB.PositionTaking);
        }
    }
}