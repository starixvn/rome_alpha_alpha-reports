namespace Alpha.Reports.Params.Consolidation
{
    using System;

    public class GetConslidationReportProductsParam
    {
        public GetConslidationReportProductsParam(int custId, int currencyId, DateTime minDate)
        {
            CurrencyId = currencyId;
            CustId = custId;
            MinDate = minDate;
        }

        public int CustId { get; }

        public int CurrencyId { get; }

        public DateTime MinDate { get; }
    }
}
