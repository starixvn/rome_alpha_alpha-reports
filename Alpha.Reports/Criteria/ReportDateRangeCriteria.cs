namespace Alpha.Reports.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ReportDateRangeCriteria : CriteriaBase
    {
        public ReportDateRangeCriteria(byte reportType)
        {
            ReportType = reportType;
        }

        [SpParam(SqlParamName = "rpttypeid")]
        public byte ReportType { get; set; }

        public override string GetSettingKey() => "Age_Rpt_GetTimeRange";

        public override bool IsValid() => ReportType >= 0;
    }
}
