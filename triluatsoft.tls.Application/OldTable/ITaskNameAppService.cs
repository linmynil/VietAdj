using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface ITaskNameAppService : IApplicationService
    {
        List<TaskNameDBObj> GetAll();
        int Delete(int ID);
        int CreateOrUpdate(CreateOrUpdateTaskNameInput input);
    }
}
