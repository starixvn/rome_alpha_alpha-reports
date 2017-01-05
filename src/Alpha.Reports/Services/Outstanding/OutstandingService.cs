using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Services.Outstanding
{
    public interface OutstandingService
    {
        IEnumerable<OutstandingModel> GetMemberOutstanding(UserLevel userLevel, UserLevel custLevel, int custId);
    }
}
