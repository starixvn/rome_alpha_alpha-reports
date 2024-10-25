namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Reports.Models.CommissionByBettype;
    using Services;

    public interface CommissionByBettypeWorkbook
    {
        Workbook<CommissionByBettypeModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<CommissionByBettypeModel> Title(string titleText);

        Workbook<CommissionByBettypeModel> Header(Func<string, string> headerMapping);

        Workbook<CommissionByBettypeModel> Body(IEnumerable<CommissionByBettypeModel> rows, Mapper<CommissionByBettypeModel> mapper);

        Workbook<CommissionByBettypeModel> Footer(CommissionByBettypeModel row, Mapper<CommissionByBettypeModel> rowMapper);

        Workbook<CommissionByBettypeModel> Sheet(string sheetName);

        byte[] ExportToBytes();
    }

    public class CommissionByBettypeWorkbookImpl : NpoiWorkbook<CommissionByBettypeModel>, CommissionByBettypeWorkbook
    {
        protected override void FillBody(Sheet sheet)
        {
            base.FillBody(sheet);

            RemoveNoPermissionInfo();
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            var lastColumnIndex = sheet.OddRowTemplate.LastCellIndex - 2;

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                lastColumnIndex,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        private void RemoveNoPermissionInfo()
        {
            var npoiSheet = Workbook.GetSheetAt(0);
            var headerAddress = npoiSheet.GetMergedRegion(0);

            npoiSheet.RemoveMergedRegion(0);

            RemoveColumnIndex(npoiSheet, 4);
            headerAddress.LastColumn--;

            npoiSheet.AddMergedRegion(headerAddress);
        }

        private void RemoveColumnIndex(ISheet sheet, int colIndex)
        {
            var rowIter = sheet.GetEnumerator();

            while (rowIter.MoveNext())
            {
                var row = (HSSFRow)rowIter.Current;
                var cell = row.GetCell(colIndex);

                if (cell != null)
                {
                    row.RemoveCell(cell);

                    for (int i = cell.ColumnIndex + 1; i <= row.Cells.Count; i++)
                    {
                        row.CopyCell(i, i - 1);
                        row.RemoveCell(row.GetCell(i));
                    }
                }
            }
        }
    }
}