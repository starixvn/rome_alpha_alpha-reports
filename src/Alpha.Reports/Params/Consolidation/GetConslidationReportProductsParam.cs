namespace Alpha.Reports.Params.Consolidation
{
    using System;

    public class GetConslidationReportProductsParam
    {
        public GetConslidationReportProductsParam(int custId, DateTime minDate, string site)
        {
            CustId = custId;
            MinDate = minDate;
            Site = site;
        }

        public int CustId { get; }

        public DateTime MinDate { get; }

        public string Site { get; }
    }
}
