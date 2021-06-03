namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using Criteria.MatchWinLossDetail;
    using DataAccesses;
    using Extensions;
    using Fanex.Data.Repository;
    using Models;
    using Models.MatchWinLossDetail;
    using Params;

    public interface MatchWinLossDetailService
    {
        IEnumerable<MatchWinLossDetailModel> GetMatchWinLossDetail(MatchWinLossDetailParam param);

        IEnumerable<Product> GetProductsForCustomer(int custId, string site, int currencyId);
    }

    public class MatchWinLossDetailServiceImpl : MatchWinLossDetailService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly MatchWinLossDetailDataAccess dataAccess;
        private readonly PartialUMService partialUMService;

        public MatchWinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            MatchWinLossDetailDataAccess dataAccess,
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<MatchWinLossDetailModel> GetMatchWinLossDetail(MatchWinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var productsForMatchWinLossDetail = dynamicRepository.Fetch<int>(new ProductsForMatchWinLossDetailCriteria());

            var validUserSelectedProductIds = productService
                .FilterProductIdsByCustomer(param.CustId, param.Site, param.CurrencyId, param.UserSelectedProductIds)
                .Where(product => productsForMatchWinLossDetail.Contains(product));

            var criteria = new MatchWinLossDetailCriteria(
                param.CustId,
                param.FromDate.GetGreaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel,
                param.UserSelectedLeagueIds);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var winLossData = dataAccess.GetData(criteria);

            return winLossData;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId, string site, int currencyId)
        {
            var productsForMatchWinLossDetail = dynamicRepository.Fetch<int>(new ProductsForMatchWinLossDetailCriteria());

            return productService.FilterProductsByCustomer(custId, site, currencyId, productsForMatchWinLossDetail);
        }

    }
}
