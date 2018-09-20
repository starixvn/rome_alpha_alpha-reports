namespace Alpha.Reports.Criteria.PokerGamesWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Fanex.Data;
    using Models;
    using Params;

    public class PokerGamesWinLossDetailCriteria : PagingReportProductCriteria
    {
        public PokerGamesWinLossDetailCriteria(
            PokerGamesWinLossDetailParam param,
            DateTime balanceUpDay,
            DateTime moveBetsDay)
            : base(
                  param.CustId,
                  param.PageIndex,
                  param.PageSize,
                  param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                  param.ToDate,
                  balanceUpDay,
                  moveBetsDay,
                  new List<int> { ProductIdConstants.PokerGames })
        {
            ReportLevel = param.ReportLevel;
        }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
            ? "Age_Rpt_WinLossDetailPokerGames_Member"
            : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_WinLossDetailPokerGames_Agent"
            : "Age_Rpt_WinLossDetailPokerGames_Master";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate && PageIndex >= 0;
    }
}