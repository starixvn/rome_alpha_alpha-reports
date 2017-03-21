namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria.MatchWinLossDetail;
    using DataAccesses;
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
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly MatchWinLossDetailDataAccess dataAccess;

        private static readonly IEnumerable<int> supportedProducts = new List<int>
        {
            ProductIdConstants.Sportsbook,
            ProductIdConstants.Racing,
            ProductIdConstants.NumberGame,
            ProductIdConstants.VirtualSports
        };

        public MatchWinLossDetailServiceImpl(
            ReportDateService reportDateService,
            ProductService productService,
            MatchWinLossDetailDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.productService = productService;
            this.dataAccess = dataAccess;
        }

        public IEnumerable<MatchWinLossDetailModel> GetMatchWinLossDetail(MatchWinLossDetailParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var validUserSelectedProductIds = productService
                .FilterProductIdsByCustomer(param.CustId, param.UserSelectedProductIds)
                .Where(product => supportedProducts.Contains(product));

            var criteria = new MatchWinLossDetailCriteria(
                param.CustId,
                param.FromDate,
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
            return productService.FilterProductsByCustomer(custId, supportedProducts);
        }
    }
}
