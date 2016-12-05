namespace Alpha.Reports.Site.ViewModels
{
    public class ActionLinkViewModel
    {
        public ActionLinkViewModel(
            string controllerName,
            string actionName,
            string linkText,
            object routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            LinkText = linkText;
            RouteValues = routeValues;
        }

        public string ControllerName { get; }

        public string ActionName { get; }

        public string LinkText { get; }

        public object RouteValues { get; }
    }
}