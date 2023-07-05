namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ReportLegendViewModel
    {
        public ReportLegendViewModel(
            IEnumerable<string> legends,
            IEnumerable<string> grossCommNotes,
            bool mobileFriendly = false)
        {
            Legends = legends;
            GrossCommNotes = grossCommNotes;
            MobileFriendly = mobileFriendly;
        }

        public IEnumerable<string> Legends { get; }

        public IEnumerable<string> GrossCommNotes { get; }

        public bool MobileFriendly { get; }
    }
}