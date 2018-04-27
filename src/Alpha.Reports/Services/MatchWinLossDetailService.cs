namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        IEnumerable<Product> GetProductsForCustomer(int custId);
    }

    public class MatchWinLossDetailServiceImpl : MatchWinLossDetailService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly MatchWinLossDetailDataAccess dataAccess;

        public MatchWinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            MatchWinLossDetailDataAccess dataAccess,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<MatchWinLossDetailModel> GetMatchWinLossDetail(MatchWinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var productsForMatchWinLossDetail = dynamicRepository.Fetch<int>(new ProductsForMatchWinLossDetailCriteria());

            var validUserSelectedProductIds = productService
                .FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds)
                .Where(product => productsForMatchWinLossDetail.Contains(product));

            var criteria = new MatchWinLossDetailCriteria(
                param.CustId,
                param.FromDate.GetLaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                validUserSelectedProductIds,
                param.ReportLevel,
                param.UserSelectedLeagueIds);

            var winLossData = dataAccess.GetData(criteria);

            return winLossData;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId)
        {
            var productsForMatchWinLossDetail = dynamicRepository.Fetch<int>(new ProductsForMatchWinLossDetailCriteria());

            return productService.FilterProductsByCustomer(custId, productsForMatchWinLossDetail);
        }

    }
}
