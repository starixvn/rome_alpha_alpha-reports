namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class GroupProductViewModel
    {
        public GroupProductViewModel(SelectListGroup group, IEnumerable<SelectListItem> products)
        {
            Group = group;
            Products = products;
        }

        public SelectListGroup Group { get; }

        public IEnumerable<SelectListItem> Products { get; }

        public bool HasGroupName => !string.IsNullOrWhiteSpace(Group.Name);
    }
}