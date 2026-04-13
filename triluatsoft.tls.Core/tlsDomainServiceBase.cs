using Abp.Domain.Services;

namespace triluatsoft.tls
{
    public abstract class tlsDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected tlsDomainServiceBase()
        {
            LocalizationSourceName = tlsConsts.LocalizationSourceName;
        }
    }
}
