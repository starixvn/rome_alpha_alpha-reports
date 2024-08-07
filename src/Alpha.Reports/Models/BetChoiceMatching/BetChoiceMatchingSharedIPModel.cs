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

        public string IP { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }
    }
}
