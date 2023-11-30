namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
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

        IEnumerable<Product> GetProductsForCustomer(string site, int currencyId);
    }

    public class MatchWinLossDetailServiceImpl : MatchWinLossDetailService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly MatchWinLossDetailDataAccess dataAccess;
        private readonly IPartialUMService partialUMService;

        public MatchWinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            MatchWinLossDetailDataAccess dataAccess,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
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

            var validUserSelectedProductIds = productService
                .FilterProductIds(param.Site, param.CurrencyId, param.UserSelectedProductIds);
            
            var criteria = new MatchWinLossDetailCriteria(
                param.CustId,
                param.FromDate.GetGreaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds == null ? string.Empty : validUserSelectedProductIds.JoinWithComma(),
                param.ReportLevel,
                param.UserSelectedLeagueIds);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var winLossData = dataAccess.GetData(criteria);

            return winLossData;
        }

        public IEnumerable<Product> GetProductsForCustomer(string site, int currencyId)
        {
            var productsForMatchWinLossDetail = dynamicRepository.Fetch<int>(new ProductsForMatchWinLossDetailCriteria());

            return productService.FilterProducts(site, currencyId, productsForMatchWinLossDetail);
        }
    }
}