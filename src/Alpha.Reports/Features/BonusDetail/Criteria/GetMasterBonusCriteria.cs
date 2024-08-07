namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;

    internal class GetMasterBonusCriteria : GetBonusDetailCriteria
    {
        public GetMasterBonusCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            byte? statusId,
            bool? isComplained,
            int pageIndex,
            int pageSize,
            int masterId) : base(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
        {
            MasterId = masterId;
        }

        public int MasterId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_BonusDetail_Master";

        public override bool IsValid()
            => base.IsValid() && MasterId > 0;
    }
}
