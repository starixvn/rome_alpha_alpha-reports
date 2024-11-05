namespace Alpha.Reports.Params
{
    using Models;

    public class OutstandingParam
    {
        public OutstandingParam(
            UserLevel userLevel,
            UserLevel custLevel,
            int custId,
            string productIds,
            bool autoFollowBetIsDummyAgent,
            byte levelType)
        {
            UserLevel = userLevel;
            CustLevel = custLevel;
            CustId = custId;
            ProductIds = productIds;
            AutoFollowBetIsDummyAgent = autoFollowBetIsDummyAgent;
            LevelType = levelType;
        }

        public UserLevel UserLevel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public string ProductIds { get; }

        public bool AutoFollowBetIsDummyAgent { get; }

        public byte LevelType { get; }
    }
}
