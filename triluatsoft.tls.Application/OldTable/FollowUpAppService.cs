using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class FollowUpAppService : tlsAppServiceBase, IFollowUpAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public FollowUpAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<FollowUpDBObj> GetAll()
        {
            List<FollowUpDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<FollowUpDBObj>("Select * from FollowUp").ToList();
            return list;
        }
        private int Create(FollowUpDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO FollowUp(Name, Code) VALUES(@p0, @p1)", input.Name, input.Code);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM FollowUp WHERE ID = @p0", ID);
        }
        private int Update(FollowUpDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE FollowUp SET Name=@p0 WHERE ID = @p1", input.Name, input.ID);
        }

        public int CreateOrUpdate(CreateOrUpdateFollowUpInput input)
        {
            FollowUpDBObj s = new FollowUpDBObj() { ID = input.ID, Name = input.Name, Code = input.Code };

            FollowUpDBObj exist = _sqlExecuter.GetDatabase().SqlQuery<FollowUpDBObj>("Select * from FollowUp Where ID = @p0", input.ID).FirstOrDefault();

            if (exist != null)
            {
                return Update(s);
            }
            else
            {
                return Create(s);
            }
        }
    }
}
