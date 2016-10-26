namespace Alpha.Reports.Site.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(int id, string displayText)
        {
            Id = id;
            DisplayText = displayText;
        }

        public int Id { get; }

        public string DisplayText { get; }
    }
}