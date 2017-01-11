namespace Alpha.Reports.Models.Outstanding
{
    using System.Collections.Generic;

    public class OutstandingData
    {
        public int CustId { get; private set; }

        public string CustName { get; private set; }

        public decimal MemberOutstanding { get; private set; }

        public decimal AgentOutstanding { get; private set; }

        public decimal MasterOutstanding { get; private set; }

        public decimal SuperOutstanding { get; private set; }

        public static OutstandingData GetAccumulatedData(IEnumerable<OutstandingData> outstandings)
        {
            var accumulatedData = new OutstandingData();

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
        {
            if (level.IsMemberLevel())
            {
                return MemberOutstanding;
            }

            if (level.IsAgentLevel())
            {
                return AgentOutstanding;
            }

            if (level.IsMasterLevel())
            {
                return MasterOutstanding;
            }

            return SuperOutstanding;
        }
    }
}