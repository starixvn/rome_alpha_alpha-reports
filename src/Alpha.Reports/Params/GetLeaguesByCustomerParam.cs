namespace Alpha.Reports.Params
{
    using System;
    using Utils;

    public class GetLeaguesByCustomerParam
    {
        public GetLeaguesByCustomerParam(
            int custId,
            string products,
            DateTime fromDate,
            DateTime toDate,
            string language)
        {
            CustId = custId;
            ProductIds = products;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            Language = language;
        }

        public int CustId { get; }

        public string ProductIds { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Language { get; }
    }
}
