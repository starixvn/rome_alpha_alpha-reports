using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Alpha.NetThrottling.Mvc;
using Alpha.Reports.Configurations;
using Alpha.Reports.Site.Constants;
using Alpha.SecurityCode.Web;
using Alpha.Web.Filters;
using Fanex.Cryptography.Aes;
using Fanex.Logging.Extensions.Web;
using Newtonsoft.Json;
using ONEbook.UM;
using ONEbook.UM.AspNetMVC.Filters;
using ONEbook.UM.Services;
using Sunplus.Agent.Model.BL;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required), AllowProtectedMode, AllowAnonymous]
    public class AuthController : Controller
    {
        private const string English = "en-US";
        private const string ChineseTraditional = "zh-TW";
        private const string ChineseSimplified = "zh-CN";
        private const string Japanese = "ja-JP";
        private const string Thai = "th-TH";
        private const string Korean = "ko-KR";
        private const string Vietnamese = "vi-VN";
        private const string Default = English;
        private const string SecretKey = "!@#$";
        private const string OK = "OK";
        private const string Failed = "Failed";
        private const string NotSupported = "Not supported";
        private const int LoginTimeoutSeconds = 10;

        // GET: Auth
        [HttpGet, DisableNetThrottling]
        public ActionResult Index(string param)
        {
            if (!IsValidReferrer())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!SiteSettings.IsStandaloneReport)
            {
                return RedirectToSpecificUrl();
            }

            var session = UserSession.Current;
            var authParams = string.IsNullOrWhiteSpace(param) ? session.GetProperty<string>(SessionKey.LoginParams) + string.Empty : param;
            var parameter = string.Empty;

            if (!string.IsNullOrWhiteSpace(authParams))
            {
               parameter = AesService.Default.Decrypt(authParams, SecretKey);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                if (Session.IsCookieless)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                return Redirect($"{UserSession.Current.GetProperty<string>(SessionKey.MainDomainSessionKey)}/site-main/Dashboard/Index");
            }

            if (!string.IsNullOrWhiteSpace(authParams) &&
                !string.Equals(authParams, session.GetProperty<string>(SessionKey.LoginParams) ?? string.Empty, StringComparison.OrdinalIgnoreCase))
            {
                session.SetProperty(SessionKey.LoginParams, authParams);
            }

            try
            {
                var parameters = parameter.Split('&');
                if (IsLoggedIn()
                    && !string.IsNullOrWhiteSpace(session.UserName)
                    && session.UserName.Equals(parameters[0], StringComparison.OrdinalIgnoreCase))
                {
                    SetRealtimeValues(parameters, session);
                    UpdateReportRoutingRealtime(parameters, session);

                    return RedirectToSpecificUrl();
                }

                if (!string.IsNullOrWhiteSpace(param)
                    && DateTime.Now.Subtract(Convert.ToDateTime(parameters[3])).TotalSeconds > LoginTimeoutSeconds)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SetUserSessionsAndAuth(parameters, session);
            }
            catch (Exception e) { ExceptionHandler.Handle(new Exception(e.Message, e), TraceEventType.Warning); }

            return RedirectToSpecificUrl();
        }

        // GET: SecurityCode
        [HttpGet]
        public ActionResult SecurityCode()
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        // GET: SecurityCodeBetlist
        [HttpGet]
        public ActionResult SecurityCodeBetlist(bool s)
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.SetProperty(SessionKey.ISENABLEDSECURITYCODEINBETLISTS, s);

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        // GET: SecurityCodeStatement
        [HttpGet]
        public ActionResult SecurityCodeStatement(bool s)
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.SetProperty(SessionKey.ISENABLEDSECURITYCODEINSTATEMENT, s);

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        // GET: Otp
        [HttpGet]
        public ActionResult Otp()
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = true;
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = true;

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangeTheme(bool isDarkMode)
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            UserSession.Current.DarkModeName = isDarkMode ? "-dark" : string.Empty;

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangeLang(string langCode)
        {
            if (!IsValidReferrer()
                || !IsLoggedIn())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            var userSession = UserSession.Current;

            if (string.IsNullOrWhiteSpace(langCode) ||
                !CodeKeys.ContainsKey(langCode))
            {
                return Json(NotSupported, JsonRequestBehavior.AllowGet);
            }

            if (SiteSettings.DisabledVietnamese && langCode == Vietnamese)
            {
                langCode = Default;
            }

            userSession.Language = langCode;
            userSession.LangKey = CodeKeys[langCode];

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            if (!IsValidReferrer())
            {
                return Json(Failed, JsonRequestBehavior.AllowGet);
            }

            Session.Clear();

            if (!Session.IsCookieless)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        private static readonly IDictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [Japanese] = "J",
            [Thai] = "TH",
            [Korean] = "KO",
            [Vietnamese] = "VI"
        };

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

        private void SetRealtimeValues(string[] parameters, IUserSession userSession)
        {
            SetOutputValuesToSession(parameters, userSession);

            var sessions = parameters[1].Split(';');

            userSession.SetProperty(SessionKey.LANGUAGE, sessions[3]);
            userSession.SetProperty(SessionKey.LANGKEY, sessions[4]);
            userSession.SetProperty(SessionKey.ISPROTECTEDMODESESSIONKEY, Convert.ToBoolean(sessions[13]));
            userSession.SetProperty(SessionKey.DARKMODENAMESESSIONKEY, sessions[14]);
            userSession.SetProperty(SessionKey.MainDomainSessionKey, sessions[15]);

            userSession.SetProperty(SessionKey.ISENABLEDSECURITYCODEINSTATEMENT, Convert.ToBoolean(sessions[21]));
            userSession.SetProperty(SessionKey.SECURITYCODE, sessions[22]);

            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp = Convert.ToBoolean(sessions[23]);
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode = Convert.ToBoolean(sessions[24]);

            userSession.SetProperty(SessionKey.ISFORCEDSECURITYCODE, false);
            userSession.SetProperty(SessionKey.ISPASSWORDEXPIRED, false);
            userSession.SetProperty(SessionKey.NICKNAME, sessions[32]);
            userSession.SetProperty(SessionKey.ISLOGGED, true);
            userSession.SetProperty(SessionKey.ISNICKNAMELOGINONLY, true);

            Session["ONEbook.UM.ByPassUM"] = sessions[33];
            userSession.SetProperty(SessionKey.BetlistSessionID, sessions[37]);
            userSession.SetProperty(SessionKey.ReportSessionID, sessions[39]);
            userSession.SetProperty(SessionKey.ISENABLEDSECURITYCODEINBETLISTS, Convert.ToBoolean(sessions[40]));
        }

        private void SetUserSessionsAndAuth(string[] parameters, IUserSession userSession)
        {
            FormsAuthentication.SetAuthCookie(parameters[0], false);

            var sessions = parameters[1].Split(';');
            userSession.SetProperty(SessionKey.SUBACCID, Convert.ToInt32(sessions[11]));

            userSession.SetProperty(SessionKey.CUSTID, Convert.ToInt32(sessions[0]));
            userSession.SetProperty(SessionKey.USERNAME, sessions[1]);
            userSession.SetProperty(SessionKey.SITE, sessions[2]);
            userSession.SetProperty(SessionKey.ROLEID, Convert.ToByte(sessions[5]));
            userSession.SetProperty(SessionKey.CURRENCY, sessions[6]);
            userSession.SetProperty(SessionKey.CURRENCYID, Convert.ToInt32(sessions[7]));
            userSession.SetProperty(SessionKey.ISSHOWCONSOLIDATEDREPORT, Convert.ToBoolean(sessions[8]));
            userSession.SetProperty(SessionKey.CustomerDatelimit, Convert.ToDateTime(sessions[9]));
            userSession.SetProperty(SessionKey.ISADMINCANVIEWVNIP, Convert.ToBoolean(sessions[10]));
            userSession.SetProperty(SessionKey.USERLOGINIP, sessions[12]);
            userSession.SetProperty(SessionKey.LoginIP_Browser, sessions[16]);
            userSession.SetProperty(SessionKey.ASSETAPPPATHSESSIONKEY, Session.IsCookieless ? $"{sessions[15]}/assets" : sessions[17]);

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

            var enableJackpotGames = sessions[18].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            userSession.SetProperty(SessionKey.GameEnable + SessionKey.Jackpot, enableJackpotGames);

            userSession.SetProperty(SessionKey.ISCASINOSYNC, Convert.ToBoolean(sessions[19]));
            userSession.SetSyncedProduct(SessionKey.RNGCasino, Convert.ToBoolean(sessions[19]));

            userSession.SetProperty(SessionKey.ADMINID, Convert.ToInt32(sessions[26]));
            userSession.SetProperty(SessionKey.ADMINNAME, sessions[27]);

            userSession.SetProperty(SessionKey.IndirectCustIdSessionKey, Convert.ToInt32(sessions[28]));
            userSession.SetProperty(SessionKey.IndirectSubAccIdSessionKey, Convert.ToInt32(sessions[29]));
            userSession.SetProperty(SessionKey.IndirectUsernameSessionKey, sessions[30]);
            userSession.SetProperty(SessionKey.AUTOFOLLOWBETISDUMMYAGENT, Convert.ToBoolean(sessions[34]));
            userSession.SetProperty(SessionKey.MainSessionID, sessions[35]);

            userSession.SetProperty(SessionKey.BetlistDomainSessionKey, sessions[36]);
            userSession.SetProperty(SessionKey.ReportDomainSessionKey, sessions[38]);

            SetRealtimeValues(parameters, userSession);
            UpdateReportRoutingRealtime(parameters, userSession);
            SetUMFilter();
        }

        private static void UpdateReportRoutingRealtime(string[] parameters, IUserSession session)
        {
            var sessions = parameters[1].Split(';');
            var routing = sessions[25];
            if (!string.Equals(routing, session.GetProperty<string>(SessionKey.ReportRouting) ?? string.Empty, StringComparison.OrdinalIgnoreCase))
            {
                session.SetProperty(SessionKey.ReportRouting, routing);
            }
        }

        private static void SetOutputValuesToSession(string[] parameters, IUserSession session)
        {
            var outputs = JsonConvert.DeserializeObject<Hashtable>(parameters[2]);
            try
            {
                UserSessionService.SetUserSessionValues(session, outputs, (userSession, values) => { });
            }
            catch (Exception ex) { ExceptionHandler.Handle(new Exception(ex.Message, ex), TraceEventType.Warning); }
        }

        private ActionResult RedirectToSpecificUrl()
        {
            if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
            {
                return Redirect($"~/{SiteSettings.ReportRouting}{Request["ReturnUrl"].Replace("/site-reports", string.Empty)}");
            }

            return Content(Session.IsCookieless ? Session.SessionID : OK, "text/html", Encoding.UTF8);
        }

        private bool IsLoggedIn()
        {
            return Request.IsAuthenticated;
        }

        private void SetUMFilter()
        {
            if (!GlobalFilters.Filters.Any(filter => filter.Instance is CheckUMFilterAttribute))
            {
                GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                    new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));
            }
        }
    }
}