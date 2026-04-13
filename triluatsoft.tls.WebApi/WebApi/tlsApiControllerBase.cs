using Abp.WebApi.Controllers;

namespace triluatsoft.tls.WebApi
{
    public abstract class tlsApiControllerBase : AbpApiController
    {
        protected tlsApiControllerBase()
        {
            LocalizationSourceName = tlsConsts.LocalizationSourceName;
        }
    }
}