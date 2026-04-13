using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface IEmployeeIncomeAppService:IApplicationService
    {
        List<EmployeeIncomeView> GetById(int? id);
        List<CIncomeTypeView> GetAllIncomeType();
        List<EmployeeIncomeView> Search(EmployeeIncomeSearchOption opts);
        string DeleteEmployeeIncome(int id);
        string SaveEmployeeIncome(EmployeeIncomeView input);
        void ImportIncomeExcel(string fileName);
    }
}
