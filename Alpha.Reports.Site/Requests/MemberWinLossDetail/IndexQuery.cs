namespace Alpha.Reports.Site.Requests.MemberWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using ViewModels.MemberWinLossDetail;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        public IndexQuery(
            IEnumerable<Product> products,
            Dictionary<string, Func<int, bool>> productPermissions)
        {
            Products = products;
            ProductPermissions = productPermissions;
        }

        public IEnumerable<Product> Products { get; }

        public Dictionary<string, Func<int, bool>> ProductPermissions { get; }
    }
}