namespace Alpha.Reports.Services
{
    using Criteria.SuperWinLossDetailByPT;
    using Fanex.Data.Repository;
    using Models.SuperWinLossDetailByPT;
    using Params.SuperWinLossDetailByPT;
    using System.Collections.Generic;

    public interface SuperWinLossDetailByPTService
    {
        IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param);

        IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param);
    }

    public class SuperWinLossDetailByPTServiceImpl : SuperWinLossDetailByPTService
    {
        private readonly IDynamicRepository repository;
        private readonly ReportDateService reportDateService;

        public SuperWinLossDetailByPTServiceImpl(
            IDynamicRepository repository,
            ReportDateService reportDateService)
        {
            this.repository = repository;
            this.reportDateService = reportDateService;
        }

        public IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param)
            => repository.Fetch<SuperPTModel>(new SuperPTCriteria(param.SuperId));

        public IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param)
            => repository.Fetch<SuperWinLossDetailByProductModel>(
                new SuperWinLossDetailByPTCriteria(
                    param.CustId,
                    param.FromDate,
                    param.ToDate,
                    reportDateService.GetBalanceUpDay(),
                    reportDateService.GetMoveBetsDay(),
                    param.UserSelectedProductIds,
                    param.ProductDetailsXML));
    }
}