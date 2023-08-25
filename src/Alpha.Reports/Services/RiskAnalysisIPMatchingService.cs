namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Criteria.RiskAnalysisIPMatching;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Fanex.Data.Repository;

    public interface RiskAnalysisIPMatchingService
    {
        IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl : RiskAnalysisIPMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public RiskAnalysisIPMatchingServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param)
        {
            var criteria = new RiskAnalysisIPMatchingCriteria(
                param.CustId,
                param.WinlostDate,
                param.MoveBetsDay,
                param.MemberCount);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return dynamicRepository.Fetch<RiskAnalysisIPMatchingModel>(criteria);
        }
    }
}