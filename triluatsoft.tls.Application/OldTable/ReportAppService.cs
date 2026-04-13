using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class ReportAppService : tlsAppServiceBase, IReportAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ReportAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<ReportDbObj> GetAll()
        {
            List<ReportDbObj> list = _sqlExecuter.GetDatabase().SqlQuery<ReportDbObj>("Select * from Report").ToList();
            return list;
        }
        private int Create(ReportDbObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO Report(Name) VALUES(@p0)", input.Name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM Report WHERE ID = @p0", ID);
        }
        private int Update(ReportDbObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE Report SET Name=@p0 WHERE ID = @p1", input.Name, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateReportInput input)
        {

            if (input.ID.HasValue)
            {
                ReportDbObj s = new ReportDbObj() { ID = input.ID.Value, Name = input.Name };
                return Update(s);
            }
            else
            {
                ReportDbObj s = new ReportDbObj() { Name = input.Name };
                return Create(s);
            }
        }
    }
}
