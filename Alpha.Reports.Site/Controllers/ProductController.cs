namespace Alpha.Reports.Site.Controllers
{
    using System.Collections.Generic;
    using MediatR;
    using Requests;
    using ViewModels;

    public class ProductController : BaseController
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IEnumerable<ProductViewModel> GetAllProducts(GetAllProductsQuery message)
        {
            var products = mediator.Send(message);

            return products;
        }
    }
}