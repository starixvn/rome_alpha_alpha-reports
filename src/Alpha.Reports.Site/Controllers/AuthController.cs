using Alpha.Web.Filters;
using Fanex.Logging.Extensions.Web;
using RSACryptoPad;
using Sunplus.Agent.Model.Entities;
using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required), AllowProtectedMode, AllowAnonymous]
    public class AuthController : Controller
    {
        // GET: Auth
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

            string[] ps = parameter.Split('&');
            string nickname;
            int custId = 0, subAccId = 0;
            long timeTicks = DateTime.Now.Ticks;

            nickname = ps[4];
            IUserSession session = UserSession.Current;

            if (ps.Length < 10
                || !int.TryParse(ps[0], out custId)
                || !int.TryParse(ps[1], out subAccId)
                || (session.IsLogged && !session.NickName.Equals(nickname, StringComparison.OrdinalIgnoreCase)))
            {
                return new EmptyResult();
            }

            if (session.IsLogged && Request.IsAuthenticated && session.NickName.Equals(nickname, StringComparison.OrdinalIgnoreCase))
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

            foreach (var key in WebConfigurationManager.AppSettings.AllKeys)
            {
                if (WebConfigurationManager.AppSettings[key].IndexOf(Request.UrlReferrer.Authority) != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}