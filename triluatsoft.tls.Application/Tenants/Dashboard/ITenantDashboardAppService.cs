using Abp.Application.Services;
using triluatsoft.tls.Tenants.Dashboard.Dto;

namespace triluatsoft.tls.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
