using System.Collections.Generic;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.DataAccesses
{
    public interface OutstandingDataAccess
    {
        IEnumerable<OutstandingModel> GetMemberOutstandingForSuper(int custId);

        IEnumerable<OutstandingModel> GetMemberOutstandingForMaster(int custId);

        IEnumerable<OutstandingModel> GetMemberOutstandingForAgent(int custId);
    }
}
