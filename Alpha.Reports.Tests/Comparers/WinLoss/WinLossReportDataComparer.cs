namespace Alpha.Reports.Tests.Comparers.WinLoss
{
    using Reports.Models;
    using Reports.Models.Winloss;

    public static class WinLossReportDataComparer
    {
        public static bool Equals(ReportData<WinLoss> reportDataA, ReportData<WinLoss> reportDataB)
        {
            if (reportDataA == null && reportDataB == null)
            {
                return true;
            }

            if (reportDataA == null || reportDataB == null)
            {
                return false;
            }

            return reportDataA.TotalRecords == reportDataB.TotalRecords
                && WinLossComparer.Equals(reportDataA.DataSource, reportDataB.DataSource)
                && WinLossComparer.Equals(reportDataA.TotalData, reportDataB.TotalData);
        }
    }
}
