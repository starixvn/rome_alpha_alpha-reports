namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using MediatR;
    using Params;
    using Requests;
    using Services;
    using ViewModels;

    public class ProductHandler :
          IRequestHandler<VisibleProductsQuery, IEnumerable<ProductViewModel>>
    {
        private RemotingRefWrapper remotingRef;
        private ProductService productService;

        public ProductHandler(RemotingRefWrapper remotingRef, ProductService productService)
        {
            this.remotingRef = remotingRef;
            this.productService = productService;
        }

        public IEnumerable<ProductViewModel> Handle(VisibleProductsQuery message)
        {
            var products = new List<ProductViewModel>();

            var visibleProductSetting = productService.GetVisibleProducts(new GetVisibleProductsParam(message.ProductSetting, message.ProductPermissions));

            foreach (var product in visibleProductSetting)
            {
                var productDisplayText = remotingRef.GetProductName(product.Id);
                products.Add(new ProductViewModel(product.Id, productDisplayText, product.StoreProcedureParameter));
            }

            return products;
        }
    }
}