namespace Alpha.Reports.Params.Consolidation
{
    using System;

    public class GetConslidationReportProductsParam
    {
        public GetConslidationReportProductsParam(int custId, string site, int currencyId, DateTime minDate)
        {
            Site = site;
            CurrencyId = currencyId;
            CustId = custId;
            MinDate = minDate;
        }

        public int CustId { get; }

        public string Site { get; }

        public int CurrencyId { get; }

        public DateTime MinDate { get; }
    }
}
