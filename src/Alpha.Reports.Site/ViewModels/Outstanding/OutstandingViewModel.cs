using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Site.ViewModels.Outstanding
{
    public class OutstandingViewModel
    {
        public string Title { get; set; }

        public IEnumerable<OutstandingModel> Outstandings { get; set; }

        public OutstandingModel Total { get; set; }
    }
}