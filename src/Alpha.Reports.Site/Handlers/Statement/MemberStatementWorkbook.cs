using System;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Helpers;
using NPOI.HSSF.UserModel;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace Alpha.Reports.Site.Handlers.Statement
{
    public interface MemberStatementWorkbook
    {
        Workbook<StatementModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<StatementModel> Sheet(string sheetName);

        Workbook<StatementModel> Title(string titleText);

        Workbook<StatementModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();
    }

    public class MemberStatementWorkbookImpl : NpoiWorkbook<StatementModel>, MemberStatementWorkbook
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

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                sheet.OddRowTemplate.LastCellIndex - 2,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            var currentRowIndex = TemplateInfo.BodyStartIndex;
            var isEven = false;
            StatementModel previousStatement = null;

            var npoiSheet = ((NpoiSheet)sheet).Sheet;
            var notedBalanceCellIndex = 0;

            foreach (var data in SpreadSheet.BodyRows)
            {
                var templateRow = isEven ? sheet.EvenRowTemplate : sheet.OddRowTemplate;
                var row = sheet.GetRow(currentRowIndex);
                var hssfRow = (HSSFRow)npoiSheet.GetRow(currentRowIndex);

                row.CopyCellsFrom(templateRow);

                var currentColIndex = row.FirstCellIndex;
                notedBalanceCellIndex = currentColIndex + 10;

                if (previousStatement != null
                    && data.OpeningBalance != previousStatement.ClosingBalance)
                {
                    hssfRow.CopyCell(notedBalanceCellIndex, currentColIndex + 1);
                }

                row.SetCellValue(currentColIndex++, data.WinLossDateString);
                row.SetCellValue(currentColIndex++, data.OpeningBalance);

                row.SetCellValue(currentColIndex++, GetCellValue(data.Turnover));
                row.SetCellValue(currentColIndex++, GetCellValue(data.VoidAmount));
                row.SetCellValue(currentColIndex++, GetCellValue(data.Outstanding));
                row.SetCellValue(currentColIndex++, GetCellValue(data.WinLoss));
                row.SetCellValue(currentColIndex++, GetCellValue(data.Commission));
                row.SetCellValue(currentColIndex++, GetCellValue(data.GrandPrize));
                row.SetCellValue(currentColIndex++, GetCellValue(data.TransferAmount));
                row.SetCellValue(currentColIndex, GetCellValue(data.ClosingBalance));

                previousStatement = data;
                currentRowIndex++;
                isEven = !isEven;
            }

            WorkbookHelper.RemoveColumnContent(Workbook.GetSheetAt(0), notedBalanceCellIndex);
        }

        internal object GetCellValue(decimal? value) => (object)value ?? Resources.Symbol_NoDataNumber;
    }
}