namespace Alpha.Reports.Site.Requests.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class BetChoiceMatchingExcelQuery : BetChoiceMatchingQuery, IRequest<ExcelFileViewModel>
    {
        public BetChoiceMatchingExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public string Username { get; }

        public decimal? Stake { get; }

        public int MemberWinLossOperator { get; }

        public decimal? MemberWinLossValue { get; }
    }
}