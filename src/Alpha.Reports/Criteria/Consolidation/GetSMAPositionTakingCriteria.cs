namespace Alpha.Reports.Criteria.Consolidation
{
    using Alpha.Reports.Configurations;
    using Fanex.Data.Repository;

    public class GetSMAPositionTakingCriteria : CriteriaBase
    {
        public GetSMAPositionTakingCriteria(string custIds)
        {
            CustIds = custIds;
            SettingStatus = SiteSettings.SettingStatus;
        }

        public string CustIds { get; }

        public byte SettingStatus { get; }

        public override string GetSettingKey()
            => "EAS_GetPTProductBySMA";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustIds);
    }
}
