using System.Threading.Tasks;
using Abp.Application.Services;
using triluatsoft.tls.Configuration.Tenants.Dto;

namespace triluatsoft.tls.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
