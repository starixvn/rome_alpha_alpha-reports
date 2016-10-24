namespace Alpha.Reports.Site.Requests
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using ViewModels;

    public class VisibleProductsQuery : IRequest<IEnumerable<ProductViewModel>>
    {
        public VisibleProductsQuery(IEnumerable<Product> products, Dictionary<string, Func<int, bool>> productPermissions)
        {
            ProductSetting = products;
            ProductPermissions = productPermissions;
        }

        public IEnumerable<Product> ProductSetting { get; }

        public Dictionary<string, Func<int, bool>> ProductPermissions { get; }
    }
}