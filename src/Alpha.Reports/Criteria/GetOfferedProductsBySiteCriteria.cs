namespace Alpha.Reports.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetOfferedProductsBySiteCriteria : CriteriaBase
    {
        public string Site { get; set; }

        [SpParam(SqlParamName = "Currency")]
        public int CurrencyId { get; set; }

        public override string GetSettingKey() => "Acc_CustSetting_CheckPGSoftEnable";

        public override bool IsValid() => !string.IsNullOrEmpty(Site) && CurrencyId > 0;
    }
}