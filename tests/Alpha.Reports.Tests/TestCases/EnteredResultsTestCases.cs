namespace Alpha.Reports.Tests.TestCases
{
    using System.Collections.Generic;
    using Reports.Models.Winloss;
    using Reports.Services;

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
                    isToday: false,
                    eventDate: DateTimeManager.Now.AddDays(-1),
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
                    isToday: true,
                    eventDate: DateTimeManager.Now,
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
                   isToday: false,
                   eventDate: DateTimeManager.Now.AddDays(-1),
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
                    isToday: true,
                    eventDate: DateTimeManager.Now,
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
