using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class TaskNameAppService : tlsAppServiceBase, ITaskNameAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public TaskNameAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<TaskNameDBObj> GetAll()
        {
            List<TaskNameDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<TaskNameDBObj>("Select * from TaskName").ToList();
            return list;
        }
        private int Create(TaskNameDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO TaskName(Name, JobCode, StandardTime) VALUES(@p0, @p1, @p2)", input.Name, input.JobCode, input.StandardTime);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM TaskName WHERE ID = @p0", ID);
        }
        private int Update(TaskNameDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE TaskName SET Name=@p0, StandardTime = @p1, JobCode = @p2 WHERE ID = @p3", input.Name, input.StandardTime, input.JobCode, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateTaskNameInput input)
        {

            if (input.ID.HasValue)
            {
                TaskNameDBObj s = new TaskNameDBObj() { ID = input.ID.Value, Name = input.Name, JobCode = input.JobCode, StandardTime = input.StandardTime };
                return Update(s);
            }
            else
            {
                TaskNameDBObj s = new TaskNameDBObj() { Name = input.Name, JobCode = input.JobCode, StandardTime = input.StandardTime };
                return Create(s);
            }
        }
    }
}
