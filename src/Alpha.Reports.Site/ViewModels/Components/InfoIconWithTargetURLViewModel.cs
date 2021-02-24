namespace Alpha.Reports.Site.ViewModels.Components
{
    public class InfoIconWithTargetURLViewModel
    {
        public InfoIconWithTargetURLViewModel(string href, string target)
        {
            Href = href;
            Target = target;
        }

        public string Href { get; }

        public string Target { get; }
    }
}