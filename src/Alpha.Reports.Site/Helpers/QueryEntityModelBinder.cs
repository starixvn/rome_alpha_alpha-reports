namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Reports.Models;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

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

                if (!string.IsNullOrWhiteSpace(breadcrumbLevelJson))
                {
                    try
                    {
                        reportBaseQuery.BreadcrumbLevel = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(breadcrumbLevelJson);
                    }
                    catch (JsonReaderException)
                    {
                        //// TODO: temporary
                        ////LogWarning(ex);
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
        {
            return controllerContext.RequestContext.HttpContext.Request["BreadcrumbLevelJson"];
        }

        protected virtual void LogWarning(JsonReaderException ex)
        {
            Logs.WriteWarn(ex, UserSession.Current.CustId);
        }
    }
}