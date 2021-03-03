namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using Alpha.Reports.Features.BonusDetail.Params;

    internal class GetMasterBonusCriteria : GetBonusDetailCriteria
    {
        public GetMasterBonusCriteria(BonusDetailParam param) : base(param)
        {
            MasterId = param.ViewerId;
        }

        public int MasterId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Master";

        public override bool IsValid()
            => base.IsValid() && MasterId > 0;
    }
}
