using Abp.Authorization;
using triluatsoft.tls.Authorization.Roles;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.MultiTenancy;

namespace triluatsoft.tls.Authorization
{
    /// <summary>
    /// Implements <see cref="PermissionChecker"/>.
    /// </summary>
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
