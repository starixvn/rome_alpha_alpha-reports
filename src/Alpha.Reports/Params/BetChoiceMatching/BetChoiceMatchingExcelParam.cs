namespace Alpha.Reports.Params.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using System;

    public class BetChoiceMatchingExcelParam : BetChoiceMatchingParam
    {
        public BetChoiceMatchingExcelParam(
            int custId,
            DateTime winlostDate,
            DateTime moveBetsDay,
            int betCount,
            string username,
            decimal? stake,
            MemberWinLossOperators memberWinLossOperator,
            decimal? memberWinLossValue)
            : base(custId, winlostDate, moveBetsDay, betCount, true)
        {
            Username = username;
            Stake = stake;
            MemberWinLossOperator = memberWinLossOperator;
            MemberWinLossValue = memberWinLossValue;
        }

        public string Username { get; }

        public decimal? Stake { get; }

        public MemberWinLossOperators MemberWinLossOperator { get; }

        public decimal? MemberWinLossValue { get; }
    }
}
