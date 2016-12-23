namespace Alpha.Reports.Site.ViewModels.Components
{
    public abstract class ReportPageViewModel<TData>
    {
        public PageHeaderViewModel Header { get; }

        public abstract FilterControlViewModel Filter { get; }

        public abstract DataGridViewModel<TData> DataGrid { get; }

        public PagerViewModel Pager { get; }
    }
}