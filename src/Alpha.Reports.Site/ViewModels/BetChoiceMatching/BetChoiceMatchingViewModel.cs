namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using System;
    using System.Collections.Generic;

    public class BetChoiceMatchingViewModel
    {
        public BetChoiceMatchingViewModel(DateTime winlostDate, IEnumerable<int> betCounts, int betCount)
        {
            WinlostDate = winlostDate;
            BetCounts = betCounts;
            BetCount = betCount;
        }

        public DateTime WinlostDate { get; }

        public int BetCount { get; }

        public IEnumerable<int> BetCounts { get; }
    }
}