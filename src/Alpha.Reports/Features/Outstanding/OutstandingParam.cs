namespace Alpha.Reports.Params
{
    using Models;

    public class OutstandingParam
    {
        public OutstandingParam(UserLevel userLevel, UserLevel custLevel, int custId, string productIds)
        {
            UserLevel = userLevel;
            CustLevel = custLevel;
            CustId = custId;
            ProductIds = productIds;
        }

        public UserLevel UserLevel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public string ProductIds { get; }
    }
}
