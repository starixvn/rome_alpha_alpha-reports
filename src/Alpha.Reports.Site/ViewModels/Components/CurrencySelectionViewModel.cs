namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CurrencySelectionViewModel
    {
        public CurrencySelectionViewModel(
            IEnumerable<SelectListItem> currencies,
            int userSelectedCurrencyId)
        {
            Currencies = currencies;
            UserSelectedCurrencyId = userSelectedCurrencyId;
        }

        public IEnumerable<SelectListItem> Currencies { get; }

        public int UserSelectedCurrencyId { get; }
    }
}