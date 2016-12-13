namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using AppResources;
    using Extensions;
    using Models.WinlossDetails;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Services;

    public class WinLossDetailExcelExporterImpl : WinLossDetailExcelExporter
    {
        private ProductService productService;

        private SpreadSheetEngine spreadSheetEngine;

        private Workbook workbook;

        public WinLossDetailExcelExporterImpl(
            ProductService productService,
            SpreadSheetEngine spreadSheetEngine,
            Workbook workbook)
        {
            this.productService = productService;
            this.spreadSheetEngine = spreadSheetEngine;
            this.workbook = workbook;
        }

        public byte[] Export(ExportExcelParam<WinLossDetail> param, ExportWinLossDetailParam exportWinLossDetailParam)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (exportWinLossDetailParam == null)
            {
                throw new ArgumentNullException(nameof(exportWinLossDetailParam));
            }

            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of} " + string.Join(", ", productService.GetProductNames(exportWinLossDetailParam.UserSelectedProductIds));
            var headerTexts = new Dictionary<string, string>
            {
                { "Username", Resources.Username },
                { "No. of Tickets", Resources.NoOfTickets },
                { "Turnover", Resources.Turnover },
                { "Net Turnover", Resources.NetTurnover },
                { "Gross Comm", Resources.GrossComm },
                { "Member", Resources.Member },
                { "Agent", Resources.Agent },
                { "Master", Resources.Master },
                { "Super", Resources.Super },
                { "Company", Resources.Company },
                { "Win Loss", Resources.WinLoss },
                { "Comm", Resources.Comm },
                { "Total", Resources.Total }
            };

            workbook.Load(param.TemplatePath);
            var spreadSheetLayout = new SpreadSheetLayout(noInfomationText: Resources.NoInformationIsAvailable);

            var spreadSheet = (new SpreadSheet<WinLossDetail>())
                .AddTitle(reportTitle)
                .AddHeaderTexts(headerTexts)
                .AddBody(param.ReportData.DataSource, BodyRowMapping)
                .AddFooter(param.ReportData.TotalData, FooterRowMapping)
                .AddSheetName(param.SheetName)
                .SetLayout(spreadSheetLayout.SetFreezePanel(4, "B"));

            return spreadSheetEngine.Export(spreadSheet, workbook);
        }

        private static IDictionary<string, object> BodyRowMapping(WinLossDetail data, int rowIndex)
        {
            var result = new Dictionary<string, object>
            {
                { "A", data.CustName },
                { "B", data.NumberOfTickets },
                { "C", data.Turnover },
                { "D", data.NetTurnover },
                { "E", data.GrossComm },
                { "F", data.MemberNetWinLoss.WinLoss },
                { "G", data.MemberNetWinLoss.Comm },
                { "H", data.MemberNetWinLoss.Total },
                { "I", data.AgentNetWinLoss.WinLoss },
                { "J", data.AgentNetWinLoss.Comm },
                { "K", data.AgentNetWinLoss.Total }
            };

            var columnName = 'L';
            columnName = AddNetWinLossColumns(data.MasterNetWinLoss, result, columnName);
            columnName = AddNetWinLossColumns(data.SuperNetWinLoss, result, columnName);
            result.Add(columnName.ToString(), data.Company);

            return result;
        }

        private static IDictionary<string, object> FooterRowMapping(WinLossDetail data, int rowIndex)
        {
            var result = BodyRowMapping(data, rowIndex);
            result["A"] = Resources.Total;

            return result;
        }

        private static char AddNetWinLossColumns(NetWinLoss netWinLoss, Dictionary<string, object> resultColumns, char columnName)
        {
            if (netWinLoss == null)
            {
                return columnName;
            }

            resultColumns.Add(columnName.ToString(), netWinLoss.WinLoss);
            columnName++;

            resultColumns.Add(columnName.ToString(), netWinLoss.Comm);
            columnName++;

            resultColumns.Add(columnName.ToString(), netWinLoss.Total);
            columnName++;

            return columnName;
        }
    }
}
