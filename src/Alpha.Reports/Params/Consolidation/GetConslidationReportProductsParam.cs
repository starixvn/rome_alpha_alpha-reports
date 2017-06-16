using System;

namespace Alpha.Reports.Params.Consolidation
{
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
