namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Web.Mvc;
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
        {
            return Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<IUser>());
        }

        protected virtual object CallBaseCreateModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext,
           Type modelType)
        {
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}