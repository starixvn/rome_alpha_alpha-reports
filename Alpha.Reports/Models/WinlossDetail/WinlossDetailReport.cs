namespace Alpha.Reports.Models.WinlossDetail
{
    using System.Collections.Generic;

    public class WinlossDetailReport : ReportData<MemberWinLossDetail>
    {
        public WinlossDetailReport(
            IEnumerable<MemberWinLossDetail> dataSource,
            int currentPageIndex,
            int pageSize,
            int totalRecords,
            MemberWinLossDetail total) :
            base(dataSource, currentPageIndex, pageSize, totalRecords, total)
        {
        }
    }
}
