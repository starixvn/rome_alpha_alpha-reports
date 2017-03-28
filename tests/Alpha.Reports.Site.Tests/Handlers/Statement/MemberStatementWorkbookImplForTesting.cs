using Alpha.Reports.Site.Handlers.Statement;
using ONEbook.SpreadSheets;

namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    public class MemberStatementWorkbookImplForTesting : MemberStatementWorkbookImpl
    {
        public void CallFillHeader(Sheet sheet)
        {
            TemplateInfo = new TemplateInfo(string.Empty);

            FillHeader(sheet);
        }

        public void CallFillNoInfoRow(Sheet sheet)
        {
            TemplateInfo = new TemplateInfo(string.Empty);

            FillNoInfoRow(sheet);
        }
    }
}