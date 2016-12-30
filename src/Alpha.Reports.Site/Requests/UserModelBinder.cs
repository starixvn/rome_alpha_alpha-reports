namespace Alpha.Reports.Site.Requests
{
    using System;
    using System.Web.Mvc;
    using Reports.Models;

    public class UserModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType != null)
            {
                var constructors = modelType.GetConstructors();

                foreach (var constructor in constructors)
                {
                    foreach (var parameter in constructor.GetParameters())
                    {
                        if (parameter.ParameterType == typeof(User))
                        {
                            return Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<User>());
                        }
                    }
                }
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}