namespace Alpha.Reports.Site.ViewModels.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using System.Collections.Generic;

    public class BetChoiceMatchingQueryModel
    {
        public BetChoiceMatchingQueryModel(IEnumerable<BetChoiceMatchingModel> items)
        {
            Items = items;
        }

        public IEnumerable<BetChoiceMatchingModel> Items { get; }

        public IDictionary<int, string> Leagues { get; }
    }
}