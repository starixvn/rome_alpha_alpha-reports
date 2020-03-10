namespace Alpha.Reports.Models.BetChoiceMatching
{
    public class BetChoiceMatchingSharedIPModel
    {
        public BetChoiceMatchingSharedIPModel()
        {
        }

        public BetChoiceMatchingSharedIPModel(
            string ip,
            int custId,
            string username)
        {
            IP = ip;
            CustId = custId;
            Username = username;
        }

        public string IP { get; private set; }

        public int CustId { get; private set; }

        public string Username { get; private set; }
    }
}
