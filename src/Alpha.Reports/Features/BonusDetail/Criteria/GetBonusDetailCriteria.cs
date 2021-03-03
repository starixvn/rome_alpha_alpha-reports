namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Fanex.Data.Repository;

    public class GetBonusDetailCriteria : CriteriaBase
    {
        protected GetBonusDetailCriteria(BonusDetailParam param)
        {
            Username = param.Username;
            FromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo);
            ToDate = param.ToDate;
            StatusId = param.StatusId;
            IsComplained = param.IsComplained;
            PageIndex = param.PageIndex;
            PageSize = param.PageSize;
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
