namespace Alpha.Reports.Services
{
    using AlphaPlus.User.Models;

    public interface UserPermissionService
    {
        bool IsEnableProduct(int product);
    }

    public class UserPermissionServiceImpl : UserPermissionService
    {
        public bool IsEnableProduct(int product) => UserPermission.IsEnableProduct(product);
    }
}