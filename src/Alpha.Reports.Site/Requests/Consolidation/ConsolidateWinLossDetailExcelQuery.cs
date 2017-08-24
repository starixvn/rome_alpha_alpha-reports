using Alpha.Reports.Models;
using Alpha.Reports.Site.Requests.WinLossDetail;

namespace Alpha.Reports.Site.Requests.Consolidation
{
    public class ConsolidateWinLossDetailExcelQuery : WinLossDetailExcelQuery
    {
        public ConsolidateWinLossDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid() => ConsolidationDetailQuery.CanViewReport(this);
    }
}