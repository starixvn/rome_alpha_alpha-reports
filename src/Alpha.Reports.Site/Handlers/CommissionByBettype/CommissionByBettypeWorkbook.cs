namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Reports.Models.CommissionByBettype;

    public class CommissionByBettypeWorkbook : NpoiWorkbook<CommissionByBettypeModel>
    {
        public CommissionByBettypeWorkbook(bool showChargeCommColumn, bool showMyanmarOddsColumn, bool showOthersCommColumn)
        {
            ShowChargeCommColumn = showChargeCommColumn;
            ShowMyanmarOddsColumn = showMyanmarOddsColumn;
            ShowOthersCommColumn = showOthersCommColumn;
        }

        public bool ShowChargeCommColumn { get; }

        public bool ShowMyanmarOddsColumn { get; }

        public bool ShowOthersCommColumn { get; }

        protected override void FillBody(Sheet sheet)
        {
            base.FillBody(sheet);

            var npoiSheet = Workbook.GetSheetAt(0);
            var headerAddress = npoiSheet.GetMergedRegion(0);

            npoiSheet.RemoveMergedRegion(0);

            if (ShowOthersCommColumn == false)
            {
                RemoveColumnIndex(npoiSheet, 7);
                headerAddress.LastColumn--;
            }
            if (ShowMyanmarOddsColumn == false)
            {
                RemoveColumnIndex(npoiSheet, 5);
                headerAddress.LastColumn--;
            }
            if (ShowChargeCommColumn == false)
            {
                RemoveColumnIndex(npoiSheet, 4);
                headerAddress.LastColumn--;
            }

            npoiSheet.AddMergedRegion(headerAddress);
        }

        public void RemoveColumnIndex(ISheet sheet, int colIndex)
        {
            var rowIter = sheet.GetEnumerator();
            while (rowIter.MoveNext())
            {
                var row = (HSSFRow)rowIter.Current;
                var cell = row.GetCell(colIndex);

                row.RemoveCell(cell);

                for (int i = cell.ColumnIndex + 1; i <= row.Cells.Count; i++)
                {
                    row.MoveCell(row.GetCell(i), i - 1);
                }
            }
        }
    }
}