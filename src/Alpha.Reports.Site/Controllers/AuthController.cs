using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Alpha.Auth.Filters;
using Alpha.Reports.Site.Constants;
using Alpha.Web.Filters;
using Fanex.Logging.Extensions.Web;
using RSACryptoPad;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required), AllowProtectedMode, AllowAnonymous]
    public class AuthController : Controller
    {
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
                ExceptionHandler.Handle(new Exception("Cannot verify request key " + Request.RawUrl), TraceEventType.Warning);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                return new EmptyResult();
            }

            IUserSession session = UserSession.Current;

            if (session.IsLogged && Request.IsAuthenticated)
            {
                return RedirectToSpecificUrl();
            }

            var parameters = parameter.Split('&');
            SetUserSessionsAndAuth(parameters);

            if (session.IsLogged && Request.IsAuthenticated)
            {
                RegisterGlobalFilters();
            }

            return RedirectToSpecificUrl();
        }

        private bool IsValidReferrer()
        {
            if (Request.UrlReferrer == null)
            {
                return false;
            }

            var session = UserSession.Current;
            if (session.IsInternal 
                || session.IsIndirectLoggedIn
                || Request.Headers["Referer"].IndexOf(Request.Headers["Host"], StringComparison.OrdinalIgnoreCase) != -1)
            {
                return true;
            }

            foreach (var key in WebConfigurationManager.AppSettings.AllKeys)
            {
                if (WebConfigurationManager.AppSettings[key].IndexOf(Request.UrlReferrer.Host, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        private static void SetUserSessionsAndAuth(string[] parameters)
        {
            var sessions = parameters[1].Split(';');
            var userSession = UserSession.Current;

            userSession.SetProperty(SessionKey.CUSTID, Convert.ToInt32(sessions[0]));
            userSession.SetProperty(SessionKey.USERNAME, sessions[1]);
            userSession.SetProperty(SessionKey.SITE, sessions[2]);
            userSession.SetProperty(SessionKey.LANGUAGE, sessions[3]);
            userSession.SetProperty(SessionKey.LANGKEY, sessions[4]);
            userSession.SetProperty(SessionKey.ROLEID, Convert.ToByte(sessions[5]));
            userSession.SetProperty(SessionKey.CURRENCY, sessions[6]);
            userSession.SetProperty(SessionKey.CURRENCYID, Convert.ToInt32(sessions[7]));
            userSession.SetProperty(SessionKey.ISSHOWCONSOLIDATEDREPORT, Convert.ToBoolean(sessions[8]));
            userSession.SetProperty(SessionKey.CustomerDatelimit, Convert.ToDateTime(sessions[9]));
            userSession.SetProperty(SessionKey.ISADMINCANVIEWVNIP, Convert.ToBoolean(sessions[10]));
            userSession.SetProperty(SessionKey.SUBACCID, Convert.ToInt32(sessions[11]));
            userSession.SetProperty(SessionKey.USERLOGINIP, sessions[12]);
            userSession.SetProperty(SessionKey.ISPROTECTEDMODESESSIONKEY, Convert.ToBoolean(sessions[13]));
            userSession.SetProperty(SessionKey.DARKMODENAMESESSIONKEY, sessions[14]);
            userSession.SetProperty(SessionKey.MainDomainSessionKey, sessions[15]);
            userSession.SetProperty(SessionKey.LoginIP_Browser, sessions[16]);
            userSession.SetProperty(SessionKey.ASSETAPPPATHSESSIONKEY, sessions[17]);
            
            var enableJackpotGames = sessions[18].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            userSession.SetProperty(SessionKey.GameEnable + SessionKey.Jackpot, enableJackpotGames);
            
            userSession.SetProperty(SessionKey.ISCASINOSYNC, Convert.ToBoolean(sessions[19]));
            userSession.SetSyncedProduct(SessionKey.RNGCasino, Convert.ToBoolean(sessions[19]));

            var pageAccess = sessions[20].Split(new[] { '$' }, StringSplitOptions.RemoveEmptyEntries).Select(bool.Parse);
            var subPageAccess = new PageAccess
            {
                CreateAccount = pageAccess.ElementAt(0),
                Transfer = pageAccess.ElementAt(1),
                UpdateAccount = pageAccess.ElementAt(2),
                ViewLog = pageAccess.ElementAt(3),
                ViewMemberInfo = pageAccess.ElementAt(4),
                ViewReports = pageAccess.ElementAt(5),
                ViewTotalBets = pageAccess.ElementAt(6)
            };

            subPageAccess.SetPermission("[L]", pageAccess.ElementAt(7));
            userSession.SetProperty(SessionKey.SUBPAGEACCESS, subPageAccess);

            FormsAuthentication.SetAuthCookie(parameters[0], false);
        }

        private static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new CheckBelongingFilter());
            GlobalFilters.Filters.Add(new AfterLoginFlowFilter());
            GlobalFilters.Filters.Add(new ProtectedModeFilter());
        }

        private ActionResult RedirectToSpecificUrl()
        {
            if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
            {
                return Redirect(Request["ReturnUrl"]);
            }

            return Json(Session.SessionID, JsonRequestBehavior.AllowGet);
        }
    }
}