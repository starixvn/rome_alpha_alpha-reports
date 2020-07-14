namespace Alpha.Reports.SpamBlocker.Models
{
    using Alpha.Reports.SpamBlocker.Web;
    using System.Web;

    public class IpIdentity : IRequestIdentity
    {
        public string GetKey()
        {
            return HttpContext.Current.Request.GetClientIp().ToString();
        }
    }
}
