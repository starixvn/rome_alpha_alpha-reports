namespace Alpha.Reports.Site.Features.BetCreditTracking
{
    using System;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using MediatR;

    public class MoveStatementHistoryHandler : 
        MoveStatementHistoryBaseHandler, 
        IRequestHandler<MoveStatementHistoryQuery, MoveStatementHistoryViewModel>
    {
        public MoveStatementHistoryHandler(MoveStatementHistoryService reportService) : base(reportService)
        {
        }

        public MoveStatementHistoryViewModel Handle(MoveStatementHistoryQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var moveStatementHistoryParam = CreateParameter(message);
            var result = reportService.GetMoveStatementHistory(moveStatementHistoryParam);

            if (result.ErrorCode != 0)
            {
                return new MoveStatementHistoryViewModel(null, message, true);
            }

            return new MoveStatementHistoryViewModel(result.Data, message, totalRows: result.TotalRows);
        }
    }
}