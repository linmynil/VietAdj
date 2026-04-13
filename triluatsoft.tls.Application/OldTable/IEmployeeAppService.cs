using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.Authorization.Users.Dto;
using triluatsoft.tls.Dto;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.StoreProcDto;

namespace triluatsoft.tls.OldTable
{
    public interface IEmployeeAppService : IApplicationService
    {
        List<UserListDto> GetEmployeesForClaim();
        List<UserListDto> GetEmployeesForCRS();
        List<UserListDto> GetEmployeesForActualFee();

        /// <summary>
        /// fee per hour
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        decimal? GetProFee(int empId);
        List<CalcUserContribution> GetUserContributionInfo(GetUserContributionInput input);
        List<CalcContribution> GetContributionInfo(GetUserContributionInput input);
        FileDto ExportUserContribution(GetUserContributionInput input);
        FileDto ExportContribution(GetUserContributionInput input);
        List<UserIncome> GetUserIncomeInfo(GetUserContributionInput input);
        FileDto ExportUserIncome(GetUserContributionInput input);
        List<EmployeeView> GetEmployeeListByTS(int TimeSheetID);
        List<EmployeeView> GetEmployeeListIncome();
        List<EmployeeView> GetChargedEmployeeListByTS(int TimeSheetID);
        EmployeeView GetEmpInfo(int empID);
        EmployeeView GetUserInfo(int empID);
        List<EmployeeView> GetEmployeeListByClaim(string claimID);
        List<EmployeeView> GetEmployeeList();
        int CreateEmployee(Employee input);
        Employee UpdateEmployee(Employee input);
    }
}
