namespace Alpha.Reports.Services
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface RiskAnalysisIPMatchingService
    {
        IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl : RiskAnalysisIPMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RiskAnalysisIPMatchingServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        //public IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param)
        //    => dynamicRepository.Fetch<RiskAnalysisIPMatchingModel>(
        //        new RiskAnalysisIPMatchingCriteria(
        //            param.CustId,
        //            param.WinlostDate,
        //            param.MoveBetsDay,
        //            param.MemberCount));

        public IEnumerable<RiskAnalysisIPMatchingModel> GetRiskAnalysisIPMatchingModel(RiskAnalysisIPMatchingParam param)
        {
            return new List<RiskAnalysisIPMatchingModel>()
            {
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                 new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                 new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                 new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                 new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                 new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.2", 6, 17, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.3", 9, 18, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("192.168.1.4", 55, 29, 32, 55, 66, 66),
                new RiskAnalysisIPMatchingModel("127.0.0.1", 5, 15, 32, 55, 66, 66)
            };
        }
    }
}
