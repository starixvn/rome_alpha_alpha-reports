using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Services
{
    public interface OutstandingService
    {
        IEnumerable<OutstandingModel> GetOutstanding(int custId, UserLevel userLevel);
    }
}
