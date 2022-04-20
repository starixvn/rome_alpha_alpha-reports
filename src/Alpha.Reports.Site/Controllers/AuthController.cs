using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Alpha.Auth.Filters;
using Alpha.Reports.Configurations;
using Alpha.Reports.Site.Constants;
using Alpha.SecurityCode.Web;
using Alpha.Web.Filters;
using Fanex.Cryptography.Aes;
using Fanex.Logging.Extensions.Web;
using Newtonsoft.Json;
using Sunplus.Agent.Model.BL;
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

            var session = UserSession.Current;
            if (!string.IsNullOrWhiteSpace(param))
            {
                session.SetProperty(SessionKey.LoginParams, param);
            }

            param = string.IsNullOrWhiteSpace(param) ? session.GetProperty<string>(SessionKey.LoginParams) + string.Empty : param;
            var parameter = param;

            try
            {
                parameter = AesService.Default.Decrypt(parameter, "!@#$");
            }
            catch
            {
                parameter = string.Empty;
                ExceptionHandler.Handle(new Exception("Cannot verify request key " + Request.RawUrl), TraceEventType.Warning);
            }
            
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return new EmptyResult();
            }

            var parameters = parameter.Split('&');
            if (session.IsLogged && Request.IsAuthenticated)
            {
                UpdateReportRoutingRealtime(parameters);

                return RedirectToSpecificUrl();
            }
            
            SetUserSessionsAndAuth(parameters);
            if (session.IsLogged && Request.IsAuthenticated)
            {
                RegisterGlobalFilters();
            }

            return RedirectToSpecificUrl();
        }

        // GET: SecurityCode
        [HttpGet]
        public ActionResult SecurityCode(string s)
        {
            if (!IsValidReferrer())
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.SetProperty(SessionKey.SECURITYCODE, s);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // GET: SecurityCodeBetlist
        [HttpGet]
        public ActionResult SecurityCodeBetlist(bool s)
        {
            if (!IsValidReferrer())
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.SetProperty(SessionKey.ISENABLEDSECURITYCODEINBETLISTS, s);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // GET: SecurityCodeStatement
        [HttpGet]
        public ActionResult SecurityCodeStatement(bool s)
        {
            if (!IsValidReferrer())
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.SetProperty(SessionKey.ISENABLEDSECURITYCODEINSTATEMENT, s);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        // GET: Otp
        [HttpGet]
        public ActionResult Otp()
        {
            if (!IsValidReferrer())
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = true;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;

            return Json("OK", JsonRequestBehavior.AllowGet);
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
                || Request.Headers["Referer"].IndexOf(Request.Headers["Host"], StringComparison.OrdinalIgnoreCase) != -1 
                || Request.Headers["Referer"].IndexOf(session.GetProperty<string>(SessionKey.MainDomainSessionKey) ?? string.Empty) != -1)
            {
                return true;
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

            userSession.SetProperty(SessionKey.ISENABLEDSECURITYCODEINSTATEMENT, Convert.ToBoolean(sessions[21]));
            userSession.SetProperty(SessionKey.SECURITYCODE, sessions[22]);

            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = Convert.ToBoolean(sessions[23]);
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = Convert.ToBoolean(sessions[24]);

            UpdateReportRoutingRealtime(parameters);

            userSession.SetProperty(SessionKey.ADMINID, Convert.ToInt32(sessions[26]));
            userSession.SetProperty(SessionKey.ADMINNAME, sessions[27]);

            userSession.SetProperty(SessionKey.IndirectCustIdSessionKey, Convert.ToInt32(sessions[28]));
            userSession.SetProperty(SessionKey.IndirectSubAccIdSessionKey, Convert.ToInt32(sessions[29]));
            userSession.SetProperty(SessionKey.IndirectUsernameSessionKey, sessions[30]);

            SetOutputValuesToSession(parameters);

            FormsAuthentication.SetAuthCookie(parameters[0], false);
        }

        private static void UpdateReportRoutingRealtime(string[] parameters)
        {
            var sessions = parameters[1].Split(';');
            var session = UserSession.Current;
            if (!string.Equals(session.GetProperty<string>(SessionKey.ReportRouting), sessions[25], StringComparison.OrdinalIgnoreCase))
            {
                session.SetProperty(SessionKey.ReportRouting, sessions[25]);
            }
        }

        private static void SetOutputValuesToSession(string[] parameters)
        {
            var outputs = JsonConvert.DeserializeObject<Hashtable>(parameters[2]);
            UserSessionService.SetUserSessionValues(UserSession.Current, outputs, new Action<IUserSession, Hashtable>((userSession, values) => { }));
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
                return Redirect($"{SiteSettings.ReportRouting}{Request["ReturnUrl"]}");
            }

            return new EmptyResult();
        }
    }
}