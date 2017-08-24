namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using ONEbook.SpreadSheets;
    using Site.Handlers.Consolidation;
    using Site.ViewModels.Consolidation;

    public class WinLossByDateWorkbookImplForTesting : WinLossByDateWorkbookImpl
    {
        public void CallFillBodyRows(Sheet sheet)
        {
            FillBodyRows(sheet);
        }

        public void InitSheet()
        {
            SpreadSheet = new SpreadSheet<ConsolidationByDateViewModel>();
            TemplateInfo = new TemplateInfo(templatePath: string.Empty);
        }
    }
}
