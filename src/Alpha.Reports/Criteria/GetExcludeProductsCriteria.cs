namespace Alpha.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetExcludeProductsCriteria : CriteriaBase
    {
        public GetExcludeProductsCriteria(int custId, bool isGroupHeadline)
        {
            CustId = custId;
            IsGroupHeadline = isGroupHeadline;
        }

        public int CustId { get; }

        public bool IsGroupHeadline { get; }

        public override string GetSettingKey()
            => "Age_ExcludeProduct_SiteCurrency_Sel";

        public override bool IsValid() => CustId > 0;
    }
}
