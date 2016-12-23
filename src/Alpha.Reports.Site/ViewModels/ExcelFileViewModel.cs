namespace Alpha.Reports.Site.ViewModels
{
    using Helpers;

    public class ExcelFileViewModel
    {
        public ExcelFileViewModel(string name, byte[] content)
        {
            Name = name;
            Content = content;
            ContentType = ExcelHelper.ContentType;
        }

        public string Name { get; }

        public byte[] Content { get; }

        public string ContentType { get; }
    }
}