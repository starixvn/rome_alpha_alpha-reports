namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using ONEbook.SpreadSheets;
    using Reports.Models.Statement;
    using Site.Handlers.Statement;

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

        public int CallGetNotedBalanceCellIndex(Row row)
        {
            return GetNotedBalanceCellIndex(row);
        }

        public void CallExportWinLoss(Row row, StatementModel data, int currentColIndex)
        {
            ExportWinLoss(row, data, currentColIndex);
        }
    }
}