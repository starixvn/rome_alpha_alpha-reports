namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using System.Collections.Generic;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class MoveStatementHistoryExcelQuery : MoveStatementHistoryQuery, IAsyncRequest<ExcelFileViewModel>
    {
        public MoveStatementHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public IEnumerable<MoveStatementHistoryByMember> Data { get; set; }
    }
}