using System.Threading.Tasks;
using Abp.Application.Services;
using triluatsoft.tls.Configuration.Host.Dto;

namespace triluatsoft.tls.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
        string SendCheckEmail(SendTestEmailInput input);
    }
}
