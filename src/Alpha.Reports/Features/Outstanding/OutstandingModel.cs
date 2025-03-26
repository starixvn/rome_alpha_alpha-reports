namespace Alpha.Reports.Models.Outstanding
{
    using System.Collections.Generic;
    using System.Linq;

    public class OutstandingModel
    {
        public OutstandingModel()
        {
        }

        public OutstandingModel(int custId, string custName, decimal memberOutstanding, decimal agentOutstanding, decimal masterOutstanding, decimal superOutstanding)
        {
            CustId = custId;
            CustName = custName;
            MemberOutstanding = memberOutstanding;
            AgentOutstanding = agentOutstanding;
            MasterOutstanding = masterOutstanding;
            SuperOutstanding = superOutstanding;
        }

        public string DownlineLink { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal MemberOutstanding { get; set; }

        public decimal AgentOutstanding { get; set; }

        public decimal MasterOutstanding { get; set; }

        public decimal SuperOutstanding { get; set; }

        public decimal? ToWin { get; set; }

        public decimal? SuperRisk { get; set; }

        public decimal? MasterRisk { get; set; }

        public decimal? AgentRisk { get; set; }

        public static OutstandingModel GetAccumulatedData(IEnumerable<OutstandingModel> outstandings)
        {
            var accumulatedData = new OutstandingModel
            {
                ToWin = outstandings.Select(i => i.ToWin).Sum(),
                AgentRisk = outstandings.Select(i => i.AgentRisk).Sum(),
                MasterRisk = outstandings.Select(i => i.MasterRisk).Sum(),
                SuperRisk = outstandings.Select(i => i.SuperRisk).Sum()
            };

            foreach (var outstanding in outstandings)
            {
                accumulatedData.MemberOutstanding += outstanding.MemberOutstanding;
                accumulatedData.AgentOutstanding += outstanding.AgentOutstanding;
                accumulatedData.MasterOutstanding += outstanding.MasterOutstanding;
                accumulatedData.SuperOutstanding += outstanding.SuperOutstanding;                
            }

            return accumulatedData;
        }

        public decimal GetValueByLevel(UserLevel level)
            => level.IsMemberLevel()
            ? MemberOutstanding
            : level.IsAgentLevel()
            ? AgentOutstanding
            : level.IsMasterLevel()
            ? MasterOutstanding
            : SuperOutstanding;

        public decimal? GetSMARiskValueByLevel(UserLevel level)
           => level.IsAgentLevel()
           ? AgentRisk
           : level.IsMasterLevel()
           ? MasterRisk
           : SuperRisk;
    }
}