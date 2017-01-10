namespace Alpha.Reports.Models.Outstanding
{
    using System.Collections.Generic;
  public class OutstandingData
{
        public int CustId { get; set; }



        public string CustName { get; set; }

        public decimal MemberOutstanding { get; set; }

        public decimal AgentOutstanding { get; set; }

        public decimal MasterOutstanding { get; set; }

        public decimal SuperOutstanding { get; set; }

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
    }
}