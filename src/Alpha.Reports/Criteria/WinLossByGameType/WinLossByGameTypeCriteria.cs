namespace Alpha.Reports.Criteria.WinLossByGameType
{
    using System;
    using Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class WinLossByGameTypeCriteria : CriteriaBase
    {
        public WinLossByGameTypeCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            UserLevel custLevel)
        {
            CustId = custId;
            FromDate = fromDate.GetBeginningOfDate();
            ToDate = toDate.GetBeginningOfDate();
            BalanceUpDay = balanceUpDay;
            CustLevel = custLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

        /// <summary>
        /// remove MoveBetsDay and ProductIds
        /// </summary>
        public DateTime MoveBetsDay => new DateTime(2017, 1, 15);

        public string ProductIds => "1,2,3,4,5,6,8,12,15,21,22";

        public override string GetSettingKey()
        {
            if (CustLevel.IsAgentLevel())
            {
                return "Age_Rpt_WinLoss_Member";
            }

            if (CustLevel.IsMasterLevel())
            {
                return "Age_Rpt_WinLoss_Agent";
            }

            return "Age_Rpt_WinLoss_Master";
        }

        ////public override string GetSettingKey()
        ////{
        ////    if (CustLevel.IsAgentLevel())
        ////    {
        ////        return "Age_Rpt_WinLossByBettype_Ag";
        ////    }

        ////    if (CustLevel.IsMasterLevel())
        ////    {
        ////        return "Age_Rpt_WinLossByBettype_MAg";
        ////    }

        ////    return "Age_Rpt_WinLossByBettype_SAg";
        ////}

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;

        public bool IsNotValid() => !IsValid();
    }
}
