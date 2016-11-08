namespace Alpha.Reports.Site.ViewModels.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public abstract class ReportPageViewModel<TData>
    {
        public PageHeaderViewModel Header { get; }

        public abstract FilterControlViewModel Filter { get; }

        public abstract DataGridViewModel<TData> DataGrid { get; }

        public PagerViewModel Pager { get; }
    }
}