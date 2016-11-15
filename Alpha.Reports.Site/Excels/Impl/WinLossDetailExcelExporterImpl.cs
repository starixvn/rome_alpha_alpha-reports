namespace Alpha.Reports.Site.Excels.Impl
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using NPOI.SS.UserModel;
    using Reports.Extensions;
    using Requests;
    using ViewModels.WinLossDetails;

    public class WinLossDetailExcelExporterImpl : BaseExcelExporter, WinLossDetailExcelExporter
    {
        private UserLevel reportLevel;

        public byte[] Export(string templatePath, WinLossDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            reportLevel = viewModel.ReportLevel;

            var reportData = new ReportData<WinLossDetail>(
                viewModel.ReportData.DataSource.OrderBy(viewModel.SortPropertyName, viewModel.IsDescendingSort),
                viewModel.ReportData.CurrentPageIndex,
                viewModel.ReportData.PageSize,
                viewModel.ReportData.TotalRecords,
                viewModel.ReportData.TotalData);

            var maxColNumber = viewModel.ReportLevel.IsSuperLevel()
                ? 18
                : viewModel.ReportLevel.IsMasterLevel()
                ? 15
                : 12;

            return Export(new ExportExcelQuery<WinLossDetail>
            {
                TemplatePath = templatePath,
                Title = Resources.MemberWinLossDetail,
                ReportData = reportData,
                DataLength = viewModel.ReportData.DataSource.Count(),
                MaxColNumber = maxColNumber,
                FromDate = viewModel.FromDate,
                ToDate = viewModel.ToDate,
                SheetName = Resources.MemberWinLossDetail
            });
        }

        protected override void ExportHeader(ISheet sheet, int maxColNumber)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var currentRowIndex = TitleRowIndex + 1;
            var currentColIndex = StartColIndex;

            var templateCell = TemplateRow.GetCell(HeaderColIndex);

            templateCell.CopyCell(currentRowIndex, StartColIndex, maxColNumber);
            templateCell.CopyCell(currentRowIndex + 1, StartColIndex, maxColNumber);
            MergeHeaderCells(sheet);

            var currentRow = sheet.GetRow(currentRowIndex);

            currentRow.WriteCells(currentColIndex, Resources.Username, Resources.NoOfTickets, Resources.Turnover, Resources.NetTurnover, Resources.GrossComm);
            currentColIndex += 5;

            currentColIndex += ExportNetWinLossHeader(sheet, currentRow, Resources.Member, currentRowIndex, currentColIndex);
            currentColIndex += ExportNetWinLossHeader(sheet, currentRow, Resources.Agent, currentRowIndex, currentColIndex);

            var netWinLossGroupNumber = 2;

            if (reportLevel.IsSuperLevel() || reportLevel.IsMasterLevel())
            {
                currentColIndex += ExportNetWinLossHeader(sheet, currentRow, Resources.Master, currentRowIndex, currentColIndex);
                netWinLossGroupNumber++;
            }

            if (reportLevel.IsSuperLevel())
            {
                currentColIndex += ExportNetWinLossHeader(sheet, currentRow, Resources.Super, currentRowIndex, currentColIndex);
                netWinLossGroupNumber++;
            }

            currentRow.WriteCell(currentColIndex++, Resources.Company);

            currentRowIndex++;
            currentColIndex = StartColIndex + 5;
            currentRow = sheet.GetRow(currentRowIndex);

            for (var i = 0; i < netWinLossGroupNumber; i++)
            {
                currentRow.WriteCell(currentColIndex++, Resources.WinLoss);
                currentRow.WriteCell(currentColIndex++, Resources.Comm);
                currentRow.WriteCell(currentColIndex++, Resources.Total);
            }
        }

        protected override void ExportRow<T>(IRow row, T genericData, int dataIndex, int maxColNumber, bool isEvenRow = false)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            var currentColIndex = StartColIndex;
            var currentRowIndex = row.RowNum;

            var templateIndex = isEvenRow ? EvenRowTextIndex : OddRowTextIndex;

            TemplateRow.GetCell(templateIndex).CopyCell(currentRowIndex, currentColIndex);
            row.WriteCell(currentColIndex++, data.CustName);

            templateIndex = isEvenRow ? EvenRowNumberIndex : OddRowNumberIndex;
            TemplateRow.GetCell(templateIndex).CopyCell(currentRowIndex, currentColIndex, currentColIndex + maxColNumber - 2);

            templateIndex = isEvenRow ? EvenRowTotalNumberIndex : OddRowTotalNumberIndex;
            var templateCell = TemplateRow.GetCell(templateIndex);
            templateCell.CopyCell(currentRowIndex, currentColIndex + 6);
            templateCell.CopyCell(currentRowIndex, currentColIndex + 9);

            row.WriteCell(currentColIndex++, data.NumberOfTickets);

            row.WriteCell(currentColIndex++, data.Turnover);
            row.WriteCell(currentColIndex++, data.NetTurnover);
            row.WriteCell(currentColIndex++, data.GrossComm);

            currentColIndex += ExportNetWinLoss(row, data.Member, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Agent, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Master, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Super, currentColIndex);

            row.WriteCell(currentColIndex++, data.Company);
        }

        protected override void ExportTotalRow<T>(IRow row, T genericData, int maxColNumber)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            var currentColIndex = StartColIndex;
            var currentRowIndex = row.RowNum;

            TemplateRow.GetCell(TotalRowTextIndex).CopyCell(currentRowIndex, currentColIndex);
            row.WriteCell(currentColIndex++, Resources.Total);

            TemplateRow.GetCell(TotalRowNumberIndex).CopyCell(currentRowIndex, currentColIndex, currentColIndex + maxColNumber - 2);

            row.WriteCell(currentColIndex++, data.NumberOfTickets);

            row.WriteCell(currentColIndex++, data.Turnover);
            row.WriteCell(currentColIndex++, data.NetTurnover);
            row.WriteCell(currentColIndex++, data.GrossComm);

            currentColIndex += ExportNetWinLoss(row, data.Member, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Agent, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Master, currentColIndex);
            currentColIndex += ExportNetWinLoss(row, data.Super, currentColIndex);

            row.WriteCell(currentColIndex++, data.Company);
        }

        private int ExportNetWinLoss(IRow row, CustWinLoss netWinLoss, int currentColIndex)
        {
            if (netWinLoss == null)
            {
                return 0;
            }

            row.WriteCell(currentColIndex++, netWinLoss.WinLoss);
            row.WriteCell(currentColIndex++, netWinLoss.Comm);
            row.WriteCell(currentColIndex++, netWinLoss.Total);

            return 3;
        }

        private int ExportNetWinLossHeader(ISheet sheet, IRow row, string title, int currentRowIndex, int currentColIndex)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            row.WriteCell(currentColIndex, title);

            return 3;
        }

        private void MergeHeaderCells(ISheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var currentRowIndex = TitleRowIndex + 1;
            var currentColIndex = StartColIndex;

            for (var i = 0; i < 5; i++)
            {
                sheet.MergeCells(currentRowIndex, currentColIndex + i, currentRowIndex + 1, currentColIndex + i);
            }

            currentColIndex += 5;

            sheet.MergeCells(currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 2);
            currentColIndex += 3;

            sheet.MergeCells(currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 2);
            currentColIndex += 3;

            sheet.MergeCells(currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 2);
            currentColIndex += 3;

            sheet.MergeCells(currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 2);
            currentColIndex += 3;

            sheet.MergeCells(currentRowIndex, currentColIndex, currentRowIndex + 1, currentColIndex);
        }
    }
}