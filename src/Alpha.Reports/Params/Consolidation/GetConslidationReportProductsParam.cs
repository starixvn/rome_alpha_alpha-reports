namespace Alpha.Reports.Params.Consolidation
{
    using System;

    public class GetConslidationReportProductsParam
    {
        public GetConslidationReportProductsParam(int custId, DateTime minDate)
        {
            CustId = custId;
            MinDate = minDate;
        }

        public int CustId { get; }

        public DateTime MinDate { get; }
    }
}
