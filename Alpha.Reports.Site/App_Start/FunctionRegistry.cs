namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using Sunplus.Agent.Model.Entities.Permission;

    public class FunctionRegistry
    {
        public static readonly Dictionary<string, Func<int, bool>> ProductPermissions = new Dictionary<string, Func<int, bool>>
        {
            [string.Empty] = (productId) => true,
            ["IsShowProduct"] = (productId) => UserPermission.IsShowProduct(productId),
            ["EnableAndSyncedPermission"] = (productId) =>
            {
                return UserPermission.IsEnableProduct(productId)
                    && UserPermission.IsSyncedProduct(productId);
            }
        };
    }
}
