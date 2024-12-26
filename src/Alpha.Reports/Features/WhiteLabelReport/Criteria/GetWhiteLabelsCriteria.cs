namespace Alpha.Reports.Features.WhiteLabelReport.Criteria
{
    using Fanex.Data.Repository;

    internal class GetWhiteLabelsCriteria : CriteriaBase
    {
        public GetWhiteLabelsCriteria(int custId)
            => CustId = custId;

        public int CustId { get; }

        public override string GetSettingKey() => "Age_WhiteLabelListSel";

        public override bool IsValid() => CustId > 0;
    }
}
