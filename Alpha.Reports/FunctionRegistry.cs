namespace Alpha.Reports
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Module.Keno.KenoSetting.Configurations;
    using Sunplus.Agent.Model.Entities.Permission;

    public class FunctionRegistry
    {
        public static readonly Dictionary<string, Func<int, bool>> ProductPermission = new Dictionary<string, Func<int, bool>>
        {
            [string.Empty] = (productId) => true,
            ["RacingPermission"] = (productId) => UserPermission.IsShowRacing,
            ["NumberGamePermission"] = (productId) => UserPermission.IsShowNumberGame,
            ["LiveCasinoPermission"] = (productId) => UserPermission.IsShowLiveCasino,
            ["VirtualSportsPermission"] = (productId) => UserPermission.IsShowVirtualSports,
            ["KenoPermission"] = (productId) =>
            {
                return UserPermission.IsShowKenoSetting
                    && KenoSection.Instance.ShowSetting;
            },
            ["GoldDeluxePermission"] = (productId) => UserPermission.IsShowGoldDeluxeSettingForCredit,
            ["EnableAndSyncedPermission"] = (productId) =>
            {
                return UserPermission.IsEnableProduct(productId)
                    && UserPermission.IsSyncedProduct(productId);
            }
        };
    }
}
