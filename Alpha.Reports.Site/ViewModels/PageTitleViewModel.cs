namespace Alpha.Reports.Site.ViewModels
{
    public class PageTitleViewModel
    {
        public PageTitleViewModel(
            string title,
            bool hasStackIcon,
            bool hasHideFilterIcon,
            bool isStackMode,
            bool hideFilter)
        {
            Title = title;
            HasStackIcon = hasStackIcon;
            HasHideFilterIcon = hasHideFilterIcon;
            IsStackMode = isStackMode;
            HideFilter = hideFilter;
        }

        public string Title { get; }

        public bool HasStackIcon { get; }

        public bool HasHideFilterIcon { get; }

        public bool IsStackMode { get; }

        public bool HideFilter { get; }
    }
}