namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class ConsolidationByDateViewModel
    {
        public ConsolidationByDateViewModel(DateTime date, IEnumerable<ConsolidationByUsernameViewModel> winLossByUsername)
        {
            Date = date;
            WinLossByUsername = winLossByUsername;
        }

        public DateTime Date { get; }

        public string DateText => Date.ToShortDateWithSlash();

        public IEnumerable<ConsolidationByUsernameViewModel> WinLossByUsername { get; }
    }
}