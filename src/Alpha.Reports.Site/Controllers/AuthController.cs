using Alpha.NetThrottling.Mvc;
using Alpha.Reports.Configurations;
using Alpha.Reports.Site.Constants;
using Alpha.Web.Filters;
using AlphaPlus.User.Models;
using Fanex.Cryptography.Aes;
using ONEbook.UM.AspNetMVC.Filters;
using System;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace Alpha.Reports.Site.Controllers
{
    [SessionState(SessionStateBehavior.Required), AllowProtectedMode, AllowAnonymous, SkipUMFilter]
    public class AuthController : Controller
    {
        private const string SecretKey = "!@#$";
        private const string OK = "OK";

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

            try
            {
                var parameters = parameter.Split('&');
                if (IsLoggedIn()
                    && !string.IsNullOrWhiteSpace(session.UserName)
                    && session.UserName.Equals(parameters[0], StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToSpecificUrl();
                }

                SetUserSessionsAndAuth(parameters);
            }
            catch { }

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
                || Request.Headers["Referer"].IndexOf(Request.Headers["Host"], StringComparison.OrdinalIgnoreCase) != -1
                || Request.Headers["Referer"].IndexOf(session.GetProperty<string>(SessionKey.MainDomainSessionKey) ?? string.Empty) != -1)
            {
                return true;
            }

            return false;
        }

        private void SetUserSessionsAndAuth(string[] parameters)
        {
            FormsAuthentication.SetAuthCookie(parameters[0], false);
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
    }
}