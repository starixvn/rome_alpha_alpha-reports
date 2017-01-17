namespace Alpha.Reports.Site.Requests
{
    using System;
    using MediatR;
    using Reports.Models;
    using Utils;
    using ViewModels;

    public class LeaguesByCustomerQuery : IRequest<LeaguesByCustomerViewModel>
    {
        public LeaguesByCustomerQuery()
        {
        }

        public LeaguesByCustomerQuery(IUser viewer)
        {
            CustId = viewer.Id;
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
            Language = viewer.Language;
        }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Products { get; set; }

        public string Language { get; }
    }
}