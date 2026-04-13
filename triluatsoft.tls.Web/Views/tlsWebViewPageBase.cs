using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Views;

namespace triluatsoft.tls.Web.Views
{
    public abstract class tlsWebViewPageBase : tlsWebViewPageBase<dynamic>
    {
       
    }

    public abstract class tlsWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        public IAbpSession AbpSession { get; private set; }
        
        protected tlsWebViewPageBase()
        {
            AbpSession = IocManager.Instance.Resolve<IAbpSession>();
            LocalizationSourceName = tlsConsts.LocalizationSourceName;
        }
    }
}