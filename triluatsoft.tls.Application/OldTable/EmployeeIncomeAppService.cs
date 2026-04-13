using Abp.Domain.Repositories;
using Abp.IO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public class EmployeeIncomeAppService : tlsAppServiceBase, IEmployeeIncomeAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<EmployeeIncome> _employeeIncomeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        public EmployeeIncomeAppService(
            IRepository<EmployeeIncome> employeeIncomeRepository,
            ISqlExecuter sqlExecuter,
            IEmployeeAppService employeeAppService
            )
        {
            _employeeIncomeRepository = employeeIncomeRepository;
            _sqlExecuter = sqlExecuter;
            _employeeAppService = employeeAppService;
        }

        public string DeleteEmployeeIncome(int id)
        {
            EmployeeIncome employeeIncome = _employeeIncomeRepository.FirstOrDefault(id);
            _employeeIncomeRepository.Delete(employeeIncome);
            return "Ok";
        }

        public List<CIncomeTypeView> GetAllIncomeType()
        {
            var context = _sqlExecuter.GetTLSDBContext();
            var query = from c in context.CIncomeTypes
                        select new CIncomeTypeView
                        {
                            ID = c.Id,
                            IncomeName = c.IncomeName,
                        };
            return query.OrderBy(c => c.ID).ToList();
        }

        public List<EmployeeIncomeView> GetById(int? id)
        {

            var context = _sqlExecuter.GetTLSDBContext();
            var query = from emp in context.EmployeeIncomes
                        join c in context.CIncomeTypes on emp.IncomeType equals c.Id
                        join en in context.Employees on emp.EmployeeID equals en.Id
                        where emp.Id == id
                        select new EmployeeIncomeView
                        {
                            ID = emp.Id,
                            EmployeeID = emp.EmployeeID,
                            UserID = emp.UserID,
                            Date = emp.Date,
                            IncomeType = c.Id,
                            Contribution = emp.Contribution,
                            IncomeAMT = emp.IncomeAMT,
                            CreateBy = emp.CreateBy,
                            CreateDate = emp.CreateDate,
                            Description = emp.Description,
                            IncomeName = c.IncomeName,
                            EmployeeName = en.Name
                        };
            return query.ToList();
        }        

        public string SaveEmployeeIncome(EmployeeIncomeView input)
        {
            var u = UserManager.Users.Where(x => x.EmployeeId == input.EmployeeID).FirstOrDefault();
            EmployeeIncome employeeIncome;
            if (input.ID.HasValue)
            {                
                employeeIncome = _employeeIncomeRepository.FirstOrDefault(p => p.Id == input.ID);                
            }
            else
            {
                employeeIncome = new EmployeeIncome();
            }
            var currUser = GetCurrentUser();
            //if (!input.EmployeeID.HasValue)
            //    employeeIncome.EmployeeID = currUser.EmployeeId;
            //else
            //    employeeIncome.EmployeeID = input.EmployeeID.Value;            
            employeeIncome.EmployeeID = input.EmployeeID;
            employeeIncome.UserID = u.UserName;
            employeeIncome.Date = input.Date;
            employeeIncome.IncomeType = input.IncomeType;
            employeeIncome.Contribution = input.Contribution;
            employeeIncome.IncomeAMT = input.IncomeAMT;
            employeeIncome.Description = input.Description;
            if (!input.ID.HasValue)
            {
                employeeIncome.CreateDate = DateTime.Now;
                employeeIncome.CreateBy = (int)currUser.EmployeeId;
                _employeeIncomeRepository.InsertAndGetId(employeeIncome);
            }
            else
            {
                employeeIncome.UpdateDate = DateTime.Now;
                employeeIncome.Updateby = (int)currUser.EmployeeId;
                _employeeIncomeRepository.Update(employeeIncome);
            }
            return "ok";
        }

        public List<EmployeeIncomeView> Search(EmployeeIncomeSearchOption opts)
        {
            Logger.Debug("Employee: " + opts.EmployeeID + "; Contribution: " + opts.Contribution.ToString());
            var context = _sqlExecuter.GetTLSDBContext();

            var curr_user = GetCurrentUser();
            var curr_role = "";

            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == curr_user.Id
                             select new RoleView
                             {
                                 ID = r.Id,
                                 Name = r.Name,
                                 DisplayName = r.DisplayName
                             };
            var roles = role_query.ToList();
            foreach (var role in roles)
            {
                curr_role = role.Name;
            }

            var query = from e in context.EmployeeIncomes
                        join income in context.CIncomeTypes on e.IncomeType equals income.Id
                        join emp in context.Employees on e.EmployeeID equals emp.Id
                        where (opts.Contribution == null || e.Contribution == opts.Contribution)
                           && (opts.StartDate == null || e.Date >= opts.StartDate)
                           && (opts.EndDate == null || e.Date <= opts.EndDate)
                           && (opts.CIncomeTypeID == null || e.IncomeType == opts.CIncomeTypeID)
                           && (opts.EmployeeID == 0 || e.EmployeeID == opts.EmployeeID)
                           && (curr_role == "Admin" || e.EmployeeID == curr_user.EmployeeId)
                        select new EmployeeIncomeView
                        {
                            ID = e.Id,
                            EmployeeID = e.EmployeeID,
                            EmployeeName = emp.Name,
                            Date = e.Date,
                            CreateDate = e.CreateDate,
                            CreateBy = e.CreateBy,
                            IncomeType = e.IncomeType,
                            Contribution = e.Contribution,
                            Description = e.Description,
                            IncomeAMT = e.IncomeAMT,
                            IncomeName = income.IncomeName
                        };
            return query.ToList();
        }

        public void ImportIncomeExcel(string fileName)
        {
            var tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["uploaded-folder"], fileName);
            var pck = new OfficeOpenXml.ExcelPackage();
            using (var stream = File.OpenRead(tempFilePath))
            {
                pck.Load(stream);
                var workbook = pck.Workbook;
                var worksheet = workbook.Worksheets.First();
                int totalRow = worksheet.Dimension.End.Row;
                for (int i = 2; i <= totalRow; i++)
                {
                    var UserName = worksheet.Cells[i, 1].Text;
                    var user = UserManager.Users.FirstOrDefault(p => p.UserName == UserName);

                    if (user != null)
                    {
                        var date = worksheet.Cells[i, 2].Value;
                        double a = Convert.ToDouble(date);
                        var convertDate = DateTime.FromOADate(a);
                        var income = _employeeIncomeRepository.FirstOrDefault(p => p.EmployeeID == user.EmployeeId && p.Date == convertDate);
                        if (income == null)
                        {
                            var data = new EmployeeIncome();
                            data.EmployeeID = user.EmployeeId;
                            data.Date = convertDate;
                            data.IncomeType = Convert.ToInt32(worksheet.Cells[i, 3].Text);
                            data.Contribution = Convert.ToBoolean(worksheet.Cells[i, 4].Value);
                            data.IncomeAMT = Convert.ToDecimal(worksheet.Cells[i, 5].Value);
                            data.Description = worksheet.Cells[i, 6].Text;
                            _employeeIncomeRepository.Insert(data);
                        }

                    }


                }

            }
            FileHelper.DeleteIfExists(tempFilePath);
        }
    }
}
