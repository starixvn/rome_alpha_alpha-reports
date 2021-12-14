using Alpha.Auth.Services;
using Alpha.Auth.Services.Impl;
using Alpha.Utilities.IP;
using Fanex.Cryptography.Aes;
using Fanex.Logging.Extensions.Web;
using RSACryptoPad;
using Sunplus.Agent.Model.Common;
using Sunplus.Agent.Model.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using UAParser;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class AuthController : Controller
    {
        private readonly IAesService aesService;
        private readonly IPService ipService;
        private readonly IAuthorizationService authorizationService = new AuthorizationService();

        public AuthController(IAesService aesService ,IPService ipService)
        {
            this.aesService = aesService;
            this.ipService = ipService;
        }

        // GET: Auth
        public ActionResult Index(string param)
        {
            IUserSession session = UserSession.Current;
            if (session.IsLogged)
            {
                return Json(Session.SessionID, JsonRequestBehavior.AllowGet);
            }

            const int dwKeySize = 1024;
            var keyPath = Server.MapPath("~/IbcKeys/ibckey.snk");
            var parameter = param + string.Empty;
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
            DateTime loginTime;
            string language, password = null, nickname;
            string browserSize = null;
            int custId = 0, subAccId = 0;
            long timeTicks = DateTime.Now.Ticks;
            bool isnickname = false;

            if( ps == null
                || ps.Length < 10
                || !int.TryParse(ps[0], out custId)
                || !int.TryParse(ps[1], out subAccId)){
                return new EmptyResult();
            }

            language = ps[7];
            nickname = ps[4];
            bool.TryParse(ps[5], out isnickname);
            try
            {
                password = aesService.Decrypt(ps[3], ps[11]);
            }
            catch
            {
                var message = string.Format("Cannot decrypt password. Encrypted: {0}, Code: {1}", ps[3], ps[11]);
                ExceptionHandler.Handle(new Exception(message), TraceEventType.Warning);
            }

            UserSession.Current.Language = language;
            UserSession.Current.LangKey = Language.LangKeyFromCulture(language);
            loginTime = new DateTime(timeTicks);
            const string DeviceIdSessionKey = "ProcessAfterLogin_Handler.DeviceIdSessionKey";
            HttpContext.Session[DeviceIdSessionKey] = (string.IsNullOrWhiteSpace(ps[9]) ? "N/A" : ps[9]);

            if (ps.Length >= 11)
            {
                browserSize = Fanex.Common.Converter.ToString2(ps[10]);
            }

            Hashtable requestInfo = GetRequestInfo(HttpContext.Request, browserSize);
            requestInfo["countrycode"] = ipService.GetIPInformation(AgentIPFormat.GetClientIP()).CountryShort;

            bool isSuccess = false;
            int errCode = 0;
            string errMsg = string.Empty;
            try
            {
                isSuccess = authorizationService.DirectLoginVerification(
                    custId,
                    password,
                    subAccId,
                    nickname,
                    isnickname,
                    loginTime,
                    requestInfo,
                    out errCode,
                    out errMsg,
                    ref session,
                    new UserSessionServiceImpl().SetUserStatusToSession);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex, string.IsNullOrWhiteSpace(password) ? TraceEventType.Warning : TraceEventType.Error);
            }

            if (isSuccess)
            {
                FormsAuthentication.SetAuthCookie(session.UserName, false);

                return Json(Session.SessionID, JsonRequestBehavior.AllowGet);
            }

            return new EmptyResult();
        }

        private static Hashtable GetRequestInfo(HttpRequestBase request, string browserSize)
        {
            // Get Browser
            Hashtable requestInfo = new Hashtable();

            requestInfo.Add("loginip", string.Empty);
            requestInfo.Add("countrycode", string.Empty);
            requestInfo.Add("logindomain", string.Empty);

            requestInfo.Add("browsername", string.Empty);
            requestInfo.Add("platform", string.Empty);

            string ip = AgentIPFormat.GetClientIP();
            string country = "Unknown";

            requestInfo["loginip"] = ip;
            requestInfo["countrycode"] = country;
            requestInfo["logindomain"] = request.ServerVariables["SERVER_NAME"];

            try
            {
                GetRequestInfoFromContext(request, browserSize, requestInfo);
            }
            catch
            {
            }

            return requestInfo;
        }

        private static void GetRequestInfoFromContext(HttpRequestBase request, string browserSize, Hashtable requestInfo)
        {
            var userAgent = request.UserAgent ?? string.Empty;
            var userAgentParser = Parser.GetDefault();
            var clientInfo = userAgentParser.Parse(userAgent);
            var clientUserAgent = clientInfo.UserAgent;
            var clientPatch = clientUserAgent.Patch ?? "0";

            var browser = $"{clientUserAgent.Family} {clientUserAgent.Major}.{clientUserAgent.Minor}.{clientPatch}";
            if (!string.IsNullOrWhiteSpace(browserSize))
            {
                browser += $", {browserSize}";
            }

            var device = $"{clientInfo.Device.Family}, IsCrawler: {clientInfo.Device.IsSpider}";

            var majorVersion = clientInfo.OS.Major ?? "0";
            var minorVersion = clientInfo.OS.Minor ?? "0";
            var osVersion = clientInfo.OS.Patch ?? "0";
            var operateSystem = $"{clientInfo.OS.Family} {majorVersion}.{minorVersion}.{osVersion}";

            requestInfo["browsername"] = browser;
            requestInfo["platform"] = $"{operateSystem}, Device: {device}";
        }
    }

    class UserSessionServiceImpl
    {
        public void SetUserStatusToSession(IUserSession session, Hashtable userData)
        {
        }
    }
}