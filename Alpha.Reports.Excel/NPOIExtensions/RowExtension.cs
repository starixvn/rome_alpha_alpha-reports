namespace Alpha.Reports.Excel.NPOIExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NPOI.SS.UserModel;

    public static class RowExtension
    {
        public static void CopyCells(this IRow sourceRow, IRow targetRow)
        {
            if (sourceRow == null)
            {
                throw new ArgumentNullException("sourceRow");
            }

            if (targetRow == null)
            {
                throw new ArgumentNullException("targetRow");
            }

            var firstCellNum = sourceRow.FirstCellNum;
            var lastCellNum = sourceRow.LastCellNum;

            for (var i = firstCellNum; i < lastCellNum; i++)
            {
                var sourceCell = sourceRow.GetCell(i);

                if (sourceCell == null)
                {
                    continue;
                }

                var targetCell = targetRow.GetCell(i) ?? targetRow.CreateCell(i);

                sourceCell.CopyCell(targetCell);
            }
        }

        public static void SetCellValues(this IRow row, int colIndex, params object[] values)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            foreach (var value in values)
            {
                row.SetCellValue(colIndex++, value);
            }
        }

        public static void SetCellValuesByValueList(this IRow row, int colIndex, IEnumerable<object> values)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            row.SetCellValues(colIndex, values.ToArray());
        }

        public static void SetCellValue(this IRow row, int colIndex, object value)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
            cell.SetCellValue(value);
        }
    }
}
