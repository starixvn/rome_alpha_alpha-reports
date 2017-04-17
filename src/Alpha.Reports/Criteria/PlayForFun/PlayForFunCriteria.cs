namespace Alpha.Reports.Criteria.PlayForFun
{
    using System;
    using Fanex.Data.Repository;

    public class PlayForFunCriteria : CriteriaBase
    {
        public PlayForFunCriteria(int custId, DateTime fromDate, DateTime toDate, int productId, int statusId)
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

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}
