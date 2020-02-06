namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Utils;
    using System;
    using System.Collections.Generic;

    public class BetChoiceMatchingViewModel
    {
        const int DefaultBetCount = 2;
        public static readonly IList<int> DefaultBetCounts = new List<int> { 2, 3, 4, 5, 8, 10, 15, 20, 30, 50, 100 };

        public DateTime WinlostDate { get; } = SystemTime.Today;

        public int BetCount { get; } = DefaultBetCount;

        public IList<int> BetCounts { get; } = DefaultBetCounts;
    }
}