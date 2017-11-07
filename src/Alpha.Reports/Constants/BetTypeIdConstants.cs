namespace Alpha.Reports.Constants
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1310:FieldNamesMustNotContainUnderscore",
        Justification = "Reviewed. Suppression is OK here.")]
    public static class BetTypeIdConstants
    {
        public const int Outright = 10;

        public const int MixParlay = 9;
        public const int SystemParlay = 29;
        public const int MixParlayBetRadarVirtualSports = 2799;

        public const int StartRacing = 31;
        public const int EndRacing = 33;
        public const int StartRacingTote = 41;
        public const int EndRacingTote = 43;

        public const int StartNumberGame = 81;
        public const int EndNumberGame = 120;

        public const int NumberGameOverUnder = 85;
        public const int NumberGameOddEven = 86;

        public const int VirtualHorseRacing_Win = 1231;
        public const int VirtualHorseRacing_Place = 1232;
        public const int VirtualHorseRacing_WinPlace = 1233;
        public const int VirtualHorseRacing_Forecast = 1237;
        public const int VirtualHorseRacing_Tricast = 1238;

        public const int ChargeCommission = 500;

        public const int StartLucky3 = 1600;
        public const int EndLucky3 = 1602;
        public const int Bullfighting = 1004;
    }
}
