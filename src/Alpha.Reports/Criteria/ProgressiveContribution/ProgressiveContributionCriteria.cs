namespace Alpha.Reports.Criteria.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Models;

    public class ProgressiveContributionCriteria : ReportProductCriteria
    {
        public ProgressiveContributionCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            IEnumerable<int> productIds,
            UserLevel reportLevel)
            : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
            ReportLevel = reportLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }
    }
}