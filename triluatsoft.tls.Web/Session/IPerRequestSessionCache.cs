using System.Threading.Tasks;
using triluatsoft.tls.Sessions.Dto;

namespace triluatsoft.tls.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
