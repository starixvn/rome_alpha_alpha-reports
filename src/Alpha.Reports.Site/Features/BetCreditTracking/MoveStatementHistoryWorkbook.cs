namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface MoveStatementHistoryWorkbook
    {
        Workbook<MoveStatementHistoryModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<MoveStatementHistoryModel> Sheet(string sheetName);

        Workbook<MoveStatementHistoryModel> Title(string titleText);

        Workbook<MoveStatementHistoryModel> Header(Func<string, string> headerMapping);

        Workbook<MoveStatementHistoryModel> Body(
            IEnumerable<MoveStatementHistoryModel> rows,
            Mapper<MoveStatementHistoryModel> mapper);

        byte[] ExportToBytes();

        MoveStatementHistoryWorkbook SetData(
            IEnumerable<MoveStatementHistoryModel> reportData,
            MoveStatementHistoryModel total);

        MoveStatementHistoryWorkbook SetRowMappers(
            Mapper<MoveStatementHistoryModel> mapper,
            Mapper<MoveStatementHistoryModel> totalMapper);
    }

    public class MoveStatementHistoryWorkbookImpl :
        NpoiWorkbook<MoveStatementHistoryModel>,
        MoveStatementHistoryWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<MoveStatementHistoryModel> ReportData { get; set; }

        internal MoveStatementHistoryModel Total { get; set; }

        internal Mapper<MoveStatementHistoryModel> Mapper { get; set; }

        internal Mapper<MoveStatementHistoryModel> TotalMapper { get; set; }

        internal Row MemberRowTemplate { get; set; }

        internal Row RefNoRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public MoveStatementHistoryWorkbook SetData(
            IEnumerable<MoveStatementHistoryModel> reportData, 
            MoveStatementHistoryModel total)
        {
            ReportData = reportData;
            Total = total;

            return this;
        }

        public MoveStatementHistoryWorkbook SetRowMappers(
            Mapper<MoveStatementHistoryModel> mapper, 
            Mapper<MoveStatementHistoryModel> totalMapper)
        {
            Mapper = mapper;
            TotalMapper = totalMapper;

            return this;
        }

        internal void FillMember(Sheet sheet, int rowIndex, MoveStatementHistoryModel data)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MemberRowTemplate);
            FillCells(row, data, TotalMapper.Mappings);
        }

        internal void FillRefNo(Sheet sheet, int rowIndex, MoveStatementHistoryModel item)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(RefNoRowTemplate);
            FillCells(row, item, Mapper.Mappings);
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            MemberRowTemplate = sheet.GetRow(currentRowIndex++);
            RefNoRowTemplate = sheet.GetRow(currentRowIndex++);
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.EvenRowTemplate = MemberRowTemplate;
            reportSheet.OddRowTemplate = RefNoRowTemplate;
            reportSheet.FooterRowsTemplate = new List<Row> { TotalRowTemplate };

            return reportSheet;
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (ReportData.Any())
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var data in ReportData)
            {
                if (currentRowIndex == TemplateInfo.BodyStartIndex) 
                {
                    FillMember(sheet, currentRowIndex++, data);
                }

                FillRefNo(sheet, currentRowIndex++, data);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (!SpreadSheet.FooterRows.Any())
            {
                return;
            }

            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(TotalRowTemplate);
            FillCells(row, Total, TotalMapper.Mappings);
        }
    }
}