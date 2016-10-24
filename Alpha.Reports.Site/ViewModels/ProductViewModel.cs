namespace Alpha.Reports.Site.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(
            int id,
            string displayText,
            string storeProcedureParameter)
        {
            Id = id;
            DisplayText = displayText;
            StoreProcedureParameter = storeProcedureParameter;
        }

        public int Id { get; }

        public string DisplayText { get; }

        public string StoreProcedureParameter { get; }
    }
}