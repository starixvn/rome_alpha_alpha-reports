namespace Alpha.Reports.Criteria.Consolidation
{
    using Alpha.Reports.Configurations;
    using Fanex.Data.Repository;

    public class GetConsolidationReportProductsCriteria : CriteriaBase
    {
        public GetConsolidationReportProductsCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public byte SettingStatus => SiteSettings.SettingStatus;

        public override string GetSettingKey()
            => "Age_Rpt_ConsolidatedGroupProductSel";

        public override bool IsValid()
            => CustId > 0;
    }
}
