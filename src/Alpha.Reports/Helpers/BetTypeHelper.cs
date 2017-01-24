namespace Alpha.Reports.Helpers
{
    using Constants;
    using Extensions;

    public static class BetTypeHelper
    {
        public static bool IsNumberGame(int betType)
            => betType.IsBetween(BetTypeIdConstants.StartNumberGame, BetTypeIdConstants.EndNumberGame);
    }
}
