namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Logging.Extensions.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Reports.Models;
    using Requests;

    public class QueryEntityModelBinder : DefaultModelBinder
    {
        private static readonly Type[] IUserType = { typeof(IUser) };
        private static readonly Type ReportBaseQueryType = typeof(ReportBaseQuery);

        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType != null
                && ReportBaseQueryType.IsAssignableFrom(modelType)
                && modelType.GetConstructor(IUserType) != null)
            {
                return CreateInstanceOfAssignableReportBaseQuery(modelType);
            }

            return CallBaseCreateModel(controllerContext, bindingContext, modelType);
        }

        protected virtual object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
            => Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<IUser>());

        protected virtual object CallBaseCreateModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext,
           Type modelType) => base.CreateModel(controllerContext, bindingContext, modelType);

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "ex", Justification = "Reviewed")]
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var boundModel = CallBaseBindModel(controllerContext, bindingContext);

            var reportBaseQuery = boundModel as ReportBaseQuery;
            if (reportBaseQuery != null)
            {
                var breadcrumbLevelJson = GetBreadcrumbLevelJson(controllerContext);

                if (!string.IsNullOrWhiteSpace(breadcrumbLevelJson)
                   && breadcrumbLevelJson.LastIndexOf(",[", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    try
                    {
                        reportBaseQuery.BreadcrumbLevel = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(
                            breadcrumbLevelJson,
                            new JsonSerializerSettings
                            {
                                Error = HandleDeserializationError
                            });
                    }
                    catch (JsonException ex)
                    {
                        //// TODO: temporary
                        LogWarning(ex);
                    }
                }

                return reportBaseQuery;
            }

            return boundModel;
        }

        protected virtual object CallBaseBindModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext)
            => base.BindModel(controllerContext, bindingContext);

        protected virtual string GetBreadcrumbLevelJson(ControllerContext controllerContext)
            => controllerContext.RequestContext.HttpContext.Request["BreadcrumbLevelJson"];

        protected virtual void LogWarning(JsonException ex)
        {
            ExceptionHandler.Handle(ex, TraceEventType.Information);
        }

        private static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            if (errorArgs.ErrorContext.Error.Message.Contains(".CustLevelId'"))
            {
                errorArgs.ErrorContext.Handled = true;
            }
        }
    }
}