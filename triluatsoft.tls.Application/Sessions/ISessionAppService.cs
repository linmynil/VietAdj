using System.Threading.Tasks;
using Abp.Application.Services;
using triluatsoft.tls.Sessions.Dto;

namespace triluatsoft.tls.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
