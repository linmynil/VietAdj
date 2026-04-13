using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface ITaskAppService : IApplicationService
    {
        List<TaskView> GetAll(TaskSearchOptions option);
        string Save(CreateOrUpdateTaskInput input);
        TaskView GetById(int taskId);
        string DeleteById(int taskId);

        List<TaskView> GetToDashboard();

        bool CheckPermission_CreateDeadline();
        bool CheckPermission_SearchDeadline();
        bool CheckPermission_EditDeadline();
        bool CheckPermission_DeleteDeadline();
    }
}
