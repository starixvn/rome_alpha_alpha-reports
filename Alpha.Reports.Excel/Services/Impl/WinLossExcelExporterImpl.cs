namespace Alpha.Reports.Excel.Services.Impl
{
    using System;
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

        public byte[] ExportWinLossExcel(ExportExcelParam<WinLoss> param, ExportWinLossParam exportWinLossParam)
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

            var spreadSheet = (new SpreadSheet<WinLoss>())
                .AddTitle(reportTitle)
                .AddHeaderTexts(headerTexts)
                .AddBody(param.ReportData.DataSource, WinLossBodyRowMapping)
                .AddFooter(param.ReportData.TotalData, WinLossFooterRowMapping)
                .AddSheetName(param.SheetName)
                .SetLayout(spreadSheetLayout);

            return spreadSheetEngine.Export(spreadSheet, workbook);
        }

        public byte[] ExportEnteredResultExcel(ExportExcelParam<EnteredSportResult> param)
        {
            var headerTexts = new Dictionary<string, string>
            {
                { "Date", Resources.Date },
                { "Yesterday", string.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1)) },
                { "Today", string.Format("{0:MM/dd/yyyy}", DateTime.Now) },
                { "Results", Resources.Results },
                { "Entered", Resources.Entered },
                { "Non-Entered", Resources.NonEntered },
            };

            workbook.Load(param.TemplatePath);
            var spreadSheetLayout = new SpreadSheetLayout(noInfomationText: Resources.NoInformationIsAvailable);

            var spreadSheet = (new SpreadSheet<EnteredSportResult>())
                .AddTitle(Resources.MatchResultEntered)
                .AddHeaderTexts(headerTexts)
                .AddBody(param.ReportData.DataSource, EnteredResultsBodyRowMapping)
                .AddSheetName(param.SheetName)
                .SetLayout(spreadSheetLayout);

            return spreadSheetEngine.Export(spreadSheet, workbook);
        }

        private static IDictionary<string, object> WinLossBodyRowMapping(WinLoss data, int rowIndex)
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

        private static IDictionary<string, object> WinLossFooterRowMapping(WinLoss data, int rowIndex)
        {
            var result = WinLossBodyRowMapping(data, rowIndex);
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

        private static IDictionary<string, object> EnteredResultsBodyRowMapping(EnteredSportResult data, int rowIndex)
        {
            var result = new Dictionary<string, object>
            {
                { "A", data.SportName },
            };

            var columnName = 'B';
            columnName = AddEnteredSportDailyColumns(data.EnterSportYesterday, result, columnName);
            columnName = AddEnteredSportDailyColumns(data.EnterSportToday, result, columnName);

            return result;
        }

        private static char AddEnteredSportDailyColumns(EnteredSportDaily enteredSportDaily, Dictionary<string, object> resultColumns, char columnName)
        {
            resultColumns.Add(columnName.ToString(), enteredSportDaily == null ? 0 : enteredSportDaily.Entered);
            columnName++;

            resultColumns.Add(columnName.ToString(), enteredSportDaily == null ? 0 : enteredSportDaily.NonEntered);
            columnName++;

            return columnName;
        }
    }
}
