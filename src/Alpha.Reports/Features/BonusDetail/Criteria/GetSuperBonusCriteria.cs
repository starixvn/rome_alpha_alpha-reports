namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;

    internal class GetSuperBonusCriteria : GetBonusDetailCriteria
    {
        public GetSuperBonusCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            byte? statusId,
            bool? isComplained,
            int pageIndex,
            int pageSize,
            int superId) : base(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
        {
            SuperId = superId;
        }

        public int SuperId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Super";

        public override bool IsValid()
            => base.IsValid() && SuperId > 0;
    }
}
