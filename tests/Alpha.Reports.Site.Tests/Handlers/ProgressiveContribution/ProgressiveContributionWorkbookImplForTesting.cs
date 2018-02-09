namespace Alpha.Reports.Site.Tests.Handlers.ProgressiveContribution
{
    using ONEbook.SpreadSheets;
    using Site.Handlers.ProgressiveContribution;

    public class ProgressiveContributionWorkbookImplForTesting : ProgressiveContributionWorkbookImpl
    {
        public void CallFillBody(Sheet sheet)
        {
            FillBody(sheet);
        }

        public void CallFillBodyRows(Sheet sheet)
        {
            FillBodyRows(sheet);
        }

        public void CallFillFooterRows(Sheet sheet)
        {
            FillFooterRows(sheet);
        }
    }
}