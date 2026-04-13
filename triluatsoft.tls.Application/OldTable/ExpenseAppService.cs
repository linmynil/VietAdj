using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public class ExpenseAppService : tlsAppServiceBase, IExpenseAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ExpenseAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<ExpenseView> GetListExpensesOfTimeSheet(int TimeSheetID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            
                var query = context.Expenses.Where(e => e.TimeSheetID == TimeSheetID)
                                       .Select(e => new ExpenseView
                                       {
                                           ExpenseID = e.Id,
                                           Description = e.Description,
                                           Notes = e.Notes,
                                           RefNbr = e.RefNbr,
                                           Amount = e.Amount,
                                           ExpenseTypeID = e.ExpenseTypeID,
                                           ExpenseTypeName = e.ExpenseType.Name,
                                           InputDate = e.InputDate,
                                           CreatedBy = e.CreatedBy,
                                           CreatedDate = e.CreatedDate,
                                           UpdatedBy = e.UpdatedBy,
                                           UpdatedDate = e.UpdatedDate
                                       });
                return query.OrderByDescending(e => e.InputDate).ToList(); //hvtam-08082015 query.ToList();
            
        }
    }
}
