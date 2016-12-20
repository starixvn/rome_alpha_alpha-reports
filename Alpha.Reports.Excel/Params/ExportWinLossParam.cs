namespace Alpha.Reports.Excel.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Models;
    using Models.Winloss;

    public class ExportWinLossParam
    {
        public ExportWinLossParam(
            string templatePath,
            string reportName,
            string sheetName,
            IEnumerable<WinLoss> reportData,
            WinLoss grantTotal,
            DateTime fromDate,
            DateTime toDate,
            UserLevel userLevel,
            string userSelectedProductIds)
        {
            TemplatePath = templatePath;
            ReportName = reportName;
            SheetName = sheetName;
            ReportData = reportData;
            GrantTotal = grantTotal;
            FromDate = fromDate;
            ToDate = toDate;
            UserLevel = userLevel;
            UserSelectedProductIds = string.IsNullOrWhiteSpace(userSelectedProductIds)
                ? Enumerable.Empty<int>()
                : userSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));
        }

        public string TemplatePath { get; }

        public string ReportName { get;}

        public string SheetName { get;}

        public IEnumerable<WinLoss> ReportData { get;}

        public WinLoss GrantTotal { get; }

        public DateTime FromDate { get;}

        public DateTime ToDate { get;}

        public UserLevel UserLevel { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }
    }
}
