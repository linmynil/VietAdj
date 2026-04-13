using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Configuration.Host.Dto;

namespace triluatsoft.tls.ClaimManagement
{
    public interface ITasMailSettingAppService: IApplicationService
    {
        EmailSettingsEditDto GetEmailSettings();
    }
}
