namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using AppResources;
    using Models;
    using Models.WinlossDetails;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOIExtensions;
    using Params;

    public class WinLossDetailExcelExporterImpl : BaseExcelExporter, WinLossDetailExcelExporter
    {
        private List<string> levelGroups;

        private IRow evenRowTemplate;
        private IRow oddRowTemplate;
        private IRow totalRowTemplate;

        public byte[] Export(ExportExcelParam<WinLossDetail> param, UserLevel userLevel)
        {
            levelGroups = GetLevelGroups(userLevel);

            return Export(param);
        }

        protected override void LoadTemplate(HSSFWorkbook workbook)
        {
            var templateSheet = workbook.GetSheet(TemplateSheetName);

            oddRowTemplate = templateSheet.GetRow(3);
            evenRowTemplate = templateSheet.GetRow(4);
            totalRowTemplate = templateSheet.GetRow(5);

            workbook.RemoveSheetAt(1);

            EndColIndex = oddRowTemplate.LastCellNum - 1;
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
                currentRow.SetCellValues(currentColIndex, group);
                sheet.SetCellValues(currentRowIndex + 1, currentColIndex, Resources.WinLoss, Resources.Comm, Resources.Total);
                currentColIndex += 3;
            }

            currentRow.SetCellValues(currentColIndex, Resources.Company);
        }

        protected override void ExportRow<T>(ISheet sheet, IRow row, T genericData, int dataIndex, bool isEvenRow = false)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            var rowTemplate = isEvenRow ? evenRowTemplate : oddRowTemplate;
            rowTemplate.CopyCells(row);
            row.Height = rowTemplate.Height;

            row.SetCellValues(StartColIndex, data.CustName, data.NumberOfTickets, data.Turnover, data.NetTurnover, data.GrossComm);
            ExportNetWinLoss(row, data.MemberNetWinLoss, StartColIndex + 5);
            ExportNetWinLoss(row, data.AgentNetWinLoss, StartColIndex + 8);
            ExportNetWinLoss(row, data.MasterNetWinLoss, StartColIndex + 11);
            ExportNetWinLoss(row, data.SuperNetWinLoss, StartColIndex + 14);
            row.SetCellValue(EndColIndex, data.Company);
        }

        protected override void ExportTotalRow<T>(ISheet sheet, IRow row, T genericData)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            var data = genericData as WinLossDetail;

            totalRowTemplate.CopyCells(row);
            row.Height = totalRowTemplate.Height;

            row.SetCellValues(StartColIndex, Resources.Total, data.NumberOfTickets, data.Turnover, data.NetTurnover, data.GrossComm);
            ExportNetWinLoss(row, data.MemberNetWinLoss, StartColIndex + 5);
            ExportNetWinLoss(row, data.AgentNetWinLoss, StartColIndex + 8);
            ExportNetWinLoss(row, data.MasterNetWinLoss, StartColIndex + 11);
            ExportNetWinLoss(row, data.SuperNetWinLoss, StartColIndex + 14);
            row.SetCellValue(EndColIndex, data.Company);
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

        private void ExportNetWinLoss(IRow row, NetWinLoss netWinLoss, int currentColIndex)
        {
            if (netWinLoss == null)
            {
                return;
            }

            row.SetCellValues(currentColIndex, netWinLoss.WinLoss, netWinLoss.Comm, netWinLoss.Total);
        }
    }
}
