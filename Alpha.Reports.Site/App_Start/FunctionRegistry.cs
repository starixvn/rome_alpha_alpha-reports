namespace Alpha.Reports.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class FunctionRegistry
    {
        public static readonly Dictionary<string, Func<int, bool>> ProductPermissions = new Dictionary<string, Func<int, bool>>
        {
            [string.Empty] = (productId) => true,
            ["RacingPermission"] = (productId) => UserPermission.IsShowRacing,
            ["NumberGamePermission"] = (productId) => UserPermission.IsShowNumberGame,
            ["LiveCasinoPermission"] = (productId) => UserPermission.IsShowLiveCasino,
            ["VirtualSportsPermission"] = (productId) => UserPermission.IsShowVirtualSports,
            ["RNGCasinoPermission"] = (productId) => UserPermission.IsShowCasino,
            ["KenoPermission"] = (productId) =>
            {
                return AppSettings.IsShowKenoSetting && !UserSession.Current.IsHideKeno;
            },
            ["GoldDeluxePermission"] = (productId) => UserPermission.IsShowGoldDeluxeSettingForCredit,
            ["ColossusBetsPermission"] = (productId) => UserPermission.IsShowColossusBets,
            ["EnableAndSyncedPermission"] = (productId) =>
            {
                return UserPermission.IsEnableProduct(productId)
                    && UserPermission.IsSyncedProduct(productId);
            }
        };
    }
}
