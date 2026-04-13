using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class ExpenseTypeAppService : tlsAppServiceBase, IExpenseTypeAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ExpenseTypeAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<ExpenseTypeDBObj> GetAll()
        {
            List<ExpenseTypeDBObj> list = _sqlExecuter.GetDatabase().SqlQuery<ExpenseTypeDBObj>("Select * from ExpenseType").ToList();
            return list;
        }
        private int Create(ExpenseTypeDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO ExpenseType(Name) VALUES(@p0)", input.Name);
        }

        public int Delete(int ID)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("DELETE FROM ExpenseType WHERE ID = @p0", ID);
        }
        private int Update(ExpenseTypeDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE ExpenseType SET Name=@p0 WHERE ID = @p1", input.Name, input.ID);
        }
        public int CreateOrUpdate(CreateOrUpdateExpenseTypeInput input)
        {
            
            if (input.ID.HasValue)
            {
                ExpenseTypeDBObj s = new ExpenseTypeDBObj() { ID = input.ID.Value, Name = input.Name };
                return Update(s);
            }
            else
            {
                ExpenseTypeDBObj s = new ExpenseTypeDBObj() {Name = input.Name };
                return Create(s);
            }
        }
    }
}
