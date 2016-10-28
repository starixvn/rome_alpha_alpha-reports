namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections.Generic;

    public class MasterWinLossDetailCriteria : BaseWinLossDetailCriteria
    {
        public MasterWinLossDetailCriteria(
               int custId,
               DateTime fromDate,
               DateTime toDate,
               DateTime balanceUpDate,
               string productIds,
               int pageSize,
               int pageIndex) : base(custId, fromDate, toDate, balanceUpDate, productIds, pageSize, pageIndex)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_WinLossDetail_Master";
    }
}
