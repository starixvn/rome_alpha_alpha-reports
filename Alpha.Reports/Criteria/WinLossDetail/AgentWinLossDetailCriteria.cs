namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;

    public class AgentWinLossDetailCriteria : BaseWinLossDetailCriteria
    {
        public AgentWinLossDetailCriteria(
               int custId,
               DateTime fromDate,
               DateTime toDate,
               DateTime balanceUpDate,
               string productIds,
               int pageSize,
               int pageIndex) : base(custId, fromDate, toDate, balanceUpDate, productIds, pageSize, pageIndex)
        {
        }

        [SpParam(Ignored = true)]
        public bool IsNewView { get; }

        public override string GetSettingKey() => IsNewView ? "Age_Rpt_WinLossDetail_Agent_New" : "Age_Rpt_WinLossDetail_Agent";
    }
}
