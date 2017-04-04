namespace Alpha.Reports.Params
{
    using System;
    using Models;

    public class PlayForFunParam
    {
        public PlayForFunParam(int custId, DateTime fromDate, DateTime toDate, int productId, int statusId, UserLevel custLevel)
        {
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            ProductId = productId;
            StatusId = statusId;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int ProductId { get; }

        public int StatusId { get; }

        public UserLevel CustLevel { get; }
    }
}
