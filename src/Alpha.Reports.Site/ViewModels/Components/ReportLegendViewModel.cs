namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using Alpha.Reports.Site.App_GlobalResources;

    public class ReportLegendViewModel
    {
        public ReportLegendViewModel(
            IEnumerable<string> legends,
            IEnumerable<string> grossCommNotes,
            string commissionNote = null,
            bool mobileFriendly = false)
        {
            Legends = legends;
            GrossCommNotes = grossCommNotes;
            CommissionNote = commissionNote ?? Resources.GrossCommRoundingNote;
            MobileFriendly = mobileFriendly;
        }

        public IEnumerable<string> Legends { get; }

        public IEnumerable<string> GrossCommNotes { get; }

        public string CommissionNote { get; }

        public bool MobileFriendly { get; }
    }
}