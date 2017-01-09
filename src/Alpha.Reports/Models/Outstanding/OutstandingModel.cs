namespace Alpha.Reports.Models.Outstanding
{
    public class OutstandingModel
    {
        public OutstandingModel()
        {
        }

        public OutstandingModel(
            string custName,
            int custId,
            decimal memberOutstanding,
            decimal agentOutstanding,
            decimal masterOutstanding,
            decimal superOutstanding)
        {
            CustName = custName;
            CustId = custId;
            MemberOutstanding = memberOutstanding;
            AgentOutstanding = agentOutstanding;
            MasterOutstanding = masterOutstanding;
            SuperOutstanding = superOutstanding;
        }

        public string CustName { get; }

        public int CustId { get; set; }

        public decimal MemberOutstanding { get; }

        public decimal SuperOutstanding { get; }

        public decimal MasterOutstanding { get; }

        public decimal AgentOutstanding { get; }
    }
}
