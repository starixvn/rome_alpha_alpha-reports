namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using Alpha.Reports.Features.BonusDetail.Params;

    internal class GetSuperBonusCriteria : GetBonusDetailCriteria
    {
        public GetSuperBonusCriteria(BonusDetailParam param) : base(param)
        {
            SuperId = param.ViewerId;
        }

        public int SuperId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Super";

        public override bool IsValid()
            => base.IsValid() && SuperId > 0;
    }
}
