namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ReportLegendViewModel
    {
        public ReportLegendViewModel(
            IEnumerable<string> legends,
            IEnumerable<string> grossCommNotes,
            IEnumerable<string> legendsAfterGrossCommNotes,
            bool mobileFriendly = false)
        {
            Legends = legends;
            GrossCommNotes = grossCommNotes;
            LegendsAfterGrossCommNotes = legendsAfterGrossCommNotes;
            MobileFriendly = mobileFriendly;
        }

        public IEnumerable<string> Legends { get; }

        public IEnumerable<string> GrossCommNotes { get; }

        public IEnumerable<string> LegendsAfterGrossCommNotes { get; }

        public bool MobileFriendly { get; }
    }
}