namespace Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;
    using System;

    public class MoveStatementHistoryCriteria : CriteriaBase
    {
        public MoveStatementHistoryCriteria(MoveStatementHistoryParam param)
        {
            Username = param.Username;
            FromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo);
            ToDate = param.ToDate;
            LoggedId = param.ViewerId;
            PageIndex = param.PageIndex;
            PageSize = param.PageSize;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int LoggedId { get; }

        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Age_Rpt_StatementMovement";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
