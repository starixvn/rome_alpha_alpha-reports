namespace Alpha.Reports.Site.ViewModels.ComponentViewModels
{
    public class PageTitleViewModel
    {
        public PageTitleViewModel(
            string title = "",
            bool hasStackIcon = true,
            bool hasHideFilterIcon = true,
            bool isStackMode = false,
            bool hideFilter = false)
        {
            Title = title;
            HasStackModeIcon = hasStackIcon;
            HasHideFilterIcon = hasHideFilterIcon;
            IsStackMode = isStackMode;
            HideFilter = hideFilter;
        }

        public string Title { get; }

        public bool HasStackModeIcon { get; }

        public bool HasHideFilterIcon { get; }

        public bool IsStackMode { get; }

        public bool HideFilter { get; }
    }
}