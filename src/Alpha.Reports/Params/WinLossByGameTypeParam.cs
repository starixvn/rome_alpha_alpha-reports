namespace Alpha.Reports.Params
{
    using System;
    using Models;
    using Utils;

    public class WinLossByGameTypeParam
    {
        public WinLossByGameTypeParam(
            int custId,
            int productId,
            DateTime fromDate,
            DateTime toDate,
            UserLevel custLevel)
        {
            CustId = custId;
            ProductId = productId;
            CustLevel = custLevel;
            if (fromDate == DateTime.MinValue)
            {
                FromDate = SystemTime.Today;
            }
            else
            {
                FromDate = fromDate;
            }

            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
        }

        public int CustId { get; }

        public int ProductId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}