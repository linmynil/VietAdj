using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.OldTable
{
    public class CauseAppService : tlsAppServiceBase, ICauseAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public CauseAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<CauseDbObj> GetAll()
        {
            List<CauseDbObj> causes = _sqlExecuter.GetDatabase().SqlQuery<CauseDbObj>("Select * from Cause").OrderBy(x => x.Name).ToList();
            return causes;
        }

        public int Create(string name)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO Cause VALUES(@p0)", name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM Cause WHERE ID = @p0", ID);
        }
        public int Update(int ID, string name)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE Cause SET Name=@p0 WHERE ID = @p1", name, ID);
        }
    }
}
