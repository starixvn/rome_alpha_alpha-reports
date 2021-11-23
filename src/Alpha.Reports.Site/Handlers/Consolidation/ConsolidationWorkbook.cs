namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels.Consolidation;

    public class ConsolidationWorkbook<T> : NpoiWorkbook<T> where T : class
    {
        protected void SetWinLossByUsernameValue(ConsolidationEntityViewModel data, Row row)
        {
            var custLevelNameIndex = 1;
            row.SetCellValue(custLevelNameIndex++, data.CustLevelName);
            row.SetCellValue(custLevelNameIndex++, data.Username);
            row.SetCellValue(custLevelNameIndex++, data.CurrencyName);
            row.SetCellValue(custLevelNameIndex++, data.Turnover);
            row.SetCellValue(custLevelNameIndex++, data.GrossComm);
            row.SetCellValue(custLevelNameIndex++, data.MemberWinLoss);
            row.SetCellValue(custLevelNameIndex++, data.MemberComm);
            row.SetCellValue(custLevelNameIndex++, data.MemberTotal);
            row.SetCellValue(custLevelNameIndex++, data.OwnerTurnover);
            row.SetCellValue(custLevelNameIndex++, data.OwnerWinLoss);
            row.SetCellValue(custLevelNameIndex++, data.OwnerComm);
            row.SetCellValue(custLevelNameIndex++, data.OwnerTotal);
            row.SetCellValue(custLevelNameIndex++, data.Company);
            row.SetCellValue(custLevelNameIndex, data.BetCount);
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