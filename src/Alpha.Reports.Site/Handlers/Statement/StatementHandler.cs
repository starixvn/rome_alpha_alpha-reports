namespace Alpha.Reports.Site.Handlers.Statement
{
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.ViewModels.Statement;
    using MediatR;

    public class StatementHandler :
        IRequestHandler<SMAStatementQuery, SMAStatementViewModel>,
        IRequestHandler<StatementDetailsQuery, StatementDetailViewModel>,
        IRequestHandler<MemberStatementQuery, MemberStatementViewModel>
    {
        private readonly StatementService reportService;

        public StatementHandler(StatementService reportService)
        {
            this.reportService = reportService;
        }

        public SMAStatementViewModel Handle(SMAStatementQuery message)
        {
            var models = reportService.GetStatements(new GetStatementParam(message.CustId, message.FromDate, message.ToDate, message.LimitedDateToShowInfo));

            return new SMAStatementViewModel(message, models);
        }

        public StatementDetailViewModel Handle(StatementDetailsQuery message)
        {
            var models = reportService.StatementDetails(new StatementDetailsParam(message.CustId, message.TransferDate));

            return new StatementDetailViewModel(message, models);
        }

        public MemberStatementViewModel Handle(MemberStatementQuery message)
        {
            var models = reportService.GetStatements(new GetStatementParam(message.CustId, message.FromDate, message.ToDate, message.LimitedDateToShowInfo));

            return new MemberStatementViewModel(message, models);
        }
    }
}