namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System.Collections.Generic;

    public static class BetChoiceMatchingConstants
    {
#pragma warning disable S109 // Magic numbers should not be used
        public static List<int> DefaultBetCounts => new List<int> { 2, 3, 4, 5, 8, 10, 15, 20, 30, 50, 100 };
#pragma warning restore S109 // Magic numbers should not be used
    }
}
