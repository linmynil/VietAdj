using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.Authorization.Users.Dto;
using triluatsoft.tls.DataExporting;
using triluatsoft.tls.Dto;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.Net.MimeTypes;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.StoreProcDto;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.OldTable
{
    public class EmployeeAppService : tlsAppServiceBase, IEmployeeAppService
    {
        public IAppFolders AppFolders { get; set; }
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Employee> _employeeRepo;
        public EmployeeAppService(ISqlExecuter sqlExecuter
            , IRepository<Employee> empRepo)
        {
            _sqlExecuter = sqlExecuter;
            _employeeRepo = empRepo;

        }

        //lấy ds user lúc search claim
        public List<UserListDto> GetEmployeesForClaim()
        {
            var exceptionUserList = new List<string> { "admin", "DOMINIC", "OPERATOR", "CFO" };
            List<UserListDto> list = UserManager.Users.Where(x => x.IsActive && !exceptionUserList.Contains(x.Name)).OrderBy(x => x.Name).ToList().MapTo<List<UserListDto>>();
            return list;
        }

        public List<UserListDto> GetEmployeesForCRS()
        {
            //TODO ADD PERMISSION                        
            var curr_user = GetCurrentUser();
            var curr_role = "";
            var context = _sqlExecuter.GetTLSDBContext();

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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }
            }
            
            var exceptionUserList = new List<string> { "admin", "DOMINIC", "OPERATOR", "CFO" };

            List<UserListDto> list = UserManager.Users.Where(x => x.IsActive && (curr_role=="Admin" || x.Id == curr_user.Id) && !exceptionUserList.Contains(x.Name)).OrderBy(x => x.Name).ToList().MapTo<List<UserListDto>>();
            return list;
        }

        public List<UserListDto> GetEmployeesForActualFee()
        {
            //TODO ADD PERMISSION                        
            var curr_user = GetCurrentUser();
            var curr_role = "";
            var context = _sqlExecuter.GetTLSDBContext();

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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }
            }

            bool getAllFee = false;

            if (curr_role == "Admin" || checkPermission_AllActualFee())
            {
                getAllFee = true;
            }

            var exceptionUserList = new List<string> { "admin", "DOMINIC", "OPERATOR", "CFO" };

            List<UserListDto> list = UserManager.Users.Where(x => x.IsActive && (getAllFee || x.Id == curr_user.Id) && !exceptionUserList.Contains(x.Name)).OrderBy(x => x.Name).ToList().MapTo<List<UserListDto>>();
            return list;
        }

        public decimal? GetProFee(int empId)
        {
            var fee = _employeeRepo.Get(empId).Fee;
            return fee;
        }

        public List<CalcUserContribution> GetUserContributionInfo(GetUserContributionInput input)
        {
            
            List<CalcUserContribution> result = new List<CalcUserContribution>();
            SqlConnection connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

            SqlCommand comm = new SqlCommand("CalcUserContribution", connection);
            comm.CommandTimeout = 18000;
            connection.Open();

            comm.Connection = connection;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
            comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
            comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
            comm.Parameters.Add("@ISFIXED", SqlDbType.VarChar).Value = "0";

            SqlDataReader reader = comm.ExecuteReader();

            // Call Read before accessing data.
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                try
                {
                    CalcUserContribution cuc = new CalcUserContribution();
                    string ord = record["ORD"].ToString();
                    Logger.Debug("ord" + ord);
                    if (!string.IsNullOrEmpty(ord))
                    {
                        cuc.ORD = int.Parse(ord);
                    }
                    cuc.TimeSheetName = record["TimeSheetName"].ToString();
                    string c1 = record["C1"].ToString();
                    if (!string.IsNullOrEmpty(c1))
                    {
                        cuc.C1 = decimal.Parse(c1);
                    }
                    string s1 = record["S1"].ToString();
                    if (!string.IsNullOrEmpty(s1))
                    {
                        cuc.S1 = DateTime.ParseExact(s1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    cuc.C2 = decimal.Parse(record["C2"].ToString());

                    string s2 = record["S2"].ToString();
                    if (!string.IsNullOrEmpty(s2))
                    {
                        cuc.S2 = DateTime.ParseExact(s2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    string balance = record["BalAMT"].ToString();

                    if (!string.IsNullOrEmpty(balance))
                    {
                        cuc.Balance = decimal.Parse(balance);
                    }

                    string c = record["C"].ToString();

                    if (!string.IsNullOrEmpty(c))
                    {
                        cuc.C = decimal.Parse(c);
                    }

                    //Logger.Debug(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", record[0], record[1]
                    //    , record[2], record[3], record[4], record[5], record[6], record[7], record[8]));
                    result.Add(cuc);
                }
                catch (Exception e)
                {
                    Logger.Error("error", e);
                }
            }

            connection.Close();
            return result;
        }

        public List<CalcContribution> GetContributionInfo(GetUserContributionInput input)
        {
            List<CalcContribution> result = new List<CalcContribution>();
            SqlConnection connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

            SqlCommand comm = new SqlCommand("CalcContribution", connection);
            comm.CommandTimeout = 18000;
            connection.Open();

            comm.Connection = connection;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
            comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
            comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
            comm.Parameters.Add("@ISFIXED", SqlDbType.VarChar).Value = "0";

            SqlDataReader reader = comm.ExecuteReader();

            // Call Read before accessing data.
            while (reader.Read())
            {
                IDataRecord record = (IDataRecord)reader;
                try
                {
                    CalcContribution cuc = new CalcContribution();
                    string ord = record["ORD"].ToString();
                    if (!string.IsNullOrEmpty(ord))
                    {
                        cuc.ORD = int.Parse(ord);
                    }
                    cuc.EmpName = record["EmpName"].ToString();
                    
                    string conAMT = record["ConAMT"].ToString();
                    if (!string.IsNullOrEmpty(conAMT))
                    {
                        cuc.ConAMT = decimal.Parse(conAMT);
                    }
                    string incomAMT = record["IncomAMT"].ToString();
                    if (!string.IsNullOrEmpty(incomAMT))
                    {
                        cuc.IncomAMT = decimal.Parse(incomAMT);
                    }                   
                    string balance = record["BALAMT"].ToString();
                    if (!string.IsNullOrEmpty(balance))
                    {
                        cuc.BALAMT = decimal.Parse(balance);
                    }

                    string audit = record["Audit"].ToString();
                    if (!string.IsNullOrEmpty(audit))
                    {
                        cuc.Audit = audit;
                    }
                    //Logger.Debug(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", record[0], record[1]
                    //    , record[2], record[3], record[4], record[5], record[6], record[7], record[8]));
                    result.Add(cuc);
                }
                catch (Exception e)
                {
                    Logger.Error("error", e);
                }
            }
            
            connection.Close();
            return result;
        }

        public List<UserIncome> GetUserIncomeInfo(GetUserContributionInput input)
        {
            DataTable rs = new DataTable();
            List<UserIncome> list = new List<UserIncome>();
            SqlConnection connection = null;
            try
            {
                connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;
                {
                    SqlCommand comm = new SqlCommand("CalcIncome", connection);
                    comm.CommandTimeout = 18000;
                    connection.Open();

                    comm.Connection = connection;
                    comm.CommandType = CommandType.StoredProcedure;
                    //comm.CommandText = SPName;
                    comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
                    comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
                    comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
                    SqlDataReader reader = comm.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        UserIncome userIncome = new UserIncome();
                        string ord = record["ORD"].ToString();
                        Logger.Debug("ord" + ord);
                        if (!string.IsNullOrEmpty(ord))
                        {
                            userIncome.ORD = int.Parse(ord);
                        }
                        userIncome.Month = record["MONTH"].ToString();
                        string scholaship = record["SCHOLASHIP"].ToString();
                        if (!string.IsNullOrEmpty(scholaship))
                        {
                            userIncome.Scholaship = decimal.Parse(scholaship);
                        }
                        string other = record["OTHER"].ToString();
                        if (!string.IsNullOrEmpty(other))
                        {
                            userIncome.Other = decimal.Parse(other);
                        }
                        string salary = record["SALARY"].ToString();
                        if (!string.IsNullOrEmpty(salary))
                        {
                            userIncome.Salary = decimal.Parse(salary);
                        }
                        string income = record["INCOME"].ToString();
                        if (!string.IsNullOrEmpty(income))
                        {
                            userIncome.Income = decimal.Parse(income);
                        }
                        list.Add(userIncome);
                    }

                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public FileDto ExportUserContribution(GetUserContributionInput input)
        {
            List<CalcUserContribution> result = new List<CalcUserContribution>();
            SqlConnection connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

            SqlCommand comm = new SqlCommand("CalcUserContribution", connection);
            comm.CommandTimeout = 18000;
            connection.Open();

            comm.Connection = connection;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
            comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
            comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
            comm.Parameters.Add("@ISFIXED", SqlDbType.VarChar).Value = "0";

            string path = AppFolders.TempFileDownloadFolder;            
            //System.Configuration.ConfigurationManager.AppSettings["ExportFolder"];
            string filename = "UserContribution_" + input.StartDate?.ToString("yyyyMMdd") + "_" + input.EndDate?.ToString("yyyyMMdd") + ".xlsx";

            var file = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, filename);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataSet rs = new DataSet();
                da.Fill(rs);
                DataTable dt = rs.Tables[0].Copy();
                dt.Columns.RemoveAt(0);
                string empName = "All";
                if (input.EmployeeID != 0)
                {
                    empName = _employeeRepo.FirstOrDefault(x => x.Id == input.EmployeeID).Name;
                }
                string tempFile = System.Configuration.ConfigurationManager.AppSettings["TemplateFoler"] + "TML_USer_Contribution_Report.xlsx";                
                ExcelService.FillDatatableToExcel(dt, Path.Combine(path, file.FileToken), tempFile, "ContributionInfo", 5, 2, 6, 10, 20, 3, 6,
                    input.StartDate?.ToString("dd/MM/yyyy"), 3, 8, input.EndDate?.ToString("dd/MM/yyyy"), 0, 2, 6, empName);

            }
            catch (Exception e)
            {
                Logger.Error("error", e);
            }
            finally
            {
                connection.Close();
            }

            return file;
        }

        public FileDto ExportContribution(GetUserContributionInput input)
        {
            List<CalcUserContribution> result = new List<CalcUserContribution>();
            SqlConnection connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

            SqlCommand comm = new SqlCommand("CalcContribution", connection);
            comm.CommandTimeout = 18000;
            connection.Open();

            comm.Connection = connection;
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
            comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
            comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
            comm.Parameters.Add("@ISFIXED", SqlDbType.VarChar).Value = "0";

            string path = AppFolders.TempFileDownloadFolder;
            //System.Configuration.ConfigurationManager.AppSettings["ExportFolder"];
            string filename = "Contribution_" + input.StartDate?.ToString("yyyyMMdd") + "_" + input.EndDate?.ToString("yyyyMMdd") + ".xlsx";

            var file = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, filename);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataSet rs = new DataSet();
                da.Fill(rs);
                DataTable dt = rs.Tables[0].Copy();
                dt.Columns.RemoveAt(0);
                string empName = "All";
                if (input.EmployeeID != 0)
                {
                    empName = _employeeRepo.FirstOrDefault(x => x.Id == input.EmployeeID).Name;
                }
                string tempFile = System.Configuration.ConfigurationManager.AppSettings["TemplateFoler"] + "TML_Contribution_Report.xlsx";
                ExcelService.FillDatatableToExcel(dt, Path.Combine(path, file.FileToken), tempFile, "ContributionInfo", 5, 2, 6, 10, 20, 3, 5,
                    input.StartDate?.ToString("dd/MM/yyyy"), 3, 7, input.EndDate?.ToString("dd/MM/yyyy"), 0, 2, 5, empName);

            }
            catch (Exception e)
            {
                Logger.Error("error", e);
            }
            finally
            {
                connection.Close();
            }

            return file;
        }

        public FileDto ExportUserIncome(GetUserContributionInput input)
        {
            List<UserIncome> result = new List<UserIncome>();
            SqlConnection connection = null;
            string path = AppFolders.TempFileDownloadFolder;
            string filename = "UserIncome_" + input.StartDate?.ToString("yyyyMMdd") + "_" + input.EndDate?.ToString("yyyyMMdd") + ".xlsx";

            var file = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, filename);

            try
            {
                connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

                SqlCommand comm = new SqlCommand("CalcIncome", connection);
                comm.CommandTimeout = 18000;
                connection.Open();

                comm.Connection = connection;
                comm.CommandType = CommandType.StoredProcedure;
                //comm.CommandText = SPName;
                comm.Parameters.Add("@EMPID", SqlDbType.Int).Value = input.EmployeeID;
                comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
                comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
                
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataSet rs = new DataSet();
                da.Fill(rs);
                DataTable dt = rs.Tables[0].Copy();
                dt.Columns.RemoveAt(2);
                dt.Columns.RemoveAt(0);

                string empName = _employeeRepo.FirstOrDefault(x => x.Id == input.EmployeeID).Name;

                string tempFile = System.Configuration.ConfigurationManager.AppSettings["TemplateFoler"] + "TML_USer_Income_Report.xlsx";
                
                ExcelService.FillDatatableToExcel(dt, Path.Combine(path, file.FileToken), tempFile, "IncomeInfo", 5, 2, 6, 10, 20, 3, 4,
                input.StartDate?.ToString("dd/MM/yyyy"), 3, 6, input.EndDate?.ToString("dd/MM/yyyy"), 0, 2, 4, empName);

            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return file;
        }

        public List<EmployeeView> GetEmployeeList()
        {
            var context = _sqlExecuter.GetTLSDBContext();
            {
                var query = from emp in context.Employees where emp.IsActive == true                            
                            select new EmployeeView
                            {
                                EmployeeID = emp.Id,
                                Name = emp.Name
                            };
                return query.Distinct().ToList();
            }
        }

        public List<EmployeeView> GetEmployeeListByClaim(string claimID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            {
                var query = from empclaim in context.EmployeeClaims
                            join e in context.Users on empclaim.EmployeeID equals e.EmployeeId
                            where empclaim.ClaimID == claimID && e.IsActive == true
                            select new EmployeeView
                            {
                                EmployeeID = (int)e.EmployeeId,
                                Name = e.Name
                            };
                return query.Distinct().ToList();
            }
        }

        public List<EmployeeView> GetEmployeeListIncome()
        {
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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }                
            }

            var query = from emp in context.Employees
                        where ((curr_role == "Admin") || (emp.Id == curr_user.EmployeeId)) && (emp.IsActive == true)
                        select new EmployeeView
                        {
                            EmployeeID = emp.Id,
                            Name = emp.Name
                        };
            return query.Distinct().OrderBy(em => em.Name).ToList();
        }

        //Get employee from a TimeSheet
        public List<EmployeeView> GetEmployeeListByTS(int TimeSheetID)
        {
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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }                
            }

            bool CheckGetAllActualFee = checkPermission_AllActualFee();
            bool CheckGetAllProFee = checkPermission_ViewAllProFee();
            bool getAllEmp = false;
            bool CheckGetAllProFeeAM = checkPermission_ViewAllProFeeAM();
            if (curr_role == "Admin" || CheckGetAllProFee || CheckGetAllActualFee)
            {
                getAllEmp = true;
            }

            var query = from p in context.ProfessionalFees
                        join em in context.Employees on p.CreateBy equals em.Id
                        join t in context.TimeSheets on p.TimeSheetID equals t.Id
                        where (p.TimeSheetID == TimeSheetID) && (em.IsActive == true)
                              && (getAllEmp 
                                 || (p.CreateBy == curr_user.EmployeeId) 
                                 || (CheckGetAllProFeeAM && (t.Claim.AccountManagerID == curr_user.EmployeeId))
                                 )
                        //where (p.TimeSheetID == TimeSheetID) && (p.CreateBy == curr_user.EmployeeId) && (em.Id == curr_user.EmployeeId)
                        select new EmployeeView
                        {
                            EmployeeID = em.Id,
                            Name = em.Name,
                            Fee = em.Fee
                        };

            //var query = from t in context.TimeSheets
            //            join p in context.ProfessionalFees on t.Id equals p.TimeSheetID
            //            join ec in context.EmployeeClaims on t.ClaimID equals ec.ClaimID
            //            join e in context.Employees on ec.EmployeeID equals e.Id
            //            where (t.Id == TimeSheetID) && (getAllEmp || (t.Claim.AccountManagerID == curr_user.EmployeeId) || (p.CreateBy == curr_user.EmployeeId))
            //            select new EmployeeView
            //            {
            //                EmployeeID = e.Id,
            //                Name = e.Name,
            //                Fee = e.Fee
            //            };
            return query.Distinct().ToList();

        }

        //hvtam-29042015 get charged employee timesheet
        //Get employee from a TimeSheet
        public List<EmployeeView> GetChargedEmployeeListByTS(int TimeSheetID)
        {
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
                if (curr_role != "Admin")
                {
                    curr_role = role.Name;
                }
            }

            bool CheckGetAllActualFee = checkPermission_AllActualFee();
            bool CheckGetAllProFee = checkPermission_ViewAllProFee();
            bool CheckGetAllProFeeAM = checkPermission_ViewAllProFeeAM();

            bool getAllEmp = false;

            if (curr_role == "Admin" || CheckGetAllProFee || CheckGetAllActualFee)
            {
                getAllEmp = true;
            }

            var query = from p in context.ProfessionalFees
                        join em in context.Employees on p.CreateBy equals em.Id
                        join t in context.TimeSheets on p.TimeSheetID equals t.Id
                        where (p.TimeSheetID == TimeSheetID) && (em.IsActive == true)
                              && (getAllEmp
                                 || (p.ChargedBy == curr_user.EmployeeId)
                                 || (CheckGetAllProFeeAM && (t.Claim.AccountManagerID == curr_user.EmployeeId))
                                 )
                        select new EmployeeView
                        {
                            EmployeeID = em.Id,
                            Name = em.Name
                        };
            return query.Distinct().ToList();

        }

        /// <summary>
        /// Get EmpInfo
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public EmployeeView GetEmpInfo(int empID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            {
                //hvtam-21112014 Get Employee Info
                EmployeeView empinfo = new EmployeeView();
                var query = from e in context.Employees
                            where e.Id == empID
                            select new EmployeeView
                            {
                                EmployeeID = e.Id,
                                Name = e.Name,
                                JobTitle = ((e.JobTitle == null || e.JobTitle == "") ? string.Empty : e.JobTitle),
                                JobPosition = e.JobPosition,
                                Address = e.Address,
                                Phone = e.Phone,
                                Email = e.Email,
                                JoinDate = e.JoinDate,
                                DateOfBirth = e.DateOfBirth,
                                Fee = e.Fee,
                                IsActive = e.IsActive.HasValue?e.IsActive.Value:false,
                                IsAuthen = e.isAuthen,

                            };
                empinfo = query.SingleOrDefault();
                return empinfo;
            }            
        }

        public EmployeeView GetUserInfo(int empID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            {
                //hvtam-21112014 Get Employee Info
                EmployeeView empinfo = new EmployeeView();
                var query = from u in context.Users
                            where u.EmployeeId == empID
                            select new EmployeeView
                            {
                                EmployeeID = u.EmployeeId,
                                Name = u.Name,
                                JobTitle = ((u.JobTitle == null || u.JobTitle == "") ? string.Empty : u.JobTitle),
                                JobPosition = u.JobPosition,
                                Address = u.Address,
                                Phone = u.Phone,
                                Email = u.EmailAddress,
                                JoinDate = u.JoinDate,
                                DateOfBirth = u.DateOfBirth,
                                Fee = u.Fee,
                                IsActive = u.IsActive,
                                IsAuthen = u.IsActive,

                            };
                empinfo = query.SingleOrDefault();
                return empinfo;
            }
        }

        public bool checkPermission_AllManagementTimeSheet()
        {
            bool all_permission = false;
            var curr_user = GetCurrentUser();
            var permission_alltimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageAllTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            all_permission = permissionlist.Result.Contains(permission_alltimesheet);
            return all_permission;
        }

        public bool checkPermission_AllActualFee()
        {
            bool all_actualfee = false;
            var curr_user = GetCurrentUser();
            var permission_alltimesheet = PermissionManager.GetPermission(AppPermissions.Pages_ReportsExport_ActualFeeAll);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            all_actualfee = permissionlist.Result.Contains(permission_alltimesheet);
            return all_actualfee;
        }

        public bool checkPermission_ViewAllProFee()
        {
            bool viewallprofee_permission = false;
            var curr_user = GetCurrentUser();
            var permission_viewallprofee = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFee);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            viewallprofee_permission = permissionlist.Result.Contains(permission_viewallprofee);
            return viewallprofee_permission;
        }

        public bool checkPermission_ViewAllProFeeAM()
        {
            bool viewallprofeeam_permission = false;
            var curr_user = GetCurrentUser();
            var permission_viewallprofeeam = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFeeAM);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            viewallprofeeam_permission = permissionlist.Result.Contains(permission_viewallprofeeam);
            return viewallprofeeam_permission;
        }

        public int CreateEmployee(Employee input)
        {
            return _employeeRepo.InsertAndGetId(input);            
        }
        public Employee UpdateEmployee(Employee input)
        {            
            return _employeeRepo.Update(input);
        }
    }
}
