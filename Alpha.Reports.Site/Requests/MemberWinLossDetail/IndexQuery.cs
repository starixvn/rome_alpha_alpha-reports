namespace Alpha.Reports.Site.Requests.MemberWinLossDetail
{
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.MemberWinLossDetail;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        private IUserSession userSession;

        public IndexQuery(IEnumerable<Product> products, IUserSession userSession)
        {
            this.userSession = userSession;
            Products = products;
        }

        public IEnumerable<Product> Products { get; }

        public string Language => userSession.Language;
    }
}