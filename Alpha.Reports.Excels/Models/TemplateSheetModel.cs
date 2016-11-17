namespace Alpha.Reports.Excels.Models
{
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class TemplateSheetModel
    {
        private const int TemplateRowIndex = 1;

        private const int NoInfoColIndex = 2;
        private const int EvenRowNumberColIndex = 3;
        private const int OddRowNumberColIndex = 4;
        private const int EvenRowTextColIndex = 5;
        private const int OddRowTextColIndex = 6;
        private const int TotalRowNumberColIndex = 7;
        private const int TotalRowTextColIndex = 8;
        private const int EvenRowTotalNumberColIndex = 9;
        private const int OddRowTotalNumberColIndex = 10;

        private const string TemplateSheetName = "Template";

        public TemplateSheetModel(HSSFWorkbook workbook)
        {
            var templateRow = workbook.GetSheet(TemplateSheetName).GetRow(TemplateRowIndex);

            NoInfo = templateRow.GetCell(NoInfoColIndex);

            EvenRowNumber = templateRow.GetCell(EvenRowNumberColIndex);
            OddRowNumber = templateRow.GetCell(OddRowNumberColIndex);

            EvenRowText = templateRow.GetCell(EvenRowTextColIndex);
            OddRowText = templateRow.GetCell(OddRowTextColIndex);

            EvenRowTotalNumber = templateRow.GetCell(EvenRowTotalNumberColIndex);
            OddRowTotalNumber = templateRow.GetCell(OddRowTotalNumberColIndex);

            TotalRowNumber = templateRow.GetCell(TotalRowNumberColIndex);
            TotalRowText = templateRow.GetCell(TotalRowTextColIndex);
        }

        public ICell NoInfo { get; set; }

        public ICell EvenRowNumber { get; set; }

        public ICell OddRowNumber { get; set; }

        public ICell EvenRowText { get; set; }

        public ICell OddRowText { get; set; }

        public ICell TotalRowNumber { get; set; }

        public ICell TotalRowText { get; set; }

        public ICell EvenRowTotalNumber { get; set; }

        public ICell OddRowTotalNumber { get; set; }
    }
}
