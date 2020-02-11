namespace Alpha.Reports.Site.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Site.App_GlobalResources;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface BetChoiceMatchingWorkbook
    {
        Workbook<BetChoiceMatchingModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<BetChoiceMatchingModel> Sheet(string sheetName);

        Workbook<BetChoiceMatchingModel> Title(string titleText);

        Workbook<BetChoiceMatchingModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        BetChoiceMatchingWorkbook Body(IEnumerable<BetChoiceMatchingEventModel> models);
    }

    public class BetChoiceMatchingWorkbookImpl : NpoiWorkbook<BetChoiceMatchingModel>, BetChoiceMatchingWorkbook
    {
        private int currentRowIndex;

        public BetChoiceMatchingWorkbook Body(IEnumerable<BetChoiceMatchingEventModel> models)
        {
            Models = models;
            decimal totalStake = 0;
            decimal totalMemberWinLoss = 0;
            decimal totalSuperWinLoss = 0;
            decimal totalCompany = 0;
            bool isRunning = true;
            foreach (var model in models)
            {
                model.Users.ForEach(user =>
                {
                    totalStake += user.Stake;
                    if (!user.IsRunning)
                    {
                        totalMemberWinLoss += user.MemberWinLoss;
                        totalSuperWinLoss += user.SuperWinLoss;
                        totalCompany += user.Company;
                        isRunning = false;
                    }
                });
            }

            Total = new BetChoiceMatchingModel(totalStake, totalMemberWinLoss, totalSuperWinLoss, totalCompany, isRunning);
            return this;
        }

        public BetChoiceMatchingModel Total { get; private set; }

        public IEnumerable<BetChoiceMatchingEventModel> Models { get; private set; }

        public Row TemplateEventRow { get; private set; }

        public Row TemplateEvenUserTotalRow { get; private set; }

        public Row TemplateEvenUserDataRow { get; private set; }

        public Row TemplateOddUserTotalRow { get; private set; }

        public Row TemplateOddUserDataRow { get; private set; }

        public Row TemplateTotalRow { get; private set; }

        public Row TemplateNoInfoRow { get; private set; }

        protected override Sheet LoadTemplateSheet()
        {
            var sheetIndex = TemplateInfo.SheetIndex;
            var templateSheet = GetSheetAt(sheetIndex++);
            var rowIndex = 0;
            TemplateEventRow = templateSheet.GetRow(rowIndex++);
            TemplateEvenUserTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateEvenUserDataRow = templateSheet.GetRow(rowIndex++);
            TemplateOddUserTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateOddUserDataRow = templateSheet.GetRow(rowIndex++);
            TemplateTotalRow = templateSheet.GetRow(rowIndex++);
            TemplateNoInfoRow = templateSheet.GetRow(rowIndex);
            return GetSheetAt(sheetIndex);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);
            if (Models.Any())
            {
                currentRowIndex = TemplateInfo.BodyStartIndex;
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            var row = sheet.GetRow(TemplateInfo.BodyStartIndex);
            row.CopyCellsFrom(TemplateTotalRow);
            row.SetCellValue(Column.GetColIndex(TemplateInfo.ColumnStartName), TemplateInfo.NoInfomationText);
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            foreach (var model in Models)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(TemplateEventRow);
                row.SetCellValue(Column.GetColIndex(Column.H), model.GlobalShowTime);
                var userIndex = 0;
                model.Users.ForEach(user =>
                {
                    var isEvenRow = userIndex % 2 == 0;
                    row = sheet.GetRow(currentRowIndex++);
                    row.CopyCellsFrom(isEvenRow ? TemplateEvenUserTotalRow : TemplateOddUserTotalRow);
                    row.SetCellValue(Column.GetColIndex(Column.A), user.Username);
                    row.SetCellValue(Column.GetColIndex(Column.B), Resources.Total);
                    row.SetCellValue(Column.GetColIndex(Column.F), user.Stake);
                    if (!user.IsRunning)
                    {
                        row.SetCellValue(Column.GetColIndex(Column.G), user.MemberWinLoss);
                        row.SetCellValue(Column.GetColIndex(Column.H), user.SuperWinLoss);
                        row.SetCellValue(Column.GetColIndex(Column.I), user.Company);
                    }
                    
                    var templateUserDataRow = isEvenRow ? TemplateEvenUserTotalRow : TemplateOddUserTotalRow;
                    user.Items.ForEach(item =>
                    {
                        row = sheet.GetRow(currentRowIndex++);
                        row.CopyCellsFrom(templateUserDataRow);
                        row.SetCellValue(Column.GetColIndex(Column.A), item.Username);
                        row.SetCellValue(Column.GetColIndex(Column.B), item.TransDate);
                        row.SetCellValue(Column.GetColIndex(Column.C), item.TransId);
                        row.SetCellValue(Column.GetColIndex(Column.D), item.Odds);
                        row.SetCellValue(Column.GetColIndex(Column.E), item.IP);
                        row.SetCellValue(Column.GetColIndex(Column.F), item.Stake);
                        if (!item.IsRunning)
                        {
                            row.SetCellValue(Column.GetColIndex(Column.G), user.MemberWinLoss);
                            row.SetCellValue(Column.GetColIndex(Column.H), user.SuperWinLoss);
                            row.SetCellValue(Column.GetColIndex(Column.I), user.Company);
                        }
                    });
                    
                    userIndex++;
                });
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(TemplateTotalRow);
            row.SetCellValue(Column.GetColIndex(Column.B), Resources.Total);
            row.SetCellValue(Column.GetColIndex(Column.F), Total.Stake);
            if (!Total.IsRunning)
            {
                row.SetCellValue(Column.GetColIndex(Column.G), Total.MemberWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.H), Total.SuperWinLoss);
                row.SetCellValue(Column.GetColIndex(Column.I), Total.Company);
            }
        }
    }
}