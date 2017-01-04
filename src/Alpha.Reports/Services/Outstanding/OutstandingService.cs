using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Services.Outstanding
{
    public interface OutstandingService
    {
        IEnumerable<OutstandingModel> GetOutstanding(int custId, UserLevel userLevel);
    }
}
