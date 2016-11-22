namespace Alpha.Reports.Excel.NPOIExtensions
{
    using System;
    using Fanex.Common;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public static class CellExtension
    {
        public static void CopyMergeInfo(this ICell sourceCell, ICell targetCell)
        {
            if (sourceCell == null)
            {
                throw new ArgumentNullException("sourceCell");
            }

            if (!sourceCell.IsMergedCell)
            {
                return;
            }

            if (targetCell == null)
            {
                throw new ArgumentNullException("targetCell");
            }

            CellRangeAddress sourceRange = null;

            for (var i = 0; i < sourceCell.Sheet.NumMergedRegions; i++)
            {
                CellRangeAddress range = sourceCell.Sheet.GetMergedRegion(i);

                if (range.FirstColumn == sourceCell.ColumnIndex
                    && range.FirstRow == sourceCell.RowIndex)
                {
                    sourceRange = range;
                    break;
                }
            }

            if (sourceRange != null)
            {
                CellRangeAddress newRange = new CellRangeAddress(
                    targetCell.RowIndex,
                    targetCell.RowIndex + sourceRange.LastRow - sourceRange.FirstRow,
                    targetCell.ColumnIndex,
                    targetCell.ColumnIndex + sourceRange.LastColumn - sourceRange.FirstColumn);

                targetCell.Sheet.AddMergedRegion(newRange);
            }
        }

        public static void CopyCell(this ICell sourceCell, ICell targetCell)
        {
            if (sourceCell == null)
            {
                throw new ArgumentNullException("sourceCell");
            }

            if (targetCell == null)
            {
                throw new ArgumentNullException("targetCell");
            }

            sourceCell.CopyMergeInfo(targetCell);

            targetCell.CellComment = sourceCell.CellComment ?? default(IComment);
            targetCell.Hyperlink = sourceCell.Hyperlink ?? default(IHyperlink);
            targetCell.CellStyle = sourceCell.CellStyle ?? default(ICellStyle);

            targetCell.SetCellType(sourceCell.CellType);

            switch (targetCell.CellType)
            {
                case CellType.Boolean:
                    targetCell.SetCellValue(sourceCell.BooleanCellValue);
                    break;
                case CellType.Numeric:
                    targetCell.SetCellValue(sourceCell.NumericCellValue);
                    break;
                case CellType.String:
                    targetCell.SetCellValue(sourceCell.StringCellValue);
                    break;
                case CellType.Error:
                    targetCell.SetCellValue(sourceCell.ErrorCellValue);
                    break;
                case CellType.Formula:
                    targetCell.CellFormula = sourceCell.CellFormula;
                    break;
            }
        }

        public static void SetCellValue(this ICell cell, object value)
        {
            if (cell == null)
            {
                throw new ArgumentNullException("cell");
            }

            var isNumber = value is double
                || value is int
                || value is long
                || value is decimal;

            if (isNumber)
            {
                cell.SetCellValue(Converter.To<double>(value));
            }
            else if (value is string)
            {
                cell.SetCellValue(Converter.ToString2(value));
            }
            else if (value is bool)
            {
                cell.SetCellValue(Converter.To<bool>(value));
            }
            else if (value is DateTime)
            {
                cell.SetCellValue(Converter.To<DateTime>(value));
            }
            else if (value == null)
            {
                cell.SetCellValue("0");
            }
            else if (value is IRichTextString)
            {
                cell.SetCellValue(Converter.To<IRichTextString>(value));
            }
            else
            {
                cell.SetCellValue("Undefined");
            }
        }

        public static void CopyCell(this ICell sourceCell, int rowIndex, int colIndex, int duplicateColNumber = 1)
        {
            if (sourceCell == null)
            {
                throw new ArgumentNullException("sourceCell");
            }

            sourceCell.CopyCell(sourceCell.Sheet, rowIndex, colIndex, duplicateColNumber);
        }

        public static void CopyCell(this ICell sourceCell, ISheet targetSheet, int rowIndex, int colIndex, int duplicateColNumber = 1)
        {
            if (sourceCell == null)
            {
                throw new ArgumentNullException("sourceCell");
            }

            var row = targetSheet.GetRow(rowIndex) ?? targetSheet.CreateRow(rowIndex);

            for (int startIndex = colIndex, endIndex = colIndex + duplicateColNumber, i = startIndex; i < endIndex; i++)
            {
                var targetCell = row.GetCell(i) ?? row.CreateCell(i);
                sourceCell.CopyCell(targetCell);
            }
        }

        public static void CopyToMergedCells(this ICell sourceCell, ISheet targetSheet, int rowIndex, int colIndex, int endColIndex)
        {
            sourceCell.CopyCell(targetSheet, rowIndex, colIndex);
            sourceCell.CopyCell(targetSheet, rowIndex, endColIndex);

            targetSheet.MergeCells(rowIndex, colIndex, rowIndex, endColIndex);
        }
    }
}
