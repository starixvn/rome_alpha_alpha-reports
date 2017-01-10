namespace Alpha.Reports.Tests.TestCases
{
    using System;
    using System.Collections.Generic;
    using Reports.Models.Winloss;

    public static class EnteredResultsTestCases
    {
        private static IEnumerable<object[]> enteredResultsSources = new List<object[]>()
        {
            new object[] { DailyEnteredSportData(), EnteredSportResultData() }
        };

        private static DailyEnteredSport YesterdayDailyEnteredSoccer =>
            new DailyEnteredSport(
                isToday: false,
                eventDate: new DateTime(2016, 12, 1),
                sportType: 1,
                entered: 5,
                nonEntered: 6);

        private static DailyEnteredSport TodayDailyEnteredSoccer =>
            new DailyEnteredSport(
                isToday: true,
                eventDate: new DateTime(2016, 12, 2),
                sportType: 1,
                entered: 5,
                nonEntered: 6);

        private static DailyEnteredSport YesterdayDailyEnteredBasketball =>
            new DailyEnteredSport(
                isToday: false,
                eventDate: new DateTime(2016, 12, 1),
                sportType: 2,
                entered: 5,
                nonEntered: 6);

        private static DailyEnteredSport TodaydayDailyEnteredBasketball =>
            new DailyEnteredSport(
                isToday: true,
                eventDate: new DateTime(2016, 12, 2),
                sportType: 2,
                entered: 5,
                nonEntered: 6);

        private static IEnumerable<DailyEnteredSport> DailyEnteredSportData()
        {
            yield return TodaydayDailyEnteredBasketball;
            yield return YesterdayDailyEnteredBasketball;

            yield return TodayDailyEnteredSoccer;
            yield return YesterdayDailyEnteredSoccer;
        }

        private static IEnumerable<EnteredSportResult> EnteredSportResultData()
        {
            yield return new EnteredSportResult(
                    sportTypeId: 1,
                    sportName: "Soccer",
                    enteredSportToday: TodayDailyEnteredSoccer,
                    enteredSportYesterday: YesterdayDailyEnteredSoccer);

            yield return new EnteredSportResult(
                  sportTypeId: 2,
                  sportName: "BasketBall",
                  enteredSportToday: TodaydayDailyEnteredBasketball,
                  enteredSportYesterday: YesterdayDailyEnteredBasketball);     
        }

        public static IEnumerable<object[]> EnteredResultsSources => enteredResultsSources;
    }
}