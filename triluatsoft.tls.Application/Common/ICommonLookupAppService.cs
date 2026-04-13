using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Common.Dto;

namespace triluatsoft.tls.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<ComboboxItemDto>> GetEditionsForCombobox();

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        string GetDefaultEditionName();
    }
}