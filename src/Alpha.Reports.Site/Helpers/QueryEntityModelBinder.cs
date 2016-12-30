namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Web.Mvc;
    using Reports.Models;
    using Requests;

    public class QueryEntityModelBinder : DefaultModelBinder
    {
        private static readonly Type[] IUserType = new[] { typeof(IUser) };
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
                return Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<IUser>());
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}