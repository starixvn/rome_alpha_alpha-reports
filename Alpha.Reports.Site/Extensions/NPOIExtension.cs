namespace Alpha.Reports.Site.Extensions
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public static class NPOIExtension
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

            var firstCellNum = Math.Min((int)sourceRow.FirstCellNum, 0);
            var lastCellNum = Math.Min(sourceRow.LastCellNum, int.MaxValue);

            for (var i = firstCellNum; i < lastCellNum; i++)
            {
                var sourceCell = sourceRow.GetCell(i);

                if (sourceCell == null)
                {
                    continue;
                }

                var targetCell = targetRow.CreateCell(i);
                sourceCell.CopyCell(targetCell);

                sourceCell.CopyMergeInfo(targetCell);
            }
        }

        public static void CopyMergeInfo(this ICell sourceCell, ICell targetCell)
        {
            if (sourceCell == null)
            {
                throw new ArgumentNullException("sourceCell");
            }

            if (targetCell == null)
            {
                throw new ArgumentNullException("targetCell");
            }

            CellRangeAddress sourceRange = null;

            for (var j = 0; j < sourceCell.Sheet.NumMergedRegions; j++)
            {
                CellRangeAddress range = sourceCell.Sheet.GetMergedRegion(j);

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

        public static ICell GetCell(this ISheet sourceSheet, int rowIndex, int colIndex)
        {
            if (sourceSheet == null)
            {
                throw new ArgumentNullException("sourceSheet");
            }

            return sourceSheet.GetRow(rowIndex).GetCell(colIndex);
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

        public static void WriteCells(this IRow row, int colId, params object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 1 && values[0] is List<object>)
            {
                values = ((List<object>)values[0]).ToArray();
            }

            int id = colId;

            foreach (var value in values)
            {
                row.WriteCell(id, value);
                id++;
            }
        }

        public static void SetCellValue(this ICell cell, object value)
        {
            if (cell == null)
            {
                throw new ArgumentNullException("cell");
            }

            if (value == null)
            {
                cell.SetCellValue("0");
            }
            else if (value is bool)
            {
                cell.SetCellValue(Converter.To<bool>(value));
            }
            else if (value is DateTime)
            {
                cell.SetCellValue(Converter.To<DateTime>(value));
            }
            else if (value is double)
            {
                cell.SetCellValue(Converter.To<double>(value));
            }
            else if (value is IRichTextString)
            {
                cell.SetCellValue(Converter.To<IRichTextString>(value));
            }
            else if (value is string)
            {
                cell.SetCellValue(Converter.ToString2(value));
            }
            else if (value is int
                || value is long
                || value is decimal)
            {
                cell.SetCellValue(Converter.To<double>(value));
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

            var row = sourceCell.Sheet.GetRow(rowIndex) ?? sourceCell.Sheet.CreateRow(rowIndex);

            for (int startIndex = colIndex, endIndex = colIndex + duplicateColNumber, i = startIndex; i < endIndex; i++)
            {
                var targetCell = row.GetCell(i) ?? row.CreateCell(i);
                sourceCell.CopyCell(targetCell);
            }
        }

        public static int WriteCell(this IRow row, int cellIndex, object value)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);
            cell.SetCellValue(value);

            return 1;
        }
    }
}