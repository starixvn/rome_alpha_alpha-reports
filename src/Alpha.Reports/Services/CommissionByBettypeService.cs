namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.CommissionByBettype;
    using Fanex.Data.Repository;
    using Models;
    using Models.CommissionByBettype;
    using Params;
    using Sunplus.Agent.Model.Definitions;

    public interface CommissionByBettypeService
    {
        IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param);

        IEnumerable<Product> GetProductsInOthers(int custId);        
    }

    public class CommissionByBettypeServiceImpl : CommissionByBettypeService
    {
        private static readonly IEnumerable<int> ProductsInOthers = new List<int> { ProductIdConstants.BullFighting };
        private readonly IDynamicRepository repository;
        private readonly ProductService productService;

        public CommissionByBettypeServiceImpl(IDynamicRepository dynamicRepository, ProductService productService)
        {
            repository = dynamicRepository;
            this.productService = productService;
        }

        public IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param)
        {
            var criteria = new CommissionByBettypeCriteria(param.CustLevel, param.CustId, param.FromDate, param.ToDate, param.MoveBetsDay);

            return repository.Fetch<CommissionByBettypeModel>(criteria);
        }

        public IEnumerable<Product> GetProductsInOthers(int custId)
            => productService.GetProductsForCustomer(custId).Where(product => ProductsInOthers.Contains(product.Id));
    }
}
