namespace Alpha.Reports.Site.Requests
{
    using System;
    using MediatR;
    using Reports.Models;
    using Utils;
    using ViewModels;

    public class LeaguesByCustomerQuery : ReportBaseQuery, IRequest<LeaguesByCustomerViewModel>
    {
        public LeaguesByCustomerQuery(IUser viewer) : base(viewer)
        {
            CustId = viewer.Id;
            ToDate = SystemTime.Today;
            FromDate = SystemTime.Today;
            LanguageKey = viewer.LanguageKey;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Products { get; set; }

        public string LanguageKey { get; }
    }
}