namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class BetChoiceMatchingExcelQuery : BetChoiceMatchingJsonQuery, IRequest<ExcelFileViewModel>
    {
        public BetChoiceMatchingExcelQuery(IUser viewer, ReportDateService reportDateService) : base(viewer, reportDateService)
        {
        }

        public string Username { get; set; }

        public decimal? Stake { get; set; }

        public MemberWinLossOperators MemberWinLossOperator { get; set; }

        public decimal? MemberWinLossValue { get; set; }
    }
}