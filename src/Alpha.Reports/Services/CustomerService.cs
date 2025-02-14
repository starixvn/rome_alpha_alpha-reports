namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Enumerations;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;

    public interface CustomerService
    {
        CustomerLevel GetCustomerLevel(string username, int uplineId);

        CustInfo GetCustInfo(CustomerLevel customerLevel);

        IEnumerable<CustInfo> GetCustInfoList(CustomerLevel customerLevel, UserLevel viewerLevel);

        void UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname);

        Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo);
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
                return new CustInfo(customerLevel.MasterId.Value, customerLevel.MasterName, UserLevel.Master.Value);
            }

            if (custLevel.IsMemberLevel())
            {
                if (customerLevel.AgentId == DirectMemberAgentIds.Super)
                {
                    return new CustInfo(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);
                }

                if (customerLevel.AgentId == DirectMemberAgentIds.Master)
                {
                    return new CustInfo(customerLevel.MasterId.Value, customerLevel.MasterName, UserLevel.Master.Value);
                }
            }

            if (customerLevel.AgentId.HasValue)
            {
                return new CustInfo(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value);
            }

            if (customerLevel.MasterId.HasValue)
            {
                return new CustInfo(customerLevel.MasterId.Value, customerLevel.MasterName, UserLevel.Master.Value);
            }

            return new CustInfo(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);
        }

        public IEnumerable<CustInfo> GetCustInfoList(CustomerLevel customerLevel, UserLevel viewerLevel)
        {
            if (viewerLevel.IsAgentLevel())
            {
                return Enumerable.Empty<CustInfo>();
            }

            var custInfoList = new List<CustInfo>();
            var custLevel = Enumeration.FromValue<UserLevel>(customerLevel.RoleId);

            if (viewerLevel.IsMasterLevel())
            {
                custInfoList.Add(new CustInfo(customerLevel.MasterId.Value, customerLevel.MasterName, UserLevel.Master.Value));
            }
            else if (viewerLevel.IsSuperLevel())
            {
                custInfoList.Add(new CustInfo(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value));

                if (custLevel.IsMemberOrAgentLevel() && customerLevel.MasterId.HasValue)
                {
                    custInfoList.Add(new CustInfo(customerLevel.MasterId.Value, customerLevel.MasterName, UserLevel.Master.Value));
                }
            }

            if (custLevel.IsMemberLevel() && customerLevel.AgentId > 0)
            {
                custInfoList.Add(new CustInfo(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value));
            }

            return custInfoList;
        }

        public void UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
            => dynamicRepository.Execute(new UpdateUserSearchCriteria(userId, search, firstName, lastName, nickname));

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo)
            => (await dynamicRepository.FetchAsync<UsernameSuggestion>(new GetUsernameSuggestionCriteria(
                actorId,
                suggestionQuery,
                isInternal,
                directDownline,
                isAccInfo))).OrderBy(item => item.Username);
    }
}
