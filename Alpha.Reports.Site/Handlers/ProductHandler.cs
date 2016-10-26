namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using MediatR;
    using Models;
    using Params;
    using RemotingRef;
    using Requests;
    using Services;
    using ViewModels;

    public class ProductHandler :
          IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>>
    {
        private CacheRefService cacheRefService;

        private ProductService productService;

        public ProductHandler(CacheRefService cacheRefService, ProductService productService)
        {
            this.cacheRefService = cacheRefService;
            this.productService = productService;
        }

        public IEnumerable<ProductViewModel> Handle(GetAllProductsQuery message)
        {
            var products = productService.GetProductsByCustomer(new GetProductByCustomerParam(message.CustId));

            return TransformProducts(products);
        }

        public IEnumerable<ProductViewModel> TransformProducts(IEnumerable<Product> products)
        {
            var productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var productDisplayText = cacheRefService.GetProductName(product.Id);
                productViewModels.Add(new ProductViewModel(product.Id, productDisplayText));
            }

            return productViewModels;
        }
    }
}