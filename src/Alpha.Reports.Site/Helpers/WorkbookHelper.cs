namespace Alpha.Reports.Site.Helpers
{
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public static class WorkbookHelper
    {
        public static void RemoveColumnContent(ISheet sheet, int colIndex)
        {
            var rowIter = sheet.GetEnumerator();

            while (rowIter.MoveNext())
            {
                var row = (HSSFRow)rowIter.Current;
                var cell = row.GetCell(colIndex);

                if (cell == null)
                {
                    continue;
                }

                row.RemoveCell(cell);
            }
        }
    }
}