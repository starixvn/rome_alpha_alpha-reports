namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels.Consolidation;

    public class ConsolidationWorkbook<T> : NpoiWorkbook<T> where T : class
    {
        protected void SetWinLossByUsernameValue(ConsolidationByUsernameViewModel data, Row row)
        {
            row.SetCellValue(1, data.CustLevelName);
            row.SetCellValue(2, data.Username);
            row.SetCellValue(3, data.CurrencyName);
            row.SetCellValue(4, data.Turnover);
            row.SetCellValue(5, data.MemberWinLoss);
            row.SetCellValue(6, data.MemberComm);
            row.SetCellValue(7, data.MemberTotal);
            row.SetCellValue(8, data.OwnerTurnover);
            row.SetCellValue(9, data.OwnerWinLoss);
            row.SetCellValue(10, data.OwnerComm);
            row.SetCellValue(11, data.OwnerTotal);
            row.SetCellValue(12, data.Company);
            row.SetCellValue(13, data.BetCount);
        }

        protected void MergeCells(Sheet sheet, Func<T, int> getGroupLength)
        {
            var rowIndex = TemplateInfo.BodyStartIndex;

            foreach (var group in SpreadSheet.BodyRows)
            {
                var row = sheet.GetRow(rowIndex);
                var itemCount = getGroupLength(group);
                var endRowIndex = rowIndex + itemCount - 1;

                if (rowIndex < endRowIndex)
                {
                    var newRange = new CellRangeAddress(rowIndex, endRowIndex, row.FirstCellIndex, row.FirstCellIndex);

                    ((NpoiSheet)sheet).Sheet.AddMergedRegion(newRange);
                }

                rowIndex += itemCount;
            }
        }
    }
}