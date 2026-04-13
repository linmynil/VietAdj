using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Auditing.Dto;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}