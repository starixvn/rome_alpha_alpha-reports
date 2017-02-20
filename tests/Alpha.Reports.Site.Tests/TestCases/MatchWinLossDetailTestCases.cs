namespace Alpha.Reports.Site.Tests.TestCases
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;

    public static class MatchWinLossDetailTestCases
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static IEnumerable<MatchWinLossDetailModel> MemberMatchWinLossDetailData()
        {
            return new List<MatchWinLossDetailModel>
            {
                CreateMatchWinLossDetailModel(matchId: 21297148, betType: 221),
                CreateMatchWinLossDetailModel(matchId: 21297148, betType: 222),
                CreateMatchWinLossDetailModel(matchId: 21297148, betType: 223),
                CreateMatchWinLossDetailModel(matchId: 21297148, betType: 224),
                CreateMatchWinLossDetailModel(matchId: 21480284, betType: 222),
                CreateMatchWinLossDetailModel(matchId: 21543132, betType: 221),
                CreateMatchWinLossDetailModel(matchId: 21543132, betType: 222),
                CreateMatchWinLossDetailModel(matchId: 21543132, betType: 223),
                CreateMatchWinLossDetailModel(matchId: 21543132, betType: 29),
                CreateMatchWinLossDetailModel(matchId: 21543132, betType: 9)
            };
        }

        private static MatchWinLossDetailModel CreateMatchWinLossDetailModel(int matchId, int betType)
            => new MatchWinLossDetailModel(
                false,
                matchId,
                419,
                49,
                betType,
                false,
                1837,
                0,
                "1001",
                false,
                "02/15 03:45AM",
                (decimal)225000.00,
                (decimal)32.13,
                new NetWinLoss((decimal)-223800.00, (decimal)0.625),
                new NetWinLoss((decimal)390.00, (decimal)21.875),
                null,
                null);
    }
}
