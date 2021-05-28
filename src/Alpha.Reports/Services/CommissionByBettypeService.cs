namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Alpha.Web.Helpers;
    using Criteria.CommissionByBettype;
    using Fanex.Data.Repository;
    using Models;
    using Models.CommissionByBettype;
    using Params;

    public interface CommissionByBettypeService
    {
        IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param);

        IEnumerable<Product> GetProductsInOthers(int custId, string site, int currencyId);
    }

    public class CommissionByBettypeServiceImpl : CommissionByBettypeService
    {
        private readonly IDynamicRepository repository;
        private readonly ProductService productService;
        private readonly PartialUMService partialUMService;

        public CommissionByBettypeServiceImpl(
            IDynamicRepository dynamicRepository,
            ProductService productService,
            PartialUMService partialUMService)
        {
            repository = dynamicRepository;
            this.productService = productService;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<CommissionByBettypeModel> GetAllCommissionByBettypes(CommissionByBettypeParam param)
        {
            var criteria = new CommissionByBettypeCriteria(param.CustLevel, param.CustId, param.FromDate, param.ToDate, param.MoveBetsDay);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return repository.Fetch<CommissionByBettypeModel>(criteria);
        }

        public IEnumerable<Product> GetProductsInOthers(int custId, string site, int currencyId)
            => productService.GetProductsForCustomer(custId, site, currencyId);
    }
}
