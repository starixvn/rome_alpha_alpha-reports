namespace Alpha.Reports.Features.BonusDetail.Params
{
    using System;
    using Alpha.Reports.Models;

    public class BonusDetailParam
    {
        public BonusDetailParam(
            int viewerId,
            UserLevel viewerLevel,
            string username,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            byte? statusId,
            bool? isComplained,
            int pageIndex,
            int pageSize)
        {
            ViewerId = viewerId;
            ViewerLevel = viewerLevel;
            Username = username;
            FromDate = fromDate;
            ToDate = toDate;
            LimitedDateToShowInfo = limitedDate;
            StatusId = statusId;
            IsComplained = isComplained;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int ViewerId { get; }

        public UserLevel ViewerLevel { get; }

        public string Username { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public byte? StatusId { get; }

        public bool? IsComplained { get; }

        public int PageIndex { get; }

        public int PageSize { get; }
    }
}
