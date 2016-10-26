namespace Alpha.Reports.Site.Requests
{
    using System.Collections.Generic;
    using MediatR;
    using ViewModels;

    public class GetAllProductsQuery : IRequest<IEnumerable<ProductViewModel>>
    {
        public GetAllProductsQuery(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}