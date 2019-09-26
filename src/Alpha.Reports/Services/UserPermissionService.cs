namespace Alpha.Reports.Services
{
    using Sunplus.Agent.Model.Entities.Permission;

    public interface UserPermissionService
    {
        bool IsEnableProduct(int product);
    }

    public class UserPermissionServiceImpl : UserPermissionService
    {
        public bool IsEnableProduct(int product) => UserPermission.IsEnableProduct(product);
    }
}
