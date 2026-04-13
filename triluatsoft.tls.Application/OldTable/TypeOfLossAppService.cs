using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class TypeOfLossAppService : tlsAppServiceBase, ITypeOfLossAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public TypeOfLossAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<TypeOfLoss> GetAll()
        {
            List<TypeOfLoss> list = _sqlExecuter.GetDatabase().SqlQuery<TypeOfLoss>("Select * from TypeOfLoss order by name").ToList();
            return list;
        }

        public int Create(string name)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO TypeOfLoss VALUES(@p0)", name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM TypeOfLoss WHERE ID = @p0", ID);
        }
        public int Update(int ID, string name)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE TypeOfLoss SET Name=@p0 WHERE ID = @p1", name, ID);
        }

        public int CreateOrUpdate(CreateOrUpdateTypeOfLossInput input)
        {
            if (input.ID.HasValue)
            {
                return Update(input.ID.Value, input.Name);
            }
            else
            {
                return Create(input.Name);
            }
        }
    }
}
