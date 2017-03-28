using System;

namespace Alpha.Reports.Params.Statement
{
    public class GetStatementParam
    {
        public GetStatementParam(
            int custId,
            DateTime fromDate,
            DateTime toDate)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}
