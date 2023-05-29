namespace Alpha.Reports.Models
{
    public class CustomerLevel
    {
        public CustomerLevel()
        {
        }

        public CustomerLevel(
            int custId,
            byte roleId,
            string username,
            int? memberId,
            string memberName,
            int? agentId,
            string agentName,
            int masterId,
            string masterName,
            int superId,
            string superName)
        {
            CustId = custId;
            RoleId = roleId;
            Username = username;
            MemberId = memberId;
            MemberName = memberName;
            AgentId = agentId;
            AgentName = agentName;
            MasterId = masterId;
            MasterName = masterName;
            SuperId = superId;
            SuperName = superName;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public string Username { get; }

        public int? MemberId { get; }

        public string MemberName { get; }

        public int? AgentId { get; }

        public string AgentName { get; }

        public int MasterId { get; }

        public string MasterName { get; }

        public int SuperId { get; }

        public string SuperName { get; }
    }
}
