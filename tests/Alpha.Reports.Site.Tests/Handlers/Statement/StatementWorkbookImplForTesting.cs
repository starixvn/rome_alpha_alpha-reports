namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using Alpha.Reports.Site.Handlers.Statement;
    using ONEbook.SpreadSheets;

    public class StatementWorkbookImplForTesting : StatementWorkbookImpl
    {
        public void CallFillNoInfoRow(Sheet sheet)
        {
            TemplateInfo = new TemplateInfo(string.Empty);

            FillNoInfoRow(sheet);
        }
    }
}
