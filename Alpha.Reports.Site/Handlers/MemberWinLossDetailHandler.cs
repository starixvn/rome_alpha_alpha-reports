namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using Newtonsoft.Json;
    using Reports.Helpers;
    using Requests.MemberWinLossDetail;
    using Services;
    using ViewModels;
    using ViewModels.MemberWinLossDetail;

    public class MemberWinLossDetailHandler :
        IRequestHandler<IndexQuery, IndexViewModel>
    {
        private RemotingRefWrapper remotingRef;

        public MemberWinLossDetailHandler(RemotingRefWrapper remotingRef)
        {
            this.remotingRef = remotingRef;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            return new IndexViewModel
            {
                MinDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now.AddMonths(-2)),
                MaxDate = null,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,

                CurrentDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now),
                Products = JsonConvert.SerializeObject(GetVisibleProducts(message)),

                HasData = true,
                Language = "en"
            };
        }

        private IEnumerable<ProductViewModel> GetVisibleProducts(IndexQuery message)
        {
            var products = new List<ProductViewModel>();
            foreach (var product in message.Products)
            {
                if (!message.ProductPermissions[product.Permission](product.Id))
                {
                    continue;
                }

                var productDisplayText = remotingRef.GetProductName(product.Id);
                products.Add(new ProductViewModel(product.Id, product.Name, product.StoreProcedureParameter));
            }

            return products;
        }
    }
}