using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public interface IOldUserAppService : IApplicationService
    {
        System.Threading.Tasks.Task MigrateOldUsers();        
    }
}
