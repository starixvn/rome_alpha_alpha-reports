namespace Alpha.Reports.Site.Handlers
{
    using System.Collections.Generic;
    using Criteria;
    using MediatR;
    using Models;
    using Requests;
    using Services;
    using ViewModels;

    public class ProductHandler :
          IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>>
    {
        private RemotingRefWrapper remotingRef;
        private ProductService productService;

        public ProductHandler(RemotingRefWrapper remotingRef, ProductService productService)
        {
            this.remotingRef = remotingRef;
            this.productService = productService;
        }

        public IEnumerable<ProductViewModel> Handle(GetAllProductsQuery message)
        {
            var products = productService.GetProductsByCustomer(new GetProductByCustomerCriteria(message.CustId));

            return TransformProducts(products);
        }

        public IEnumerable<ProductViewModel> TransformProducts(IEnumerable<Product> products)
        {
            var productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var productDisplayText = remotingRef.GetProductName(product.Id);
                productViewModels.Add(new ProductViewModel(product.Id, productDisplayText));
            }

            return productViewModels;
        }
    }
}