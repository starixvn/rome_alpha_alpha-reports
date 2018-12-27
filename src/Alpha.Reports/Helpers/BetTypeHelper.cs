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

        public static bool IsHappy5(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartHappy5, BetTypeIdConstants.EndHappy5);

        public static bool IsLucky3(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartLucky3, BetTypeIdConstants.EndLucky3);

        public static bool IsVirtualHorseRacing(int betType)
            => betType == BetTypeIdConstants.VirtualHorseRacing_Win
            || betType == BetTypeIdConstants.VirtualHorseRacing_Place
            || betType == BetTypeIdConstants.VirtualHorseRacing_WinPlace
            || betType == BetTypeIdConstants.VirtualHorseRacing_Forecast
            || betType == BetTypeIdConstants.VirtualHorseRacing_Tricast;
    }
}
