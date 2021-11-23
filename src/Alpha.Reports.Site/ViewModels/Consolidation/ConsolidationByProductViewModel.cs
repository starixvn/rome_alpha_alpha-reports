namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;

    public class ConsolidationByProductViewModel
    {
        public ConsolidationByProductViewModel(
            int productId,
            string productName,
            IEnumerable<ConsolidationEntityViewModel> winLossByUsername)
        {
            ProductId = productId;
            ProductName = productName;
            WinLossByUsername = winLossByUsername;
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public IEnumerable<ConsolidationEntityViewModel> WinLossByUsername { get; }
    }
}