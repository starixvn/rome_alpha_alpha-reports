namespace Alpha.Reports.Features.SuperWinLossDetailByPT
{
    using Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SuperPTCriteria : CriteriaBase
    {
        public SuperPTCriteria(string superId)
        {
            SuperId = superId;
            SettingStatus = SiteSettings.SettingStatus;
        }

        [SpParam(SqlParamName = "Supers")]
        public string SuperId { get; }

        public byte SettingStatus { get; }

        public override string GetSettingKey() => "EAS_GetPTProductBySuper";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(SuperId);
    }
}