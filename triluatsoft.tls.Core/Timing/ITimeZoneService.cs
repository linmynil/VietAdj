using System.Threading.Tasks;
using Abp.Configuration;

namespace triluatsoft.tls.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
