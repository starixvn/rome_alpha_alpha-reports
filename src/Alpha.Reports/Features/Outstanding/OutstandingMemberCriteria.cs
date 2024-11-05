namespace Alpha.Reports.Criteria.Outstanding
{
    using Fanex.Data.Repository;

    public class OutstandingMemberCriteria : CriteriaBase
    {
        public OutstandingMemberCriteria(int custId, string productIds)
        {
            CustId = custId;
            ProductIds = productIds;
        }

        public int CustId { get; }

        public string ProductIds { get; }

        public override string GetSettingKey() => "Age_Rpt_Outstanding_Member";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(ProductIds);
    }
}
