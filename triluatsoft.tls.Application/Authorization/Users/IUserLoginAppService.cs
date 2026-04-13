using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Authorization.Users.Dto;

namespace triluatsoft.tls.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
