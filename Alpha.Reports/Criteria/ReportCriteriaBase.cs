namespace Alpha.Reports.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public abstract class ReportCriteriaBase : CriteriaBase
    {
        protected ReportCriteriaBase(int custId, int pageIndex, int pageSize, DateTime fromDate, DateTime toDate)
        {
            CustId = custId;
            PageIndex = pageIndex;
            PageSize = pageSize;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int CustId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; set; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override bool IsValid()
        {
            return CustId > 0
                && FromDate <= ToDate
                && PageIndex >= 0;
        }

        public bool IsNotValid() => !IsValid();
    }
}
