using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class ClaimTypeAppService : tlsAppServiceBase, IClaimTypeAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ClaimTypeAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<ClaimType> GetAll()
        {
            List<ClaimType> list = _sqlExecuter.GetDatabase().SqlQuery<ClaimType>("Select * from ClaimType order by code ").ToList();
            return list;
        }
        private int Create(ClaimType input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO ClaimType(Code, Name) VALUES(@p0, @p1)", input.Code, input.Name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM ClaimType WHERE ID = @p0", ID);
        }
        private int Update(ClaimType input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE ClaimType SET Code = @p0, Name=@p1 WHERE ID = @p2",input.Code, input.Name, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateClaimTypeInput input)
        {

            if (input.ID.HasValue)
            {
                ClaimType s = new ClaimType() { ID = input.ID.Value, Name = input.Name, Code = input.Code };
                return Update(s);
            }
            else
            {
                ClaimType s = new ClaimType() { Name = input.Name, Code = input.Code};
                return Create(s);
            }
        }
    }
}
