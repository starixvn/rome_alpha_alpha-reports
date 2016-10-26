namespace Alpha.Reports.Site.App_Start
{
    using System;
    using Sunplus.Agent.Model.Entities.Permission;

    public class FunctionRegistry
    {
        public static readonly Func<int, bool> ProductPermissions =
            (productId) => UserPermission.HasPermissionOnProduct(productId);
    }
}
