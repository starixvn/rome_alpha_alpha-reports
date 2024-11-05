using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Reports.Criteria.WinLoss
{
    public class WinLossMasterCriteria : WinLossAgentCriteria
    {
        public WinLossMasterCriteria(
           int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            string productDetails,
            byte levelType)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, levelType)
        {
        }

        public override string GetSettingKey() => "Age_Rpt_WinLoss_Master";
    }
}
