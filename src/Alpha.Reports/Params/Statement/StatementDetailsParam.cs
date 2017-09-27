namespace Alpha.Reports.Params.Statement
{
    using System;

    public class StatementDetailsParam
    {
        public StatementDetailsParam(int custId, DateTime transferDate)
        {
            CustId = custId;
            TransferDate = transferDate;
        }

        public int CustId { get; }

        public DateTime TransferDate { get; }
    }
}
