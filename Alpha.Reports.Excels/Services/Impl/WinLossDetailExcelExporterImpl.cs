namespace Alpha.Reports.Excels.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using AppResources;
    using NPOI.SS.UserModel;
    using NPOIExtensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossDetails;

    public class WinLossDetailExcelExporterImpl : BaseExcelExporter, WinLossDetailExcelExporter
    {
        private List<string> levelGroups;

        public byte[] Export(ExportExcelParam<WinLossDetail> param, UserLevel userLevel)
        {
            levelGroups = GetLevelGroups(userLevel);
            param.MaxColNumber = 6 + (levelGroups.Count * 3);

            return Export(param);
        }

        protected override void ExportHeader(ISheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException("sheet");
            }

            var currentRowIndex = HeaderRowIndex;
            var currentRow = sheet.GetRow(currentRowIndex);

            currentRow.SetCellValues(
                StartColIndex,
                Resources.Username,
                Resources.NoOfTickets,
                Resources.Turnover,
                Resources.NetTurnover,
                Resources.GrossComm);

            var currentColIndex = StartColIndex + 5;
            foreach (var group in levelGroups)
            {
                ExportNetWinLossHeader(currentRow, group, currentColIndex);
                sheet.SetCellValues(currentRowIndex + 1, currentColIndex, Resources.WinLoss, Resources.Comm, Resources.Total);
                currentColIndex += 3;
            }

            currentRow.SetCellValues(currentColIndex++, Resources.Company);
        }

        protected override void ExportRow<T>(ISheet sheet, IRow row, T genericData, int dataIndex, bool isEvenRow = false)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            var currentColIndex = StartColIndex;
            var currentRowIndex = row.RowNum;

            var templateCell = isEvenRow ? TemplateSheetModel.EvenRowText : TemplateSheetModel.OddRowText;
            templateCell.CopyCell(sheet, currentRowIndex, currentColIndex++);

            templateCell = isEvenRow ? TemplateSheetModel.EvenRowNumber : TemplateSheetModel.OddRowNumber;
            templateCell.CopyCell(sheet, currentRowIndex, currentColIndex, EndColIndex);

            templateCell = isEvenRow ? TemplateSheetModel.EvenRowTotalNumber : TemplateSheetModel.OddRowTotalNumber;

            for (var i = currentColIndex + 6; i <= EndColIndex; i += 3)
            {
                templateCell.CopyCell(sheet, currentRowIndex, i);
            }

            currentColIndex = StartColIndex;
            row.SetCellValues(currentColIndex, data.CustName, data.NumberOfTickets, data.Turnover, data.NetTurnover, data.GrossComm);
            currentColIndex += 5;

            currentColIndex = ExportNetWinLoss(row, data.MemberNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.AgentNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.MasterNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.SuperNetWinLoss, currentColIndex);

            row.SetCellValue(currentColIndex, data.Company);
        }

        protected override void ExportTotalRow<T>(ISheet sheet, IRow row, T genericData)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            var currentColIndex = StartColIndex;
            var currentRowIndex = row.RowNum;

            TemplateSheetModel.TotalRowText.CopyCell(sheet, currentRowIndex, currentColIndex++);
            TemplateSheetModel.TotalRowNumber.CopyCell(sheet, currentRowIndex, currentColIndex, EndColIndex);

            currentColIndex = StartColIndex;
            row.SetCellValues(currentColIndex, Resources.Total, data.NumberOfTickets, data.Turnover, data.NetTurnover, data.GrossComm);
            currentColIndex += 5;

            currentColIndex = ExportNetWinLoss(row, data.MemberNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.AgentNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.MasterNetWinLoss, currentColIndex);
            currentColIndex = ExportNetWinLoss(row, data.SuperNetWinLoss, currentColIndex);

            row.SetCellValues(currentColIndex, data.Company);
        }

        private List<string> GetLevelGroups(UserLevel userLevel)
        {
            var levelGroups = new List<string> { Resources.Member, Resources.Agent };

            if (userLevel.IsSuperLevel() || userLevel.IsMasterLevel())
            {
                levelGroups.Add(Resources.Master);
            }

            if (userLevel.IsSuperLevel())
            {
                levelGroups.Add(Resources.Super);
            }

            return levelGroups;
        }

        private void ExportNetWinLossHeader(IRow row, string title, int currentColIndex)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            row.SetCellValues(currentColIndex, title);
        }

        private int ExportNetWinLoss(IRow row, NetWinLoss netWinLoss, int currentColIndex)
        {
            if (netWinLoss == null)
            {
                return currentColIndex;
            }

            row.SetCellValues(currentColIndex, netWinLoss.WinLoss, netWinLoss.Comm, netWinLoss.Total);

            return currentColIndex + 3;
        }
    }
}
