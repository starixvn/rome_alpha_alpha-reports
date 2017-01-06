namespace Alpha.Reports.Models.Outstanding
{
    public class OutstandingModel
    {
        public string CustName { get; set; }

        public int CustId { get; set; }

        public decimal MemberOutstanding { get; set; }

        public decimal SuperOutstanding { get; set; }

        public decimal MasterOutstanding { get; set; }

        public decimal AgentOutstanding { get; set; }
    }
}
