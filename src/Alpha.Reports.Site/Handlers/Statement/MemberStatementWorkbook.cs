namespace Alpha.Reports.Site.Handlers.Statement
{
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Site.App_GlobalResources;
    using ONEbook.SpreadSheets;

    public interface MemberStatementWorkbook : StatementWorkbook
    {
    }

    public class MemberStatementWorkbookImpl : StatementWorkbookImpl, MemberStatementWorkbook
    {
        protected override void FillHeader(Sheet sheet)
        {
            var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);
            int colIndex = row.FirstCellIndex;

            row.SetCellValue(colIndex++, Resources.Date);
            row.SetCellValue(colIndex++, Resources.OpeningBalance);
            row.SetCellValue(colIndex++, Resources.Turnover);
            row.SetCellValue(colIndex++, Resources.Void + "/" + Resources.Rejected);
            row.SetCellValue(colIndex++, Resources.Outstanding);
            row.SetCellValue(colIndex++, Resources.WinLoss);
            row.SetCellValue(colIndex++, Resources.Commission);
            row.SetCellValue(colIndex++, Resources.GrandPrize);
            row.SetCellValue(colIndex++, Resources.TransferAmt);
            row.SetCellValue(colIndex, Resources.AccumulatedTotal);
        }

        protected override void ExportWinLoss(Row row, StatementModel data, int currentColIndex)
        {
            var colIndex = currentColIndex;
            row.SetCellValue(colIndex++, GetCellValue(data.Turnover));
            row.SetCellValue(colIndex++, GetCellValue(data.VoidAmount));
            row.SetCellValue(colIndex++, GetCellValue(data.Outstanding));
            row.SetCellValue(colIndex++, GetCellValue(data.WinLoss));
            row.SetCellValue(colIndex++, GetCellValue(data.Commission));
            row.SetCellValue(colIndex++, GetCellValue(data.GrandPrize));
            row.SetCellValue(colIndex++, GetCellValue(data.TransferAmount));
            row.SetCellValue(colIndex, GetCellValue(data.ClosingBalance));
        }

        protected override int GetNotedBalanceCellIndex(Row row)
            => row.FirstCellIndex + 10;
    }
}