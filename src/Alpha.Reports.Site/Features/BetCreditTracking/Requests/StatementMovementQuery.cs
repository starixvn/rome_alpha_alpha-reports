namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.ViewModels;
    using Alpha.Reports.Site.Requests;
    using MediatR;
    using System.Web.Mvc;

    public class StatementMovementQuery : FromBalanceUpDateDateRangeQuery, IAsyncRequest<StatementMovementViewModel>
    {
        public StatementMovementQuery(IUser viewer) : base(viewer)
        {
        }

        [AllowHtml]
        public string MemberUsername { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }
    }
}