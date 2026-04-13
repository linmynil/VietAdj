using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;

namespace triluatsoft.tls.Authorization.Users
{
    public interface IUserLogoutInformer
    {
        void InformClients(IReadOnlyList<IOnlineClient> clients);
    }
}
