namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Newtonsoft.Json;
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

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var boundModel = base.BindModel(controllerContext, bindingContext);

            var reportBaseQuery = boundModel as ReportBaseQuery;
            if (reportBaseQuery != null
                    && !string.IsNullOrWhiteSpace(controllerContext.RequestContext.HttpContext.Request["BreadcrumbLevelJson"]))
            {
                var drillDownInfoText = controllerContext.RequestContext.HttpContext.Request["BreadcrumbLevelJson"];
                reportBaseQuery.BreadcrumbLevel = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(drillDownInfoText);

                return reportBaseQuery;
            }

            return boundModel;
        }
    }
}