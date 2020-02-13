namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class BetChoiceMatchingExcelQuery : BetChoiceMatchingQuery, IRequest<ExcelFileViewModel>
    {
        public BetChoiceMatchingExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public string Username { get; set; }

        public decimal? Stake { get; set; }

        public MemberWinLossOperators MemberWinLossOperator { get; set; }

        public decimal? MemberWinLossValue { get; set; }
    }
}