using Alpha.Reports.Enumerations;

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
            string firstName,
            string lastName,
            string username2,
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
            FirstName = firstName;
            LastName = lastName;
            Username2 = username2;
            MemberId = memberId;
            MemberName = memberName;
            AgentId = agentId;
            AgentName = agentName;
            MasterId = masterId;
            MasterName = masterName;
            SuperId = superId;
            SuperName = superName;
        }

        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username2 { get; set; }

        public int? MemberId { get; set; }

        public string MemberName { get; set; }

        public int? AgentId { get; set; }

        public string AgentName { get; set; }

        public int? MasterId { get; set; }

        public string MasterName { get; set; }

        public int SuperId { get; set; }

        public string SuperName { get; set; }
    }
}
