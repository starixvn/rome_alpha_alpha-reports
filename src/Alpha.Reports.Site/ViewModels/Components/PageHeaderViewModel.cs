namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using Reports.Models;

    public class PageHeaderViewModel
    {
        public PageHeaderViewModel(
            string title,
            bool hasStackIcon,
            bool hasHideFilterIcon,
            bool isStackMode,
            bool isFilterVisible,
            IEnumerable<CustInfo> breadcrumbLevels)
        {
            Title = title;
            HasStackIcon = hasStackIcon;
            HasHideFilterIcon = hasHideFilterIcon;
            IsStackMode = isStackMode;
            IsFilterVisible = isFilterVisible;
            BreadcrumbLevels = breadcrumbLevels;
        }

        public string Title { get; }

        public bool HasStackIcon { get; }

        public bool HasHideFilterIcon { get; }

        public bool IsShowActionIcons => HasStackIcon || HasHideFilterIcon;

        public bool IsStackMode { get; }

        public bool IsFilterVisible { get; }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; set; }
    }
}