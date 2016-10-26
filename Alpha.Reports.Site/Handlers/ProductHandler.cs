namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using Params;
    using Reports.AppResources;
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
            var visibleProductSetting = productService.GetVisibleProducts(new GetVisibleProductsParam(message.ProductSetting, message.ProductPermissions));

            return VisibleProducts(visibleProductSetting);
        }

        public IEnumerable<ProductViewModel> VisibleProducts(IEnumerable<Product> visibleProducts)
        {
            var products = new List<ProductViewModel>();

            foreach (var product in visibleProducts)
            {
                var productDisplayText = remotingRef.GetProductName(product.Id);
                products.Add(new ProductViewModel(product.Id, productDisplayText));
            }

            return products;
        }
    }
}