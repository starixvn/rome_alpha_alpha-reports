namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLossDetail;

    public class MemberWinLossDetailQuery : IRequest<IndexViewModel>
    {
        private IUserSession userSession;

        public MemberWinLossDetailQuery(IEnumerable<Product> products, IUserSession userSession)
        {
            this.userSession = userSession;
            Products = products;
        }

        public IEnumerable<Product> Products { get; }

        public string Language => userSession.Language;
    }
}