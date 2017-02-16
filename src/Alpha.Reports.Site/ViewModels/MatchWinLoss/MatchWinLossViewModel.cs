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

        public decimal GrossComm { get; private set; }

        public decimal Commission { get; private set; }

        public decimal PositionTaking { get; private set; }

        public decimal Total => Commission + PositionTaking;

        public MatchWinLossViewModel Add(MatchWinLossViewModel model)
        {
            if (model == null)
            {
                return this;
            }

            GrossComm += model.GrossComm;
            Commission += model.Commission;
            PositionTaking += model.PositionTaking;

            return this;
        }
    }
}