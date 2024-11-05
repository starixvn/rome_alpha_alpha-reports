namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;

    public class WinLossDetailWeekDataParam
    {
        public WinLossDetailWeekDataParam(
            int viewerId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDateToShowInfo,
            IEnumerable<int> userSelectedProductIds,
            string productDetailsXML,
            string username,
            bool isReward,
            byte levelType,
            string site)
        {
            ViewerId = viewerId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            UserSelectedProductIds = userSelectedProductIds;
            ProductDetailsXML = productDetailsXML;
            Username = username;
            IsReward = isReward;
            LevelType = levelType;
            Site = site;
        }

        public int ViewerId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }

        public string ProductDetailsXML { get; }

        public string Username { get; }

        public bool IsReward { get; }

        public byte LevelType { get; }

        public string Site { get; }
    }
}
