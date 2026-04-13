using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public interface IExpenseAppService: IApplicationService
    {
        List<ExpenseView> GetListExpensesOfTimeSheet(int TimeSheetID);
    }
}
