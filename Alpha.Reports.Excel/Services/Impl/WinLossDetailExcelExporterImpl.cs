namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using AppResources;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using NPOI.SS.UserModel;
    using NPOIExtensions;
    using Params;
    using Reports.Services;

    public class WinLossDetailExcelExporterImpl : BaseExcelExporter, WinLossDetailExcelExporter
    {
        private ProductService productService;

        private ExportWinLossDetailParam exportWinLossDetailParam;

        private List<string> levelGroups;

        public WinLossDetailExcelExporterImpl(ProductService productService)
        {
            this.productService = productService;
        }

        public byte[] Export(ExportExcelParam<WinLossDetail> param, ExportWinLossDetailParam exportWinLossDetailParam)
        {
            this.exportWinLossDetailParam = exportWinLossDetailParam;

            levelGroups = GetLevelGroups(exportWinLossDetailParam.UserLevel);
            GetRowValuesFunc = (genericData) =>
            {
                var result = new List<object>()
                {
                    genericData.CustName,
                    genericData.NumberOfTickets,
                    genericData.Turnover,
                    genericData.NetTurnover,
                    genericData.GrossComm
                };

                GetNetWinLossValues(genericData.MemberNetWinLoss, result);
                GetNetWinLossValues(genericData.AgentNetWinLoss, result);
                GetNetWinLossValues(genericData.MasterNetWinLoss, result);
                GetNetWinLossValues(genericData.SuperNetWinLoss, result);

                result.Add(genericData.Company);

                return result;
            };

            return Export(param);
        }

        protected override void FillTitle<T>(ISheet sheet, ExportExcelParam<T> param)
        {
            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of} " + string.Join(", ", productService.GetProductNames(exportWinLossDetailParam.UserSelectedProductIds));

            sheet.SetCellValue(TitleRowIndex, StartColIndex, reportTitle);
        }

        protected override void FillHeader(ISheet sheet)
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

        protected override void FreezePanel(ISheet sheet)
        {
            sheet.CreateFreezePane(StartColIndex + 1, 3);
        }

        private static void GetNetWinLossValues(NetWinLoss netWinLoss, List<object> columnValues)
        {
            if (netWinLoss == null)
            {
                return;
            }

            columnValues.Add(netWinLoss.WinLoss);
            columnValues.Add(netWinLoss.Comm);
            columnValues.Add(netWinLoss.Total);
        }

        private List<string> GetLevelGroups(UserLevel userLevel)
        {
            var levelGroups = new List<string> { Resources.Member, Resources.Agent };

            if (userLevel.IsMasterOrSuperLevel())
            {
                levelGroups.Add(Resources.Master);
            }

            if (userLevel.IsSuperLevel())
            {
                levelGroups.Add(Resources.Super);
            }

            return levelGroups;
        }
    }
}
