namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria.PlayForFun;
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface RiskAnalysisIPMatchingService
    {
        IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl
    {
        private readonly IDynamicRepository dynamicRepository;

        public RiskAnalysisIPMatchingServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param)
            => dynamicRepository.Fetch<RiskAnalysisIPMatchingModel>(
                new RiskAnalysisIPMatchingCriteria(
                    param.CustId,
                    param.WinlostDate,
                    param.MoveBetsDay,
                    param.MemberCount));
    }
}
