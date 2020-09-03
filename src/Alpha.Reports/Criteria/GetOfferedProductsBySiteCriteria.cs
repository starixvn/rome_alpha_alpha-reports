namespace Alpha.Reports.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetOfferedProductsBySiteCriteria : CriteriaBase
    {
        public GetOfferedProductsBySiteCriteria(int custId, string site, int currencyId)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
        }

        public int CustId { get; }

        public string Site { get; }

        [SpParam(SqlParamName = "Currency")]
        public int CurrencyId { get; }

        public override string GetSettingKey() => "Acc_CustSetting_CheckPGSoftEnable";

        public override bool IsValid() => !string.IsNullOrEmpty(Site) && CurrencyId > 0;
    }
}