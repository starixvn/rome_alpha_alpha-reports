namespace Alpha.Reports.Tests.TestCases
{
    using System;
    using System.Collections.Generic;
    using Reports.Models.Winloss;

    public static class EnteredResultsTestCases
    {
        private static IEnumerable<object[]> enteredResultsSources = new List<object[]>()
        {
            new object[] { BuildDailyEnteredSportData(), BuildEnteredSportResultData() }
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

        public static List<DailyEnteredSport> BuildDailyEnteredSportData()
        {
            var dailyEnteredSportData = new List<DailyEnteredSport>();

            dailyEnteredSportData.Add(TodaydayDailyEnteredBasketball);
            dailyEnteredSportData.Add(YesterdayDailyEnteredBasketball);

            dailyEnteredSportData.Add(TodayDailyEnteredSoccer);
            dailyEnteredSportData.Add(YesterdayDailyEnteredSoccer);

            return dailyEnteredSportData;
        }

        public static List<EnteredSportResult> BuildEnteredSportResultData()
        {
            var enteredSportResultData = new List<EnteredSportResult>();

            enteredSportResultData.Add(
               new EnteredSportResult(
                   sportTypeId: 2,
                   sportName: "BasketBall",
                   enteredSportToday: TodaydayDailyEnteredBasketball,
                   enteredSportYesterday: YesterdayDailyEnteredBasketball));

            enteredSportResultData.Add(
                new EnteredSportResult(
                    sportTypeId: 1,
                    sportName: "Soccer",
                    enteredSportToday: TodayDailyEnteredSoccer,
                    enteredSportYesterday: YesterdayDailyEnteredSoccer));

            return enteredSportResultData;
        }

        public static IEnumerable<object[]> EnteredResultsSources => enteredResultsSources;
    }
}