using System.Collections.Generic;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    public class OutstandingViewModel
    {
        public IEnumerable<OutstandingModel> Outstandings { get; set; }
    }
}