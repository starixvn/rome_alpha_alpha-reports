namespace Alpha.Reports.Tests.TestCases
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Reports.Models.Winloss;

    public class EnteredResultsTestCases : IEnumerable<object[]>
    {
        private DailyEnteredSport YesterdayDailyEnteredSoccer
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: false,
                    eventDate: DateTime.Now.AddDays(-1),
                    sportType: 1,
                    entered: 5,
                    nonEntered: 6);
            }
        }

        private DailyEnteredSport TodayDailyEnteredSoccer
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: true,
                    eventDate: DateTime.Now,
                    sportType: 1,
                    entered: 5,
                    nonEntered: 6);
            }
        } 

        private DailyEnteredSport YesterdayDailyEnteredBasketball
        {
            get
            {
                return new DailyEnteredSport(
                   isToday: false,
                   eventDate: DateTime.Now.AddDays(-1),
                   sportType: 2,
                   entered: 5,
                   nonEntered: 6);
            }
        } 

        private DailyEnteredSport TodaydayDailyEnteredBasketball
        {
            get
            {
                return new DailyEnteredSport(
                    isToday: true,
                    eventDate: DateTime.Now,
                    sportType: 2,
                    entered: 5,
                    nonEntered: 6);
            }
        } 

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { BuildDailyEnteredSportData(), BuildEnteredSportResultData() };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<DailyEnteredSport> BuildDailyEnteredSportData()
        {
            var dailyEnteredSportData = new List<DailyEnteredSport>();

            dailyEnteredSportData.Add(TodayDailyEnteredSoccer);
            dailyEnteredSportData.Add(YesterdayDailyEnteredSoccer);

            dailyEnteredSportData.Add(YesterdayDailyEnteredBasketball);
            dailyEnteredSportData.Add(TodaydayDailyEnteredBasketball);

            return dailyEnteredSportData;
        }

        public List<EnteredSportResult> BuildEnteredSportResultData()
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
    }
}
