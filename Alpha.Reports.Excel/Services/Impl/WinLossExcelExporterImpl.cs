namespace Alpha.Reports.Excel.Services.Impl
{
    using System.Collections.Generic;
    using AppResources;
    using Extensions;
    using Models.Winloss;
    using Models.WinlossDetails;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Services;

    public class WinLossExcelExporterImpl : WinLossExcelExporter
    {
        private ProductService productService;

        private SpreadSheetEngine spreadSheetEngine;

        private Workbook workbook;

        public WinLossExcelExporterImpl(
            ProductService productService,
            SpreadSheetEngine spreadSheetEngine,
            Workbook workbook)
        {
            this.productService = productService;
            this.spreadSheetEngine = spreadSheetEngine;
            this.workbook = workbook;
        }

        public byte[] Export(ExportExcelParam<Models.Winloss.WinLoss> param, ExportWinLossParam exportWinLossParam)
        {
            var reportTitle = $"{param.ReportName} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}" +
                $" {Resources.of} " + string.Join(", ", productService.GetProductNames(exportWinLossParam.UserSelectedProductIds));
            var headerTexts = new Dictionary<string, string>
            {
                { "Username", Resources.Username },
                { "Turnover", Resources.Turnover },
                { "Gross Comm", Resources.GrossComm },
                { "Master Total", Resources.MasterTotal },
                { "Agent Total", Resources.AgentTotal },
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

            var spreadSheet = (new SpreadSheet<Models.Winloss.WinLoss>())
                .AddTitle(reportTitle)
                .AddHeaderTexts(headerTexts)
                .AddBody(param.ReportData.DataSource, BodyRowMapping)
                .AddFooter(param.ReportData.TotalData, FooterRowMapping)
                .AddSheetName(param.SheetName)
                .SetLayout(spreadSheetLayout.SetFreezePanel(4, "B"));

            return spreadSheetEngine.Export(spreadSheet, workbook);
        }

        private static IDictionary<string, object> BodyRowMapping(Models.Winloss.WinLoss data, int rowIndex)
        {
            var result = new Dictionary<string, object>
            {
                { "A", data.CustName },
                { "B", data.Turnover },
                { "C", data.GrossComm },
            };

            var columnName = 'D';
            columnName = AddNetWinLossColumns(data.MemberNetWinLoss, result, columnName);
            columnName = AddNetWinLossColumns(data.AgentNetWinLoss, result, columnName);
            columnName = AddNetWinLossColumns(data.MasterNetWinLoss, result, columnName);
            columnName = AddNetWinLossColumns(data.SuperNetWinLoss, result, columnName);
            result.Add(columnName.ToString(), data.Company);

            return result;
        }

        private static IDictionary<string, object> FooterRowMapping(Models.Winloss.WinLoss data, int rowIndex)
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

            if (netWinLoss.WinLoss != null)
            {
                resultColumns.Add(columnName.ToString(), netWinLoss.WinLoss);
                columnName++;

                resultColumns.Add(columnName.ToString(), netWinLoss.Comm);
                columnName++;
            }

            resultColumns.Add(columnName.ToString(), netWinLoss.Total);
            columnName++;

            return columnName;
        }
    }
}
