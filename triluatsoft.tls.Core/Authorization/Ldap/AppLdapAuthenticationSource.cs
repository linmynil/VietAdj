using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.MultiTenancy;

namespace triluatsoft.tls.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
