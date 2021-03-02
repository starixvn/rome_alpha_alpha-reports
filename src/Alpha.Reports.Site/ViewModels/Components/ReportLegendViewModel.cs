namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ReportLegendViewModel
    {
        public ReportLegendViewModel(IEnumerable<string> legends, IEnumerable<string> grossCommNotes)
        {
            Legends = legends;
            GrossCommNotes = grossCommNotes;
        }

        public IEnumerable<string> Legends { get; }

        public IEnumerable<string> GrossCommNotes { get; }
    }
}