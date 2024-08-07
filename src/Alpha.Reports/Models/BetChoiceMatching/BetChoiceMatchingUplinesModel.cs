namespace Alpha.Reports.Models.BetChoiceMatching
{
    public class BetChoiceMatchingUplinesModel
    {
        public BetChoiceMatchingUplinesModel()
        {
        }

        public BetChoiceMatchingUplinesModel(
            int agentId,
            string agentName,
            int masterId,
            string masterName)
        {
            AgentId = agentId;
            AgentName = agentName;
            MasterId = masterId;
            MasterName = masterName;
        }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int MasterId { get; set; }

        public string MasterName { get; set; }
    }
}
