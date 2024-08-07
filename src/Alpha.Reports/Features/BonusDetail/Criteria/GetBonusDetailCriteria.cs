namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetBonusDetailCriteria : CriteriaBase
    {
        protected GetBonusDetailCriteria(
            string username,
            DateTime fromDate,
            DateTime toDate,
            byte? statusId,
            bool? isComplained,
            int pageIndex,
            int pageSize)
        {
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            StatusId = statusId;
            IsComplained = isComplained;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte? StatusId { get; }

        public bool? IsComplained { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; set; }

        public override string GetSettingKey()
            => throw new NotImplementedException();

        public override bool IsValid() => FromDate <= ToDate;
    }
}
