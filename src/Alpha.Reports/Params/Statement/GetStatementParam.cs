namespace Alpha.Reports.Params.Statement
{
    using System;

    public class GetStatementParam
    {
        public GetStatementParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            LimitedDateToShowInfo = limitedDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}
