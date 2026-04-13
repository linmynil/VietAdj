using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.View;

namespace triluatsoft.tls.OldTable
{
    public class ClaimBackgroundJobs : tlsAppServiceBase, IClaimBackgroundJobs
    {
        private readonly IClaimProcessAppService _claimProcessAppService;
        public ClaimBackgroundJobs(IClaimProcessAppService claimProcessAppService)
        {
            _claimProcessAppService = claimProcessAppService;
        }
        public async System.Threading.Tasks.Task RunAsync()
        {
            DateTime date = DateTime.Now;
            await _claimProcessAppService.SendReminder();
        }
    }
}
