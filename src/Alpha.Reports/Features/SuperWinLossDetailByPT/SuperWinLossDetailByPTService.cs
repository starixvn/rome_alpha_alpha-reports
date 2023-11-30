namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.SuperWinLossDetailByPT;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Criteria;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Params;
    using Fanex.Data.Repository;

    public interface SuperWinLossDetailByPTService
    {
        IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param);

        IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param);
    }

    public class SuperWinLossDetailByPTServiceImpl : SuperWinLossDetailByPTService
    {
        private readonly IDynamicRepository repository;
        private readonly ReportDateService reportDateService;
        private readonly IPartialUMService partialUMService;

        public SuperWinLossDetailByPTServiceImpl(
            IDynamicRepository repository,
            ReportDateService reportDateService,
            IPartialUMService partialUMService)
        {
            this.repository = repository;
            this.reportDateService = reportDateService;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param)
            => repository.Fetch<SuperPTModel>(new SuperPTCriteria(param.SuperId));

        public IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param)
        {
            var criteria = new SuperWinLossDetailByPTCriteria(
                param.CustId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds == null ? string.Empty : param.UserSelectedProductIds.JoinWithComma(),
                param.ProductDetailsXML);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return repository.Fetch<SuperWinLossDetailByProductModel>(criteria);
        }
    }
}