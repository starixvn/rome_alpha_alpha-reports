using System.Collections.Generic;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.DataAccesses.Outstanding
{
    public interface OutstandingDataAccess
    {
        IEnumerable<OutstandingModel> GetMemberOutstandingForSuper(int superId);

        IEnumerable<OutstandingModel> GetMemberOutstandingForMaster(int masterId);

        IEnumerable<OutstandingModel> GetMemberOutstandingForAgent(int agentId);
    }
}
