namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System.ComponentModel;

    public enum MemberWinLossOperators
    {
        [Description(">=")]
        GreatorOrEqual = 0,

        [Description("=")]
        Equal = 1,

        [Description("<=")]
        LessOrEqual = 2
    }
}
