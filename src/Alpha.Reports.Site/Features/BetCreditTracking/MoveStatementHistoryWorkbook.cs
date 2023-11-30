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
        Workbook<MoveStatementHistoryByRefNo> UseTemplate(TemplateInfo templateInfo);

        Workbook<MoveStatementHistoryByRefNo> Sheet(string sheetName);

        Workbook<MoveStatementHistoryByRefNo> Title(string titleText);

        Workbook<MoveStatementHistoryByRefNo> Header(Func<string, string> headerMapping);

        Workbook<MoveStatementHistoryByRefNo> Body(
            IEnumerable<MoveStatementHistoryByRefNo> rows,
            Mapper<MoveStatementHistoryByRefNo> mapper);

        byte[] ExportToBytes();

        MoveStatementHistoryWorkbook SetData(
            IEnumerable<MoveStatementHistoryByMember> data);

        MoveStatementHistoryWorkbook SetRowMappers(
            Mapper<MoveStatementHistoryByRefNo> memberMapper,
            Mapper<MoveStatementHistoryByRefNo> refNoMapper);
    }

    public class MoveStatementHistoryWorkbookImpl :
        NpoiWorkbook<MoveStatementHistoryByRefNo>,
        MoveStatementHistoryWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<MoveStatementHistoryByMember> Data { get; set; }

        internal Mapper<MoveStatementHistoryByRefNo> MemberMapper { get; set; }

        internal Mapper<MoveStatementHistoryByRefNo> RefNoMapper { get; set; }

        internal Row MemberRowTemplate { get; set; }

        internal Row RefNoRowTemplate { get; set; }

        internal Row RedRefNoRowTemplate { get; set; }

        public MoveStatementHistoryWorkbook SetData(IEnumerable<MoveStatementHistoryByMember> data)
        {
            Data = data;

            return this;
        }

        public MoveStatementHistoryWorkbook SetRowMappers(
            Mapper<MoveStatementHistoryByRefNo> memberMapper, 
            Mapper<MoveStatementHistoryByRefNo> refNoMapper)
        {
            MemberMapper = memberMapper;
            RefNoMapper = refNoMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            MemberRowTemplate = sheet.GetRow(currentRowIndex++);
            RefNoRowTemplate = sheet.GetRow(currentRowIndex++);
            RedRefNoRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);
            reportSheet.EvenRowTemplate = MemberRowTemplate;
            reportSheet.OddRowTemplate = RefNoRowTemplate;

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

            if (Data.Any())
            {
                FillBodyRows(sheet);
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

            foreach (var member in Data)
            {
                FillMemberTitle(sheet, currentRowIndex++, member);

                foreach (var row in member.RefNoRow)
                {
                    FillRefNo(sheet, currentRowIndex++, row);
                }
            }
        }

        internal void FillMemberTitle(Sheet sheet, int rowIndex, MoveStatementHistoryByMember member)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(MemberRowTemplate);
            FillCells(row, member.MemberRow, MemberMapper.Mappings);
        }

        internal void FillRefNo(Sheet sheet, int rowIndex, MoveStatementHistoryByRefNo refno)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(refno.IsMultipleMoved ? RedRefNoRowTemplate : RefNoRowTemplate);
            FillCells(row, refno, RefNoMapper.Mappings);
        }
    }
}