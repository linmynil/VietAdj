using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace triluatsoft.tls.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
