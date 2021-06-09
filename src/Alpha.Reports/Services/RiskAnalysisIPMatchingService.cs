namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Alpha.Reports.Criteria.RiskAnalysisIPMatching;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;

    public interface RiskAnalysisIPMatchingService
    {
        IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl : RiskAnalysisIPMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;

        public RiskAnalysisIPMatchingServiceImpl(
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
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

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return dynamicRepository.Fetch<RiskAnalysisIPMatchingModel>(criteria);
        }
    }
}
