namespace Alpha.Reports.Site.Requests.Consolidation
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.WinLossDetail;

    public class ConsolidateWinLossDetailExcelQuery : WinLossDetailExcelQuery
    {
        public ConsolidateWinLossDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid() => ConsolidationDetailQuery.CanViewReport(this);
    }
}