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

        private static DailyEnteredSport YesterdayDailyEnteredSoccer
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: 0,
                    eventDate: new DateTime(2016, 12, 1),
                    sportType: 1,
                    entered: 5,
                    nonEntered: 6);
            }
        }

        private static DailyEnteredSport TodayDailyEnteredSoccer
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: 1,
                    eventDate: new DateTime(2016, 12, 2),
                    sportType: 1,
                    entered: 5,
                    nonEntered: 6);
            }
        }

        private static DailyEnteredSport YesterdayDailyEnteredBasketball
        {
            get
            {
                return new DailyEnteredSport(
                   isToday: 0,
                   eventDate: new DateTime(2016, 12, 1),
                   sportType: 2,
                   entered: 5,
                   nonEntered: 6);
            }
        }

        private static DailyEnteredSport TodaydayDailyEnteredBasketball
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: 1,
                    eventDate: new DateTime(2016, 12, 2),
                    sportType: 2,
                    entered: 5,
                    nonEntered: 6);
            }
        }

        public static List<DailyEnteredSport> BuildDailyEnteredSportData()
        {
            var dailyEnteredSportData = new List<DailyEnteredSport>();

            dailyEnteredSportData.Add(TodayDailyEnteredSoccer);
            dailyEnteredSportData.Add(YesterdayDailyEnteredSoccer);

            dailyEnteredSportData.Add(YesterdayDailyEnteredBasketball);
            dailyEnteredSportData.Add(TodaydayDailyEnteredBasketball);

            return dailyEnteredSportData;
        }

        public static List<EnteredSportResult> BuildEnteredSportResultData()
        {
            var enteredSportResultData = new List<EnteredSportResult>();

            enteredSportResultData.Add(
               new EnteredSportResult(
                   sportTypeId: 2,
                   sportName: "BasketBall",
                   enterSportToday: TodaydayDailyEnteredBasketball,
                   enterSportYesterday: YesterdayDailyEnteredBasketball));

            enteredSportResultData.Add(
                new EnteredSportResult(
                    sportTypeId: 1,
                    sportName: "Soccer",
                    enterSportToday: TodayDailyEnteredSoccer,
                    enterSportYesterday: YesterdayDailyEnteredSoccer));

            return enteredSportResultData;
        }

        public static IEnumerable<object[]> EnteredResultsSources
        {
            get
            {
                return enteredResultsSources;
            }
        }
    }
}