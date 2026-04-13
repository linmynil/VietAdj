using Abp.Application.Services;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Authorization.Permissions.Dto;

namespace triluatsoft.tls.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
