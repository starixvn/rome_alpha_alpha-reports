namespace Alpha.Reports.Services
{
    using Sunplus.Agent.Model.Entities.Permission;

    public interface UserPermissionService
    {
        bool IsShowMyanmarOdds();

        bool IsIMbcCustomer();

        bool IsEnableProduct(int product);
    }

    public class UserPermissionServiceImpl : UserPermissionService
    {
        public bool IsIMbcCustomer()
        {
            return UserPermission.IsIMBCCustomer;
        }

        public bool IsShowMyanmarOdds()
        {
            return UserPermission.IsShowMyanmarOdds;
        }

        public bool IsEnableProduct(int product)
        {
            return UserPermission.IsEnableProduct(product);
        }
    }
}
