namespace Alpha.Reports.Site.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(
            int id,
            string displayText,
            string netTurnoverTooltip)
        {
            Id = id;
            DisplayText = displayText;
            NetTurnoverTooltip = netTurnoverTooltip;
        }

        public int Id { get; }

        public string DisplayText { get; }

        public string NetTurnoverTooltip { get; }
    }
}