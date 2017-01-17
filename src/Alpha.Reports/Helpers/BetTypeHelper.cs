namespace Alpha.Reports.Helpers
{
    using Constants;
    using Extensions;

    public static class BetTypeHelper
    {
        public static bool IsRacing(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartRacing, BetTypeIdConstants.EndRacing)
            || betType.IsBetween(BetTypeIdConstants.StartRacingTote, BetTypeIdConstants.EndRacingTote);

        public static bool IsNumberGame(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartNumberGame, BetTypeIdConstants.EndNumberGame);

        public static bool IsVirtualHorseRacing(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartVirtualHorseRacing, BetTypeIdConstants.EndVirtualHorseRacing);
    }
}
