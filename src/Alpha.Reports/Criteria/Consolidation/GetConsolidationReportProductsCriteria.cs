namespace Alpha.Reports.Criteria.Consolidation
{
    using Alpha.Reports.Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetConsolidationReportProductsCriteria : CriteriaBase
    {
        public GetConsolidationReportProductsCriteria(int custId, string subfix)
        {
            CustId = custId;
            Subfix = subfix;
        }

        public int CustId { get; }

        public byte SettingStatus => SiteSettings.SettingStatus;

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey()
            => $"Age_Rpt_ConsolidatedGroupProductSel{Subfix}";

        public override bool IsValid()
            => CustId > 0;
    }
}
