namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using System;
    using System.Collections.Generic;

    public class BetChoiceMatchingViewModel
    {
        public BetChoiceMatchingViewModel(DateTime winlostDate, DateTime minDate, IEnumerable<int> betCounts, int betCount)
        {
            WinlostDate = winlostDate;
            MinDate = minDate;
            BetCounts = betCounts;
            BetCount = betCount;
        }

        public DateTime WinlostDate { get; }

        public DateTime MinDate { get; }

        public int BetCount { get; }

        public IEnumerable<int> BetCounts { get; }
    }
}