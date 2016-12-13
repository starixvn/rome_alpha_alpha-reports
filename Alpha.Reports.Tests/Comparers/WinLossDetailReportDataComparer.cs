namespace Alpha.Reports.Tests.Comparers
{
    using Reports.Models;
    using Reports.Models.WinlossDetails;

    public static class WinLossDetailReportDataComparer
    {
        public static bool Equals(ReportData<WinLossDetail> reportDataA, ReportData<WinLossDetail> reportDataB)
        {
            if (reportDataA == null && reportDataB == null)
            {
                return true;
            }

            if (reportDataA == null || reportDataB == null)
            {
                return false;
            }

            return reportDataA.CurrentPageIndex == reportDataB.CurrentPageIndex
                && reportDataA.PageSize == reportDataB.PageSize
                && reportDataA.TotalRecords == reportDataB.TotalRecords
                && WinLossDetailComparer.Equals(reportDataA.TotalData, reportDataB.TotalData)
                && WinLossDetailComparer.Equals(reportDataA.DataSource, reportDataB.DataSource);
        }
    }
}
