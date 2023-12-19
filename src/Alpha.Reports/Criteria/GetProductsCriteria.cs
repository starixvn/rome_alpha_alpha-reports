namespace Alpha.Reports.Criteria
{
    using Alpha.Reports.Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetProductsCriteria : CriteriaBase
    {
        public GetProductsCriteria(string site, int currencyId)
        {
            Site = site;
            CurrencyId = currencyId;
        }
        public string Site { get; }

        [SpParam(SqlParamName = "Currency")]
        public int CurrencyId { get; }

        public byte SettingStatus => SiteSettings.SettingStatus;

        public override string GetSettingKey() => "Age_GetOfferedProductBySiteCurrency";

        public override bool IsValid() =>!string.IsNullOrEmpty(Site) && CurrencyId > 0;
    }
}