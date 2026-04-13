using Abp;

namespace triluatsoft.tls
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="tlsDomainServiceBase"/>.
    /// For application services inherit tlsAppServiceBase.
    /// </summary>
    public abstract class tlsServiceBase : AbpServiceBase
    {
        protected tlsServiceBase()
        {
            LocalizationSourceName = tlsConsts.LocalizationSourceName;
        }
    }
}