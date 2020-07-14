namespace Alpha.Reports.SpamBlocker.Models
{
    using System.Web.Mvc;

    public interface IRequestIdentity
    {
        string GetKey(ActionExecutingContext filterContext);
    }
}
