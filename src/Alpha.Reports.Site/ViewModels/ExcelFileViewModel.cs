namespace Alpha.Reports.Site.ViewModels
{
    public class ExcelFileViewModel
    {
        public ExcelFileViewModel(string name, byte[] content)
        {
            Name = string.IsNullOrWhiteSpace(name) 
                ? "no-name.xls"
                : $"{name.Replace(' ', '_').Replace('/', '_')}.xls";

            Content = content;            
        }

        public string Name { get; }

        public byte[] Content { get; }

        public string ContentType => "application/vnd.ms-excel";
    }
}