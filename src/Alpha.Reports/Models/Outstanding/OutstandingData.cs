namespace Alpha.Reports.Models.Outstanding
{
    public class OutstandingData
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public decimal MemberOutstanding { get; set; }

        public decimal AgentOutstanding { get; set; }

        public decimal MasterOutstanding { get; set; }

        public decimal SuperOutstanding { get; set; }

        public void Accumulate(OutstandingData outstanding)
        {
            MemberOutstanding += outstanding.MemberOutstanding;
            AgentOutstanding += outstanding.AgentOutstanding;
            MasterOutstanding += outstanding.MasterOutstanding;
            SuperOutstanding += outstanding.SuperOutstanding;
        }
    }
}
