namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class PageHeaderViewModel
    {
        public PageHeaderViewModel(
            string title,
            bool hasStackIcon,
            bool hasHideFilterIcon,
            bool isStackMode,
            bool hideFilter,
            IEnumerable<ActionLinkViewModel> breadcrumbLevels)
        {
            Title = title;
            HasStackIcon = hasStackIcon;
            HasHideFilterIcon = hasHideFilterIcon;
            IsStackMode = isStackMode;
            HideFilter = hideFilter;
            BreadcrumbLevels = breadcrumbLevels;
        }

        public string Title { get; }

        public bool HasStackIcon { get; }

        public bool HasHideFilterIcon { get; }

        public bool IsShowActionIcons => HasStackIcon || HasHideFilterIcon;

        public bool IsStackMode { get; }

        public bool HideFilter { get; }

        public IEnumerable<ActionLinkViewModel> BreadcrumbLevels { get; set; }
    }
}