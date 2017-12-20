namespace Alpha.Reports.Models.Outstanding
{
    using System.Collections.Generic;

    public class OutstandingModel
    {
        public OutstandingModel()
        {
        }

        internal OutstandingModel(int custId, string custName, decimal memberOutstanding, decimal agentOutstanding, decimal masterOutstanding, decimal superOutstanding)
        {
            CustId = custId;
            CustName = custName;
            MemberOutstanding = memberOutstanding;
            AgentOutstanding = agentOutstanding;
            MasterOutstanding = masterOutstanding;
            SuperOutstanding = superOutstanding;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal MemberOutstanding { get; private set; }

        public decimal AgentOutstanding { get; private set; }

        public decimal MasterOutstanding { get; private set; }

        public decimal SuperOutstanding { get; private set; }

        public static OutstandingModel GetAccumulatedData(IEnumerable<OutstandingModel> outstandings)
        {
            var accumulatedData = new OutstandingModel();

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
    }
}