namespace Alpha.Reports.Services
{
    using AlphaPlus.User.Models;

    public interface UserPermissionService
    {
        bool IsShowMyanmarOdds();

        bool IsEnableProduct(int product);
    }

    public class UserPermissionServiceImpl : UserPermissionService
    {
        public bool IsShowMyanmarOdds() => UserPermission.IsShowMyanmarOdds;

        public bool IsEnableProduct(int product) => UserPermission.IsEnableProduct(product);
    }
}
