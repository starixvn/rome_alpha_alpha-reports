namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using System.Collections.Generic;

    public class MoveStatementHistoryExcelQuery : MoveStatementHistoryQuery, IRequest<ExcelFileViewModel>
    {
        public MoveStatementHistoryExcelQuery(IUser viewer) : base(viewer)
        {

        }

        public IEnumerable<MoveStatementHistoryByMemberModel> Data { get; set; }
    }
}