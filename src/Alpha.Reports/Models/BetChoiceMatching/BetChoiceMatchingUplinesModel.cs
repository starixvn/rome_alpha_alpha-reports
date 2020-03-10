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

        public int AgentId { get; private set; }

        public string AgentName { get; private set; }

        public int MasterId { get; private set; }

        public string MasterName { get; private set; }
    }
}
