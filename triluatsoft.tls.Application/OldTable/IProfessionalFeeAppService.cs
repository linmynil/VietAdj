using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public interface IProfessionalFeeAppService : IApplicationService
    {
        List<ProfessionalFeeView> GetList(string ClaimID, int TimeSheetID);
        List<ProfessionalFeeView> GetListProfeeofTimesheet(int TimeSheetID);
        List<ProfessionalFeeView> GetListProfeeofTimesheetByEmp(int TimeSheetID, int EmployeeID);
    }
}
