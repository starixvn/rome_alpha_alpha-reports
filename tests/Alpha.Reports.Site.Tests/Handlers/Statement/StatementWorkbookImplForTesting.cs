using Alpha.Reports.Site.Handlers.Statement;
using ONEbook.SpreadSheets;

namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    public class StatementWorkbookImplForTesting : StatementWorkbookImpl
    {
        public void CallFillNoInfoRow(Sheet sheet)
        {
            TemplateInfo = new TemplateInfo(string.Empty);

            FillNoInfoRow(sheet);
        }
    }
}
