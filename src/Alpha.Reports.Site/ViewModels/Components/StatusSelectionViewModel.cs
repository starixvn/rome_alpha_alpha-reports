namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class StatusSelectionViewModel
    {
        public StatusSelectionViewModel(IEnumerable<SelectListItem> statuses, string userSelectedStatusId)
        {
            Statuses = statuses;
            UserSelectedStatusId = userSelectedStatusId;
        }

        public IEnumerable<SelectListItem> Statuses { get; }

        public string UserSelectedStatusId { get; }
    }
}