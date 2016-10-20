namespace Alpha.Reports.Criteria.WinLossDetail
{
    using Fanex.Data;

    public class MasterWinLossDetailCriteria : BaseWinLossDetailCriteria
    {
        [SpParam(Ignored = true)]
        public bool IsNewView { get; }

        public override string GetSettingKey() => IsNewView ? "Age_Rpt_WinLossDetail_MAg_New" : "Age_Rpt_WinLossDetail_SAg";
    }
}
