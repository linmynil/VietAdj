using Abp.Application.Services;
using triluatsoft.tls.Dto;
using triluatsoft.tls.Logging.Dto;

namespace triluatsoft.tls.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
