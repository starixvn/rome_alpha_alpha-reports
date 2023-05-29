namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Enumerations;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;

    public interface CustomerService
    {
        CustomerLevel GetCustomerLevel(string username, int uplineId);

        CustInfo GetCustInfo(CustomerLevel customerLevel);

        IEnumerable<CustInfo> GetCustInfoList(CustomerLevel customerLevel, UserLevel viewerLevel);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public CustomerLevel GetCustomerLevel(string username, int uplineId)
            => dynamicRepository.Get<CustomerLevel>(new GetCustomerLevelCriteria(username, uplineId));

        public CustInfo GetCustInfo(CustomerLevel customerLevel)
        {
            var custLevel = Enumeration.FromValue<UserLevel>(customerLevel.RoleId);

            if (custLevel.IsMasterLevel())
            {
                return new CustInfo(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);
            }

            if (custLevel.IsAgentLevel())
            {
                return new CustInfo(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
            }

            return new CustInfo(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value);
        }

        public IEnumerable<CustInfo> GetCustInfoList(CustomerLevel customerLevel, UserLevel viewerLevel)
        {
            var custLevel = Enumeration.FromValue<UserLevel>(customerLevel.RoleId);

            if (viewerLevel.IsSuperLevel())
            {
                yield return new CustInfo(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);

                if (custLevel.IsMemberOrAgentLevel())
                {
                    yield return new CustInfo(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
                }
            }
            else if (viewerLevel.IsMasterLevel())
            {
                yield return new CustInfo(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
            }

            if (custLevel.IsMemberLevel() && customerLevel.AgentId.HasValue && !string.IsNullOrWhiteSpace(customerLevel.AgentName))
            {
                yield return new CustInfo(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value);
            }
        }
    }
}
