namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using ONEbook.SpreadSheets;
    using Site.Handlers.Consolidation;
    using Site.Handlers.WinLossByProduct;
    using Site.ViewModels.Consolidation;

    public class WinLossByProductWorkbookImplForTesting : ConsolidationByProductWorkbookImpl
    {
        public void CallFillBodyRows(Sheet sheet)
        {
            FillBodyRows(sheet);
        }

        public void InitSheet()
        {
            SpreadSheet = new SpreadSheet<ConsolidationByProductViewModel>();
            TemplateInfo = new TemplateInfo(templatePath: string.Empty);
        }
    }
}
