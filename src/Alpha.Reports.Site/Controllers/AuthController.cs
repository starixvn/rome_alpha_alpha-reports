using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Alpha.Reports.Models;
using Alpha.Web.Filters;
using Fanex.Logging.Extensions.Web;
using RSACryptoPad;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required), AllowProtectedMode, AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IUser user;

        public AuthController(IUser user)
        {
            this.user = user;
        }

        // GET: Auth
        [HttpGet]
        public ActionResult Index(string param)
        {
            if (!IsValidReferrer())
            {
                return new EmptyResult();
            }

            if (!string.IsNullOrWhiteSpace(param))
            {
                Session["LoginParams"] = param;
            }

            const int dwKeySize = 1024;
            var keyPath = Server.MapPath("~/IbcKeys/ibckey.snk");
            param = string.IsNullOrWhiteSpace(param) ? Session["LoginParams"] + string.Empty : param;
            var parameter = param;
            parameter = parameter.Replace(' ', '+');
            var encryption = new EncryptionThread();

            try
            {
                parameter = encryption.DecryptString(parameter, dwKeySize, keyPath);
            }
            catch
            {
                parameter = null;
                ExceptionHandler.Handle(new Exception("Cannot verify request key " + Request.RawUrl));
            }

            if (null == parameter)
            {
                return new EmptyResult();
            }

            IUserSession session = UserSession.Current;

            if (session.IsLogged && !IsValidLoginAccountName(parameter))
            {
                return new EmptyResult();
            }

            if (session.IsLogged && Request.IsAuthenticated && IsValidLoginAccountName(parameter))
            {
                return Json(Session.SessionID, JsonRequestBehavior.AllowGet);
            }

            FormsAuthentication.SetAuthCookie(session.UserName, false);

            if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
            {
                return Redirect(Request["ReturnUrl"]);
            }

            return Json(Session.SessionID, JsonRequestBehavior.AllowGet);
        }

        private bool IsValidReferrer()
        {
            if (Request.UrlReferrer == null)
            {
                return false;
            }

            var session = UserSession.Current;
            if (session.IsInternal || session.IsIndirectLoggedIn)
            {
                return true;
            }

            foreach (var key in WebConfigurationManager.AppSettings.AllKeys)
            {
                if (WebConfigurationManager.AppSettings[key].IndexOf(Request.UrlReferrer.Authority, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsValidLoginAccountName(string decryptedParams)
        {
            var session = UserSession.Current;
            var loginUsername = session.IsInternal ? session.UserName :
                session.IsIndirectLoggedIn ? session.IndirectUsername :
                session.NickName;
            string[] ps = decryptedParams.Split('&');

            if (session.IsInternal)
            {
                return ps[0].Equals(loginUsername, StringComparison.OrdinalIgnoreCase);
            }

            if (session.IsIndirectLoggedIn)
            {
                return ps[8].Equals(loginUsername, StringComparison.OrdinalIgnoreCase);
            }

            return ps[4].Equals(loginUsername, StringComparison.OrdinalIgnoreCase);
        }
    }
}