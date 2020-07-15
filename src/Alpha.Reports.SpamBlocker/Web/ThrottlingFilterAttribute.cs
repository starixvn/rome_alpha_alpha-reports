namespace Alpha.Reports.SpamBlocker.Web
{
    using Alpha.Reports.SpamBlocker.Configurations;
    using Alpha.Reports.SpamBlocker.Models;
    using Alpha.Reports.SpamBlocker.Repositories;
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
            CacheRepository = new FanexCacheRepository();
        }

        public ThrottlingFilterAttribute(ThrottlePolicy policy, ICacheRepository cacheRepository)
        {
            Policy = policy;
            CacheRepository = cacheRepository;
        }

        public bool Enabled { get; private set; } = true;

        public ICacheRepository CacheRepository { get; private set; }

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
            var throttleCounter = CacheRepository.FirstOrDefault(id);
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

            CacheRepository.Save(id, throttleCounter, TimeSpan.FromSeconds(policy.PerSeconds));
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