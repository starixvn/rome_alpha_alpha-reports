namespace Alpha.Reports.Site.ViewModels
{
    public class ExcelFileViewModel
    {
        private const string ExcelContentType = "application/vnd.ms-excel";

        public ExcelFileViewModel(string name, byte[] content)
        {
            Name = name;
            Content = content;
            ContentType = ExcelContentType;
        }

        public string Name { get; }

        public byte[] Content { get; }

        public string ContentType { get; }
    }
}