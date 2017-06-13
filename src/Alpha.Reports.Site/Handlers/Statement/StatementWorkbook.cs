using System;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Helpers;
using NPOI.HSSF.UserModel;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace Alpha.Reports.Site.Handlers.Statement
{
    public interface StatementWorkbook
    {
        Workbook<StatementModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<StatementModel> Sheet(string sheetName);

        Workbook<StatementModel> Title(string titleText);

        Workbook<StatementModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();
    }

    public class StatementWorkbookImpl : NpoiWorkbook<StatementModel>, StatementWorkbook
    {
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
                notedBalanceCellIndex = notedBalanceCellIndex == 0 ? GetNotedBalanceCellIndex(row) : notedBalanceCellIndex;

                if (previousStatement != null
                    && data.OpeningBalance != previousStatement.ClosingBalance)
                {
                    hssfRow.CopyCell(notedBalanceCellIndex, currentColIndex + 1);
                }

                row.SetCellValue(currentColIndex++, data.WinLossDateString);
                row.SetCellValue(currentColIndex++, data.OpeningBalance);

                ExportWinLoss(row, data, currentColIndex);

                previousStatement = data;
                currentRowIndex++;
                isEven = !isEven;
            }

            WorkbookHelper.RemoveColumnContent(Workbook.GetSheetAt(0), notedBalanceCellIndex);
        }

        protected virtual void ExportWinLoss(Row row, StatementModel data, int currentColIndex)
        {
            var colIndex = currentColIndex;
            row.SetCellValue(colIndex++, GetCellValue(data.Turnover));
            row.SetCellValue(colIndex++, GetCellValue(data.WinLoss));
            row.SetCellValue(colIndex++, GetCellValue(data.Commission));
            row.SetCellValue(colIndex++, GetCellValue(data.TransferAmount));
            row.SetCellValue(colIndex, GetCellValue(data.ClosingBalance));
        }

        protected virtual int GetNotedBalanceCellIndex(Row row)
            => row.FirstCellIndex + 7;

        internal static object GetCellValue(decimal? value) => (object)value ?? Resources.Symbol_NoDataNumber;
    }
}