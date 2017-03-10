namespace Alpha.Reports.Params
{
    using System;

    public class PlayForFunParam
    {
        public PlayForFunParam(int custId, DateTime fromDate, DateTime toDate, int productId, int statusId)
        {
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            ProductId = productId;
            StatusId = statusId;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ProductId { get; }

        public int StatusId { get; }
    }
}
