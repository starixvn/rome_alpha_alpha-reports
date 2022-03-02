namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Fanex.Data.Repository;
    using System;

    public class BetCreditHistoryCriteria : CriteriaBase
    {
        public BetCreditHistoryCriteria(BetCreditHistoryParam param)
        {
            Username = param.Member;
            FromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo);
            ToDate = param.ToDate;
            RefNo = param.RefNo;
            LoggedId = param.ViewerId;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public long? RefNo { get; }

        public int LoggedId { get; }

        public int ErrorCode { get; set; }

        public string ErrorMess { get; set; }

        public override string GetSettingKey() => "Age_Rpt_BetCreditHistory";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
