using Abp.Application.Services;
using triluatsoft.tls.OldTable.dto;
using System.Collections.Generic;

namespace triluatsoft.tls.OldTable
{
    public interface IClaimProcessAppService: IApplicationService
    {
        List<ClaimProcessDto> GetAll(string ClaimID);
        List<string> GetProcessClaim();
        string UpdateClaimProcess(int ClaimProcessID, int UpdateStatus);
        System.Threading.Tasks.Task SendReminder();
    }
}
