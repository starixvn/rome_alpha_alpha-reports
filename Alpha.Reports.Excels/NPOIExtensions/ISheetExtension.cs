namespace Alpha.Reports.Excels.NPOIExtensions
{
    using System;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public static class ISheetExtension
    {
        public static void CopyRow(this ISheet sourceSheet, ISheet targetSheet, int sourceRowIndex, int destinateRowIndex)
        {
            if (sourceSheet == null)
            {
                throw new ArgumentNullException("sourceSheet");
            }

            if (targetSheet == null)
            {
                throw new ArgumentNullException("targetSheet");
            }

            var sourceRow = sourceSheet.GetRow(sourceRowIndex);

            if (sourceRow == null)
            {
                return;
            }

            targetSheet.AppendMoreRows(destinateRowIndex);

            var targetRow = targetSheet.GetRow(destinateRowIndex) ?? targetSheet.CreateRow(destinateRowIndex);
            targetRow.Height = sourceRow.Height;

            sourceRow.CopyCells(targetRow);
        }

        public static void AppendMoreRows(this ISheet sheet, int destinateRowIndex)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            if (destinateRowIndex <= sheet.LastRowNum)
            {
                return;
            }

            for (var i = sheet.LastRowNum + 1; i <= destinateRowIndex; i++)
            {
                sheet.CreateRow(i);
            }
        }

        public static int MergeCells(this ISheet sourceSheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            if (sourceSheet == null)
            {
                throw new ArgumentNullException("sourceSheet");
            }

            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);

            return sourceSheet.AddMergedRegion(rangeAddress);
        }

        public static void CopyMergeInfo(this ISheet sourceSheet, ISheet targetSheet)
        {
            if (sourceSheet == null)
            {
                throw new ArgumentNullException("sourceSheet");
            }

            if (targetSheet == null)
            {
                throw new ArgumentNullException("targetSheet");
            }

            for (var i = 0; i < sourceSheet.NumMergedRegions; i++)
            {
                CellRangeAddress range = sourceSheet.GetMergedRegion(i);

                targetSheet.AddMergedRegion(range);
            }
        }

        public static void SetCellValue(this ISheet sourceSheet, int rowIndex, int colIndex, object value)
        {
            var row = sourceSheet.GetRow(rowIndex) ?? sourceSheet.CreateRow(rowIndex);
            row.SetCellValue(colIndex, value);
        }

        public static void SetCellValues(this ISheet sourceSheet, int rowIndex, int colIndex, params object[] values)
        {
            var row = sourceSheet.GetRow(rowIndex) ?? sourceSheet.CreateRow(rowIndex);
            row.SetCellValues(colIndex, values);
        }
    }
}
