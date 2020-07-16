namespace Alpha.Reports.SpamBlocker.Web
{
    using Alpha.Reports.SpamBlocker.Configurations;
    using Alpha.Reports.SpamBlocker.Models;
    using Fanex.Caching;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class ThrottlingFilterAttribute : ActionFilterAttribute
    {
        public ThrottlingFilterAttribute()
        {
            var configuration = SpamBlockerConfiguration.Current;
            Enabled = configuration.Enabled;
            Policy = new ThrottlePolicy
            {
                PerSeconds = configuration.PerSeconds,
                MaxRequests = configuration.MaxRequests
            };
            CacheService = new CacheServiceImpl();
        }

        public ThrottlingFilterAttribute(ThrottlePolicy policy, CacheService cacheService)
        {
            Policy = policy;
            CacheService = cacheService;
        }

        public bool Enabled { get; private set; } = true;

        public CacheService CacheService { get; private set; }

        public ThrottlePolicy Policy { get; private set; }

        public IRequestIdentity Identity { get; private set; } = new UserIdentity();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Enabled)
            {
                ProcessRequest(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void ProcessRequest(ActionExecutingContext filterContext)
        {
            var applyingPolicy = GetApplyingPolicy(filterContext);
            if (applyingPolicy != null)
            {
                ProcessRequest(filterContext, applyingPolicy);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void ProcessRequest(ActionExecutingContext filterContext, ThrottlePolicy policy)
        {
            var id = $"SpamBlocker_{Identity.GetKey(filterContext)}";
            var throttleCounter = CacheService.Get<ThrottleCounter>(id);
            if (throttleCounter == null || DateTime.Now > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.PerSeconds))
            {
                throttleCounter = new ThrottleCounter
                {
                    Timestamp = DateTime.Now,
                    TotalRequests = 1
                };
            }
            else
            {
                throttleCounter = new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };
            }

            if (throttleCounter.TotalRequests > policy.MaxRequests)
            {
                WriteBlockedResponse(filterContext);
                return;
            }

            CacheService.Set(
                id,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.PerSeconds)
                });
            base.OnActionExecuting(filterContext);
        }

        private void WriteBlockedResponse(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                {"controller", "Error" },
                {"action", "Blocked" }
            });
        }

        private ThrottlePolicy GetApplyingPolicy(ActionExecutingContext filterContext)
        {
            ThrottlePolicy policy = Policy;

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                var attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                policy = attr.Policy;
            }

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DisableThrottlingAttribute), true))
            {
                policy = null;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(EnableThrottlingAttribute), true))
            {
                var attr = (EnableThrottlingAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(EnableThrottlingAttribute), true).First();
                policy = attr.Policy;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableThrottlingAttribute), true))
            {
                policy = null;
            }

            return policy;
        }
    }
}