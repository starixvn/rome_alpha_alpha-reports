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
            bool autoFollowBetIsDummyAgent)
        {
            UserLevel = userLevel;
            CustLevel = custLevel;
            CustId = custId;
            ProductIds = productIds;
            AutoFollowBetIsDummyAgent = autoFollowBetIsDummyAgent;
        }

        public UserLevel UserLevel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public string ProductIds { get; }

        public bool AutoFollowBetIsDummyAgent { get; }
    }
}
