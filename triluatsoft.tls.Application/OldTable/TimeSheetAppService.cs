using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.OldTable.View;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.OldTable
{
    public class TimeSheetAppService : tlsAppServiceBase, ITimeSheetAppService
    {
        private readonly IRepository<TimeSheet> _timesheetRepo;
        private readonly IRepository<Claim, string> _claimTableRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<EmployeeClaim> _employeeClaimRepo;
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IRepository<Expense> _expenseRepo;
        private readonly IRepository<ProfessionalFee> _professionalFeeRepo;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IProfessionalFeeAppService _proService;
        private readonly IExpenseAppService _expService;
        private readonly IEmployeeAppService _empService;

        public TimeSheetAppService(IRepository<TimeSheet> timesheetRepo
            , IRepository<Claim, string> claimRepo
            , IRepository<Customer> customerRepo
            , IRepository<EmployeeClaim> employeeClaimRepo
            , IRepository<Employee> employeeRepo
            , IRepository<Expense> expenseRepo
            , IRepository<ProfessionalFee> professionalFeeRepo
            , ISqlExecuter sqlExecuter
            , IProfessionalFeeAppService proService
            , IExpenseAppService expService
            , IEmployeeAppService empService)
        {
            _timesheetRepo = timesheetRepo;
            _claimTableRepo = claimRepo;
            _customerRepo = customerRepo;
            _employeeClaimRepo = employeeClaimRepo;
            _employeeRepo = employeeRepo;
            _expenseRepo = expenseRepo;
            _professionalFeeRepo = professionalFeeRepo;
            _sqlExecuter = sqlExecuter;
            _proService = proService;
            _expService = expService;
            _empService = empService;
        }
        public void Create(string claimId, int empId)
        {
            int SeqNo = GetNewTSSeq(claimId);
            TimeSheet ts = new TimeSheet();
            ts.ClaimID = claimId;
            ts.TSCode = "TS";
            ts.TSSeqNo = SeqNo;
            ts.TimeSheetName = claimId + ".TS" + ts.TSSeqNo.ToString();
            ts.ExchangeRate = 22000;  //hvtam-28042016 tạm hardcode
            ts.CreateBy = empId;

            ts.CreateDate = DateTime.Now;
            ts.UpdateBy = empId; ;
            ts.UpdateDate = DateTime.Now;
            ts.IsIssued = false;
            ts.IsInvoiced = false;

            ts.IsDebitNote = false;
            ts.DebitNoteBy = 0;
            ts.InvoiceBy = 0;
            _timesheetRepo.InsertAndGetId(ts);
        }
        public int GetNewTSSeq(string claimId)
        {
            int SeqNo = CConvert.ToInt(_timesheetRepo.GetAll().Where(t => t.ClaimID == claimId).Max(t => t.TSSeqNo));
            return (SeqNo + 1);
        }

        /// <summary>
        /// return claimId + ".TS" + tsSeq.ToString(); to create new timesheet
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public string GetNewTSSeqStr(string claimId)
        {

            int tsSeq = GetNewTSSeq(claimId);
            return claimId + ".TS" + tsSeq.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagedResultDto<VTimesheet> SearchTimeSheet(TimeSheetSearchOption input)
        {

            //hvtam-14042015: Chi hien thi timesheet theo userid search
            //int userid = 0;
            //TODO
            //if (HasPermission(PageCapability.Timesheet_Manage_Admin)) //admin right
            //{
            //    userid = 0;
            //}
            //else if (HasPermission(PageCapability.Timesheet_Manage_Mine)) //user right
            //{
            //    userid = User.EmployeeID.Value;
            //}            

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

            var permission_ammngt = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageAMTimesheet);
            var permission_mymngt = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageMyTimesheet);
            var permission_allmngt = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageAllTimesheet);
            var permission_issuedmngt = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageIssuedTimesheetOnly);
            var permission_notissuedmgnt = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageNotIssuedTimesheetOnly);
            var permission_getallfee = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFee);
            var permission_getallfeeAM = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ViewAllProFeeAM);

            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);

            var ammngt = permissionlist.Result.Contains(permission_ammngt);
            var mymngt = permissionlist.Result.Contains(permission_mymngt);
            var allmngt = permissionlist.Result.Contains(permission_allmngt);
            var issuedmngt = permissionlist.Result.Contains(permission_issuedmngt);
            var notissuedmgnt = permissionlist.Result.Contains(permission_notissuedmgnt);
            var getallProFee = permissionlist.Result.Contains(permission_getallfee);
            var getallProFeeAM = permissionlist.Result.Contains(permission_getallfeeAM);

            bool getAllFee = false;
            if (curr_role == "Admin" || getallProFee)
            {
                getAllFee = true;
            }

            /*
            TimeSheetSearchOption opt = new TimeSheetSearchOption();
            opt.ClaimID = "";
            opt.TimeSheet = "";
            opt.HaveInvoice = null;
            opt.HaveDebitNote = null; */

            string where = " IsDeleted = 'false'";
            //string where = " ";                        

            if (curr_role != "Admin")
            {
                if (!allmngt)
                {
                    if (mymngt)
                    {
                        where += " AND (CID IN (SELECT CLaimID FROM EmployeeClaim WHERE EmployeeID = " + curr_user.EmployeeId + "))";
                    }
                    else
                    {
                        if (ammngt)
                        {
                            where += " AND (CID IN (SELECT ID FROM Claim WHERE AccountManagerID = " + curr_user.EmployeeId + "))";
                        }
                        else
                        {
                            where += " AND ( 0 > 1 )";
                        }
                    }
                }

                if (!issuedmngt || !notissuedmgnt)
                {
                    if (issuedmngt)
                    {
                        where += " AND IsIssued = 1";
                    } else
                    {
                        if (notissuedmgnt)
                        {
                            where += " AND IsIssued = 0";
                        } else
                        {
                            where += " AND ( 0 > 1 )";
                        }                        
                    }
                }
            }

            if (input.TsRole.HasValue)
            {
                switch (input.TsRole.Value)
                {
                    case 1:
                        where += " AND (CID IN (SELECT ID FROM Claim WHERE AccountManagerID = " + curr_user.EmployeeId + "))";
                        break;
                    case 2:
                        where += " AND (CID IN (SELECT CLaimID FROM EmployeeClaim WHERE EmployeeID = " + curr_user.EmployeeId + "))";
                        break;
                }
            }

            if (input.StartDate != null)
            {
                where += string.Format(" AND (CreateDate >= '{0:yyyy-MM-dd HH:mm:ss}')", input.StartDate);

            }
            if (input.EndDate != null)
            {
                where += string.Format(" AND (CreateDate < '{0:yyyy-MM-dd HH:mm:ss}' )", input.EndDate);

            }
            if (!string.IsNullOrEmpty(input.ClaimID))
            {
                where += string.Format(" AND CID like '%{0}%' ", input.ClaimID);
            }
            if (!string.IsNullOrEmpty(input.TimeSheet))
            {
                where += string.Format(" AND TimeSheetName like '%{0}%' ", input.TimeSheet);
            }
            if (input.isIssued.HasValue)
            {
                where += string.Format(" AND IsIssued = '{0}' ", input.isIssued.Value);
            }
            if (input.HaveInvoice.HasValue)
            {
                where += string.Format(" AND IsInvoiced = '{0}' ", input.HaveInvoice.Value);
            }            
            string order = " ORDER BY DateOfAssignment DESC ";
            var page = Math.Max(input.Page, 1);
            var rowNumT = input.PageSize * page;
            var rowNumF = rowNumT - (input.PageSize - 1);
            rowNumF = Math.Max(rowNumF, 0);
            string from = string.Format(" (SELECT ROW_NUMBER() OVER(" + order + ") AS Row#, * FROM VTimesheet WHERE " + where + " ) AS TimeOrder WHERE (Row# Between {0} AND {1}) ", rowNumF, rowNumT);
            List<VTimesheet> list = _sqlExecuter.GetDatabase().SqlQuery<VTimesheet>("Select * from  "
                + from
                ).ToList();

            var total = _sqlExecuter.GetDatabase().SqlQuery<int>("SELECT COUNT(TID) from VTimesheet WHERE " + where).First();

            var ae_claim = _sqlExecuter.GetDatabase().SqlQuery<string>("SELECT ClaimID from EmployeeClaim WHERE EmployeeID = " + curr_user.EmployeeId).ToList();
            var am_claim = _sqlExecuter.GetDatabase().SqlQuery<string>("SELECT ID from Claim WHERE AccountManagerID = " + curr_user.EmployeeId).ToList();

            foreach (var tsitem in list)
            {
                tsitem.TsRole = 0;
                if (curr_role != "Admin")
                {
                    if (ae_claim.Contains(tsitem.CID))
                    {
                        tsitem.TsRole = 2;
                    }
                    
                    if (am_claim.Contains(tsitem.CID))
                    {
                        tsitem.TsRole = 1;
                    }
                }

                //if (!tsitem.IsIssued.Value)
                //{
                tsitem.ProFeeGrandAMT = Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == tsitem.TID) && (getAllFee || (pf.CreateBy == curr_user.EmployeeId) || (getallProFeeAM && (tsitem.AccountManagerID == curr_user.EmployeeId))))).Sum(o => o.ChargedProFeeValue != null ? o.ChargedProFeeValue : o.ProFeeValue)) ?? 0) * (tsitem.ExchangeRate ?? 0));
                tsitem.ExpenseAMT = ((context.Expenses.Where(exp => exp.TimeSheetID == tsitem.TID).Sum(o => o.Amount)) ?? 0);
                tsitem.GrandAMT = tsitem.ProFeeGrandAMT + tsitem.ExpenseAMT;

                //tsitem.ProFeeGrandAMT = 0;
                //tsitem.ExpenseAMT = 0;
                //tsitem.GrandAMT = 0;
                //} else
                //{

                //}
            }


            var result = new PagedResultDto<VTimesheet>()
            {
                TotalCount = total
                ,
                Items = list
            };
            return result;
        }
        public PagedResultDto<TimeSheetView> SearchTimeSheet2(TimeSheetSearchOption opt)
        {
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

            var mngAllTS = checkPermission_AllManagementTimeSheet();
            var mngAllProFee = checkPermission_ViewAllProFee();
            var mngAllProFeeAM = checkPermission_ViewAllProFeeAM();

            bool getAllTS = false;
            if ((curr_role == "Admin") || (mngAllTS))
            {
                getAllTS = true;
            }

            //hvtam-14042015: Chi hien thi timesheet theo userid search
            //int userid = 0;
            //TODO
            //if (HasPermission(PageCapability.Timesheet_Manage_Admin)) //admin right
            //{
            //    userid = 0;
            //}
            //else if (HasPermission(PageCapability.Timesheet_Manage_Mine)) //user right
            //{
            //    userid = User.EmployeeID.Value;
            //}

            //var context = _sqlExecuter.GetTLSDBContext();
            
            var query = from t in context.TimeSheets
                        join c in context.Claims on t.ClaimID equals c.Id
                        join cust in context.Customers on c.InsurerID equals cust.Id
                        join empclaim in context.EmployeeClaims on c.Id equals empclaim.ClaimID // chi lay timesheet dc tao boi chinh user
                        where 
                          ((opt.StartDate == null) || (t.CreateDate >= opt.StartDate))
                          && ((opt.EndDate == null) || (t.CreateDate <= opt.EndDate))                          
                          && (string.IsNullOrEmpty(opt.ClaimID) || t.ClaimID.Contains(opt.ClaimID))
                          && (string.IsNullOrEmpty(opt.TimeSheet) || t.TimeSheetName.Contains(opt.TimeSheet))// || t.TSSeqNo == CConvert.ToInt(opt.TimeSheet))
                          && ((opt.HaveInvoice == null) || (t.IsInvoiced == opt.HaveInvoice.Value))
                          && (opt.HaveDebitNote == null || t.IsDebitNote == opt.HaveDebitNote.Value)
                          //&& (userid == 0 || empclaim.EmployeeID == userid) //0 cho user thuoc group admin
                          //&&(userid == 0 || t.CreateBy == userid //0 cho user thuoc group admin
                          && ((getAllTS) || (empclaim.EmployeeID == curr_user.EmployeeId)) // chi lay timesheet dc tao boi chinh user hoac nhung ts cua claim ma user la AM
                          && ((opt.isIssued == null) || (t.IsIssued == opt.isIssued.Value))
                        //orderby t.IsIssued ascending, c.DateOfAssignment descending //hvtam-15122014

                        select new TimeSheetView
                        {
                            TimeSheetID = t.Id,
                            ClaimID = t.ClaimID,
                            TimeSheetName = t.TimeSheetName,
                            CustomerName = cust.Name,
                            IsIssued = t.IsIssued.Value,
                            IsInvoiced = t.IsInvoiced.Value,
                            IsSubmited = t.IsSubmited,
                            SubmitDate = t.SubmitDate,
                            SubmitByName = (from e in context.Employees where e.Id == t.SubmitBy select e.Name).FirstOrDefault(),
                            SubmitBy = t.SubmitBy,

                            CreateBy = t.CreateBy.Value,
                            CreatedDate = t.CreateDate.Value,
                            UpdateBy = t.UpdateBy.Value,
                            UpdateDate = t.UpdateDate.Value,
                            
                            CreateByName = (from e in context.Employees where e.Id == t.CreateBy select e.Name).FirstOrDefault(),
                            UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),

                            AssignmentDate = c.DateOfAssignment,

                            ExchangeRate = t.ExchangeRate.Value,
                            DiscountVal = t.DiscountVal.Value,
                            DiscountType = t.DiscountType,
                            DiscountAMT = Math.Round(t.DiscountAMT ?? 0), //hvtam-16092015

                            ////hvtam-22042015 ProFeeAMT = t.ProFeeAMT.Value, neu chua approve thi lay gia tri profee, neu approve thi lay gia tri profeeaproved
                            //ExpenseAMT = t.IsIssued == true ? (t.ExpenseAMT ?? 0)
                            //                              : (context.Expense.Where(exp => exp.TimeSheetID == t.TimeSheetID).Sum(o => (decimal?)o.Amount)) ?? 0,
                            //  //hvtam-24042015: Profee in USD
                            //ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => pf.TimeSheetID == t.TimeSheetID).Sum(o => (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  
                            //ProFeeAMT = (t.IsIssued == true ?  t.ProFeeAMT??0 
                            //                               : ((context.ProfessionalFees.Where(pf => pf.TimeSheetID == t.TimeSheetID).Sum(o => (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0)), //hvtam-22042015

                            ExpenseAMT = (t.IsIssued == true ? t.ExpenseAMT : (context.Expenses.Where(exp => exp.TimeSheetID == t.Id).Sum(o => (decimal?)o.Amount)) ?? 0),
                            ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.EmployeeId) || (mngAllProFee) || (mngAllProFeeAM && (c.AccountManagerID == curr_user.EmployeeId))))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  

                            ProFeeAMTVND = (t.IsIssued == true ? t.ProFeeAMT : Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.EmployeeId) || mngAllProFee || (mngAllProFeeAM && (c.AccountManagerID == curr_user.EmployeeId))))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0))), //hvtam-22042015

                            ProFeeGrandAMT = Math.Round(t.ProFeeGrandAMT ?? 0),
                            TaxAMT = Math.Round(t.TaxAMT ?? 0),
                            GrandAMT = Math.Round(t.GrandAMT ?? 0),

                            //hvtam-24042015 ActualTime calculate
                            //ActualProFeeAMT = ((context.ProfessionalFees.Where(pf => pf.TimeSheetID == t.TimeSheetID).Sum(o => (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0), //hvtam-22042015
                            //endhvtam-24042015 ActualTime calculate

                            IsDebitNote = t.IsDebitNote.Value,
                            DebitNoteCode = t.DebitNoteCode,
                            DebitNoteDate = t.DebitNoteDate.Value,
                            DebitNoteBy = t.DebitNoteBy.Value,
                            DebitNoteByName = (from e in context.Employees where e.Id == t.DebitNoteBy select e.Name).FirstOrDefault(),

                            TSCode = t.TSCode,
                            TSSeqNo = t.TSSeqNo.Value,

                            InvoiceCode = t.InvoiceCode
                        };

            query = query.Distinct().OrderByDescending(t => t.AssignmentDate);

            var total = query.Count();

            query = query.Skip(opt.PageSize * opt.Page)
                .Take(opt.PageSize);

            var list = query.ToList();

            foreach (var tsv in list) {
                tsv.GrandAMT = tsv.ProFeeGrandAMT + tsv.ExpenseAMT;
                List<EmployeeView> lstEmpView = new List<EmployeeView>();
                //if (HasPermission(PageCapability.Timesheet_Manage_Admin)) //admin right
                {

                    if (tsv.IsIssued == false)
                    {
                        lstEmpView = _empService.GetEmployeeListByTS(tsv.TimeSheetID); //hvtam-31102015 - report working timesheet
                    }
                    else
                    {
                        lstEmpView = _empService.GetChargedEmployeeListByTS(tsv.TimeSheetID); //hvtam-31102015-report charged timesheet
                    }
                    tsv.ListEmpView = lstEmpView;
                }
                //else if (HasPermission(PageCapability.Timesheet_Report_ProFee)) //user right
                //{

                //    userid = User.EmployeeID.Value;

                //    EmployeeView emp = EmployeeService.getEmpInfo(userid);
                //    lstEmpView.Add(emp);
                //    ddlUser.DataSource = lstEmpView;
                //    ddlUser.DataTextField = "Name";
                //    ddlUser.DataValueField = "EmployeeID";
                //    ddlUser.BindDataWithEmptyItem(lstEmpView, EmployeeViewColumn.EMPLOYEE_ID, EmployeeViewColumn.EMPLOYEE_NAME);
                //    ddlUser.SelectByValue(lstEmpView != null
                //    ? emp.EmployeeID.ToString()
                //    : DomainService.ITEM_EMPTY_VALUE);
                //}

            }

            var result = new PagedResultDto<TimeSheetView>()
            {
                TotalCount = total,
                Items = list
            };
            return result;
        }

        public List<ProfessionalFeeView> GetProfessionalFee(int timeSheetID)
        {

            //****************************************************************************************************
            //hvtam-24022015
            // Chi cho phep user nao, load profee cua user do        
            //****************************************************************************************************
            //List<ProfessionalFeeView> list = ProfessionalFeeService.GetList(claimID, int.Parse(TimeSheetID)); 
            //hvtam-29032016 List<ProfessionalFeeView> profeelist = new  List<ProfessionalFeeView>(); profeelist->UserListProFee

            //hvtam-09042015: Chi cho hien thi timesheet cua chinh user đo

            //TODO
            //if (HasPermission(PageCapability.Timesheet_Enter))
            //{

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

            //int userid = (int)GetCurrentUser().Id;
            //userid = 0;
            List<ProfessionalFeeView> userListProFee = GetListProfeebyUser(timeSheetID, curr_role, curr_user.EmployeeId);

            //}
            return userListProFee;

        }

        //[UnitOfWork(IsDisabled = true)]
        public List<ProfessionalFeeView> GetListProfeebyUser(int TimeSheetID, string roleName, long userId)
        {
            var allpfmngt = checkPermission_ViewAllProFee();
            var allpfmngtAM = checkPermission_ViewAllProFeeAM();

            var context = _sqlExecuter.GetTLSDBContext();

            var query = (from pf in context.ProfessionalFees
                         join t in context.TimeSheets on pf.TimeSheetID equals t.Id
                         join j in context.TaskNames on pf.JobCodeID equals j.Id
                         join c in context.Claims on t.ClaimID equals c.Id
                         join em in context.Employees on pf.CreateBy equals em.Id
                         where t.Id == TimeSheetID
                            //&& pf.CreateBy == userid
                            && (roleName == "Admin" || allpfmngt || (allpfmngtAM && (c.AccountManagerID == userId)) || pf.CreateBy == userId) //0 cho user thuoc group admin
                         select new ProfessionalFeeView
                         {
                             ProfessionalFeeID = pf.Id,
                             TimeSheetID = pf.TimeSheetID.Value,
                             InputDate = pf.InputDate.Value,
                             JobCodeID = pf.JobCodeID,
                             Notes = pf.Notes,
                             //WorkingHour = pf.WorkingHour,
                             //ApprovedHour = pf.ApprovedHour,
                             CreateDate = pf.CreateDate.Value,
                             CreateBy = pf.CreateBy.Value,
                             UpdateBy = pf.UpdateBy.Value,
                             UpdateDate = pf.UpdateDate.Value,
                             JobName = j.Name,
                             StandardTime = j.StandardTime,
                             CreateByName = em.Name,
                             //hvtam-27082015
                             //FeePerHour = (from e in context.Employees where e.ID == pf.CreateBy select e.Fee).FirstOrDefault().Value,
                             FeePerHour = pf.FeePerHour.Value,
                             //endhvtam-27082015
                             ProFeeValue = pf.ProFeeValue.Value,

                             WorkTime = pf.WorkTime,
                             ApproveTime = pf.ApproveTime,

                             isOwner = (pf.CreateBy == userId || roleName == "Admin") ? true : false,

                             //}).OrderByDescending(t => t.CreateDate); //hvtam-22082015
                         }).OrderByDescending(t => t.InputDate);
            return query.ToList();

        }
        public List<ExpenseView> GetUserExpense(int TimeSheetID)
        {
            //TODO check permission
            //if (HasPermission(PageCapability.Timesheet_Enter)) //user right
            //{
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

            //int userid = (int)GetCurrentUser().Id;
            //userid = 0;
            //var context = _sqlExecuter.GetTLSDBContext();

            var query = (from e in context.Expenses
                         from et in context.ExpenseTypes
                         from t in context.TimeSheets
                         where e.ExpenseTypeID == et.Id && e.TimeSheetID == t.Id
                         //&& e.ID == ExpenseID
                         && t.Id == TimeSheetID
                         && (curr_role == "Admin" || e.CreatedBy == curr_user.EmployeeId) //0 cho user thuoc group admin
                         select new ExpenseView
                         {
                             ExpenseID = e.Id,
                             Description = e.Description,
                             RefNbr = e.RefNbr,
                             Notes = e.Notes,
                             Amount = e.Amount,
                             ClaimID = e.ClaimID,
                             ExpenseTypeID = e.ExpenseTypeID,
                             ExpenseTypeName = et.Name,
                             InputDate = e.InputDate.Value,
                             CreatedBy = e.CreatedBy,
                             CreatedDate = e.CreatedDate,
                             UpdatedBy = e.UpdatedBy,
                             UpdatedDate = e.UpdatedDate,
                             IsEditable = (t.IsIssued == false),

                             TimeSheetID = t.Id,
                             TimeSheetName = t.TimeSheetName,
                             TSCode = t.TSCode,
                             TSSeqNo = t.TSSeqNo.Value,
                         }).OrderByDescending(e => e.InputDate);
            return query.ToList();


            //}

        }
        public string SaveFee(CreateOrUpdateFeeInput input)
        {
            string wkt = input.WorkTime;
            var currUser = GetCurrentUser();
            ProfessionalFee fee;
            if (input.ProfessionalFeeID.HasValue)
            {
                //update
                fee = _professionalFeeRepo.Get(input.ProfessionalFeeID.Value);
            }
            else
            {
                //create
                fee = new ProfessionalFee();
                fee.CreateBy = (int)currUser.EmployeeId;
                fee.CreateDate = DateTime.Now;
                fee.ChargedBy = (int)currUser.EmployeeId;
            }

            fee.UpdateBy = (int)currUser.EmployeeId;
            fee.UpdateDate = DateTime.Now;
            fee.TimeSheetID = input.TimeSheetID;
            fee.JobCodeID = input.JobCodeID;
            fee.Notes = input.Notes.Trim();
            fee.InputDate = input.InputDate;
            fee.WorkTime = ToTimeSpan(input.WorkTime);
            fee.ApproveTime = ToTimeSpan(input.WorkTime);
            fee.FeePerHour = _employeeRepo.Get((int)currUser.EmployeeId).Fee;            
            fee.ChargedFeePerHour = fee.FeePerHour;
            fee.ChargedProFeeValue = ProfeeValueCalUSD(fee.WorkTime, fee.ChargedFeePerHour);

            decimal decimalworktime = CConvert.ToDecimal(fee.WorkTime.Value.Hours) + CConvert.ToDecimal(fee.WorkTime.Value.Minutes) / 60; //hvtam-16122014: around de lam trong, ko thi bi sai        return 
            fee.ProFeeValue = Math.Round((decimalworktime * fee.FeePerHour.Value), 2);

            var newId = _professionalFeeRepo.InsertOrUpdateAndGetId(fee);

            UpdateTimesheet(fee.TimeSheetID.Value);

            return "ok";
        }

        public string SaveExpense(CreateOrUpdateExpenseInput input)
        {

            var currUser = GetCurrentUser();
            Expense obj;
            if (input.ExpenseID.HasValue)
            {
                //update
                obj = _expenseRepo.Get(input.ExpenseID.Value);
            }
            else
            {
                //create
                obj = new Expense();
                obj.CreatedBy = (int)currUser.EmployeeId;
                obj.CreatedDate = DateTime.Now;
            }
            obj.TimeSheetID = input.TimeSheetID;
            
            if (input.InputDate == DateTime.MinValue)
                obj.InputDate = null;
            else
                obj.InputDate = input.InputDate;
            obj.Description = input.Description;
            obj.Amount = input.Amount;
            obj.ExpenseTypeID = input.ExpenseTypeID;
            var newId = _expenseRepo.InsertOrUpdateAndGetId(obj);

            UpdateTimesheet(obj.TimeSheetID.Value);
            return "ok";
        }

        /// <summary>
        /// string hh:mm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private TimeSpan ToTimeSpan(string obj)
        {
            if (obj.Length != 4) return new TimeSpan(1, 0, 0);
            var Arr = obj.Take(2);

            var hour = CConvert.ToInt(obj.Substring(0, 2));
            var min = CConvert.ToInt(obj.Substring(2, 2));
            hour = (hour > 23 ? 0 : hour);
            min = (min > 60 ? 0 : min);

            return new TimeSpan(hour, min, 0);
        }

        private TimeSpan ToTimeSpan2(string obj)
        {
            int hour = 1;
            int min = 0;
            if (obj.Length == 4)
            {
                hour = CConvert.ToInt(obj.Substring(0, 2));
                min = CConvert.ToInt(obj.Substring(2, 2));
                hour = (hour > 23 ? 0 : hour);
                min = (min > 60 ? 0 : min);
            } else
            {
                if (obj.Length == 8)
                {
                    hour = CConvert.ToInt(obj.Substring(0, 2));
                    min = CConvert.ToInt(obj.Substring(3, 2));
                    hour = (hour > 23 ? 0 : hour);
                    min = (min > 60 ? 0 : min);
                }
            }
            return new TimeSpan(hour, min, 0);
        }

        public string DeleteFee(int ProfessionalFeeID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Add/Edit time sheet
                    ProfessionalFee pf = context.ProfessionalFees.Find(ProfessionalFeeID);
                    if (pf == null)
                        return (string.Format("Professional Fee {0} not found in DB", ProfessionalFeeID));
                    int TimeSheetID = pf.TimeSheetID.Value;

                    context.ProfessionalFees.Remove(pf);
                    context.SaveChanges();
                    //UpdateProTemp(TimeSheetID);
                    #region 20140919 - DongNT Update ProFee temp

                    UpdateTimesheet(TimeSheetID);
                    #endregion

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "Error";
                }
                return "ok";
            }
        }
        public string DeleteExpense(int expenseID)
        {
            var e = _expenseRepo.Get(expenseID);
            _expenseRepo.Delete(e);
            CurrentUnitOfWork.SaveChanges();
            UpdateTimesheet(e.TimeSheetID.Value);
            return "ok";

        }

        private void UpdateTimesheet(int TimeSheetID)
        {
            //TimeSheet ts = context.TimeSheets.Find(TimeSheetID);
            TimeSheet ts = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (ts != null)
            {
                decimal TaxRate = 10m;

                List<ProfessionalFee> lst = _professionalFeeRepo.GetAll()
                    .Where(x => x.TimeSheetID == TimeSheetID).ToList();
                List<ProfessionalFeeView> lstPF = lst.Select(p =>
                    new ProfessionalFeeView { ApproveTime = (p.ApproveTime == null ? p.WorkTime : p.ApproveTime), FeePerHour = p.FeePerHour })
                .ToList();

                //var query = from p in context.ProfessionalFees
                //            where p.TimeSheetID == TimeSheetID
                //            select new ProfessionalFeeView
                //            {
                //                //ApprovedHour = (p.ApprovedHour == null ? p.WorkingHour : p.ApprovedHour), hvtam-21042015
                //                ApproveTime = (p.ApproveTime == null ? p.WorkTime : p.ApproveTime), //hvtam-21042015
                //                                                                                    //hvtam-27082015
                //                                                                                    //FeePerHour = (from e in context.Employees where e.ID == pf.CreateBy select e.Fee).FirstOrDefault().Value,
                //                FeePerHour = p.ProFeeValue,
                //                //endhvtam-27082015

                //            };
                //List<ProfessionalFeeView> lstPF = query.ToList();
                decimal ExchangeRate = ts.ExchangeRate.Value;
                decimal ProFeeAMT = 0;
                foreach (ProfessionalFeeView v in lstPF)
                {
                    //ProFeeAMT += CConvert.ToDecimal(v.ApprovedHour) * CConvert.ToDecimal(v.FeePerHour) * ExchangeRate; //hvtam-21042015
                    ProFeeAMT += ProfeeValueCalVND(v.ApproveTime, v.FeePerHour, ExchangeRate);//hvtam-21042015
                }
                //ProFeeAMT = lstPF.Sum(p => p.ApprovedHour * p.FeePerHour).Value * ExchangeRate;
                decimal ExpenseAMT = CConvert.ToDecimal(_expenseRepo.GetAll().Where(e => e.TimeSheetID == TimeSheetID).Sum(o => o.Amount));

                decimal DiscountAMT = 0;
                if (ts.DiscountVal.GetValueOrDefault(0) > 0)
                    DiscountAMT = (ts.DiscountType == "P" ? CConvert.ToDecimal(ts.DiscountVal) * ProFeeAMT / 100 : CConvert.ToDecimal(ts.DiscountVal));
                decimal ProFeeGrandAMT = ProFeeAMT - DiscountAMT;

                decimal TaxAMT = Math.Round((ProFeeGrandAMT + ExpenseAMT) * TaxRate / 100, MidpointRounding.AwayFromZero);
                decimal GrandAMT = Math.Round(ProFeeGrandAMT + ExpenseAMT + TaxAMT, MidpointRounding.AwayFromZero);

                ts.ProFeeAMT = ProFeeAMT;
                ts.DiscountAMT = DiscountAMT;
                ts.ProFeeGrandAMT = ProFeeGrandAMT;
                ts.ExpenseAMT = ExpenseAMT;
                ts.TaxAMT = TaxAMT;
                ts.GrandAMT = GrandAMT;
            }
        }



        //hvtam-Calculate ProfeeValue
        private decimal ProfeeValueCalVND(TimeSpan? worktime, decimal? feeperhour, decimal exchangerate)
        {


            decimal decimalworktime = CConvert.ToDecimal(worktime.Value.Hours) + Math.Round(CConvert.ToDecimal(worktime.Value.Minutes) / 60, 2); //hvtam-16122014: around de lam trong, ko thi bi sai        return 
            return (decimalworktime * exchangerate * feeperhour.Value);

        }

        public string Issued(int TimeSheetID, DateTime IssueDate)
        {
            //decimal TaxRate = 10m;
            var currUser = GetCurrentUser();

            TimeSheet tsInfo = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (tsInfo == null)
                throw new Exception("TimeSheetID not found in DB");

            tsInfo.IsIssued = true;
            tsInfo.IssueDate = IssueDate;
            tsInfo.UpdateBy = (int)currUser.EmployeeId;
            tsInfo.UpdateDate = DateTime.Now;


            //decimal ExpenseAMT = _expenseRepo.GetAll().Where(e => e.TimeSheetID == TimeSheetID).Sum(o => o.Amount) ?? 0 * tsInfo.ExchangeRate ?? 0;
            //ExpenseAMT = Math.Round(ExpenseAMT, MidpointRounding.AwayFromZero);

            //decimal ProFeeAMT = _professionalFeeRepo.GetAll().Where(e => e.TimeSheetID == TimeSheetID).Sum(o => o.ChargedProFeeValue) ?? 0 * tsInfo.ExchangeRate ?? 0;
            //ProFeeAMT = Math.Round(ProFeeAMT, MidpointRounding.AwayFromZero); //hvtam-15092015


            //decimal DiscountAMT = 0;
            //if (tsInfo.DiscountVal.GetValueOrDefault(0) > 0)
            //    DiscountAMT = (tsInfo.DiscountType == "P" ? tsInfo.DiscountVal.Value * ProFeeAMT / 100 : tsInfo.DiscountVal.Value);
            //DiscountAMT = Math.Round(DiscountAMT, MidpointRounding.AwayFromZero); //hvtam-15092015
            //decimal ProFeeGrandAMT = ProFeeAMT - DiscountAMT;

            //decimal TaxAMT = Math.Round((ProFeeGrandAMT + ExpenseAMT) * TaxRate / 100, MidpointRounding.AwayFromZero);

            //decimal GrandAMT = Math.Round(ProFeeGrandAMT + ExpenseAMT + TaxAMT, MidpointRounding.AwayFromZero);

            //tsInfo.ProFeeAMT = ProFeeAMT;
            //tsInfo.DiscountAMT = DiscountAMT;
            //tsInfo.ProFeeGrandAMT = ProFeeGrandAMT;
            //tsInfo.ExpenseAMT = ExpenseAMT;
            //tsInfo.TaxAMT = TaxAMT;
            //tsInfo.GrandAMT = GrandAMT;

            return "ok";
        }


        public string UnIssued(int TimeSheetID)
        {
            var currUser = GetCurrentUser();
            TimeSheet tsInfo = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (tsInfo == null)
                return "TimeSheetID not found in DB";
            //throw new ClaimException(string.Format("TimeSheet {0} not found in DB", p_info.TimeSheetID));

            if (tsInfo.IsInvoiced.Value)
                return string.Format("TimeSheet '{0}' has been Invoice.", tsInfo.TimeSheetName);
            //throw new ClaimException(string.Format("TimeSheet '{0}' has been Invoice.", tsInfo.TimeSheetName));

            tsInfo.IsIssued = false;
            tsInfo.IssueDate = null;
            tsInfo.UpdateBy = (int)currUser.EmployeeId;
            tsInfo.UpdateDate = DateTime.Now;

            tsInfo.IsDebitNote = false;
            tsInfo.DebitNoteCode = string.Empty;
            tsInfo.DebitNoteDate = DateTime.Now;
            return "ok";
        }

        public TimeSheetView GetTSInfo(int TimeSheetID)
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

            bool CheckGetAllFee = checkPermission_ViewAllProFee();
            bool CheckGetAllFeeAM = checkPermission_ViewAllProFeeAM();
            bool getAllFee = false;
            if (curr_role == "Admin" || CheckGetAllFee)
            {
                getAllFee = true;
            }

            var query = (from t in context.TimeSheets
                         join c in context.Claims on t.ClaimID equals c.Id
                         where t.Id == TimeSheetID
                         select new TimeSheetView
                         {
                             TimeSheetID = t.Id,
                             ClaimID = t.ClaimID,
                             TimeSheetName = t.TimeSheetName,
                             CreateBy = t.CreateBy.Value,
                             CreatedDate = t.CreateDate.Value,
                             UpdateBy = t.UpdateBy.Value,
                             UpdateDate = t.UpdateDate.Value,

                             IsIssued = t.IsIssued.Value,

                             IsDebitNote = t.IsDebitNote.Value,
                             DebitNoteCode = t.DebitNoteCode,
                             DebitNoteDate = t.DebitNoteDate.Value,
                             DebitNoteBy = t.DebitNoteBy.Value,
                             DebitNoteByName = (from e in context.Employees where e.Id == t.DebitNoteBy select e.Name).FirstOrDefault(),
                             TSCode = t.TSCode,
                             TSSeqNo = t.TSSeqNo.Value,

                             IsInvoiced = t.IsInvoiced.Value,
                             InvoiceAMT = t.InvoiceAMT ?? 0,
                             InvoiceCode = t.InvoiceCode,
                             IsSubmited = t.IsSubmited,
                             SubmitBy = t.SubmitBy,
                             SubmitByName = (from e in context.Employees where e.Id == t.SubmitBy select e.Name).FirstOrDefault(),

                             CreateByName = (from e in context.Employees where e.Id == t.CreateBy select e.Name).FirstOrDefault(),
                             UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),


                             ExchangeRate = t.ExchangeRate.Value,
                             DiscountVal = t.DiscountVal == null ? 0 : t.DiscountVal.Value,
                             DiscountType = t.DiscountType == null ? "P" : t.DiscountType,
                             DiscountAMT = Math.Round(t.DiscountAMT ?? 0),

                             ExpenseAMT = (t.IsIssued == true ? t.ExpenseAMT : (context.Expenses.Where(exp => exp.TimeSheetID == t.Id).Sum(o => (decimal?)o.Amount)) ?? 0),
                             ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && (getAllFee || (pf.CreateBy == curr_user.EmployeeId) || (CheckGetAllFeeAM && (c.AccountManagerID == curr_user.EmployeeId))))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  
                             ProFeeAMTVND = Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && (getAllFee || (pf.CreateBy == curr_user.EmployeeId) || (CheckGetAllFeeAM && (c.AccountManagerID == curr_user.EmployeeId))))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0)), //hvtam-22042015

                             //ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.Id)))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  
                             //ProFeeAMTVND = (t.IsIssued == true ? t.ProFeeAMT : Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.Id)))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0))), //hvtam-22042015

                             ProFeeGrandAMT = Math.Round(t.ProFeeGrandAMT ?? 0), //Neu db co gia tri thi lay theo db, ko thi lay theo cong thuc trong (ProFeeAMT- DiscountAMT)
                             TaxAMT = Math.Round(t.TaxAMT ?? 0),
                             GrandAMT = Math.Round(t.GrandAMT ?? 0),

                             AssignmentDate = (from e in context.Claims where e.Id == t.ClaimID select e.DateOfAssignment).FirstOrDefault(),


                         });
            TimeSheetView info = query.FirstOrDefault();
            info.ListProFee = _proService.GetListProfeeofTimesheet(info.TimeSheetID);
            info.ListExpense = _expService.GetListExpensesOfTimeSheet(info.TimeSheetID);
            info.ActualFeeVND = info.ListProFee.Sum(o =>
            {
                //var actualFeeVND = ProfeeValueCalVND(o.ApproveTime, o.ChargedFeePerHour, info.ExchangeRate.Value);
                var actualFeeVND = ProfeeValueCalVND(o.WorkTime, o.FeePerHour, info.ExchangeRate.Value);
                return actualFeeVND;
            });
            return info;

        }

        public TimeSheetView GetTSInfoByEmp(int TimeSheetID, int EmployeeID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            
            var query = (from t in context.TimeSheets
                         join c in context.Claims on t.ClaimID equals c.Id
                         where t.Id == TimeSheetID
                         select new TimeSheetView
                         {
                             TimeSheetID = t.Id,
                             ClaimID = t.ClaimID,
                             TimeSheetName = t.TimeSheetName,
                             CreateBy = t.CreateBy.Value,
                             CreatedDate = t.CreateDate.Value,
                             UpdateBy = t.UpdateBy.Value,
                             UpdateDate = t.UpdateDate.Value,

                             IsIssued = t.IsIssued.Value,

                             IsDebitNote = t.IsDebitNote.Value,
                             DebitNoteCode = t.DebitNoteCode,
                             DebitNoteDate = t.DebitNoteDate.Value,
                             DebitNoteBy = t.DebitNoteBy.Value,
                             DebitNoteByName = (from e in context.Employees where e.Id == t.DebitNoteBy select e.Name).FirstOrDefault(),
                             TSCode = t.TSCode,
                             TSSeqNo = t.TSSeqNo.Value,

                             IsInvoiced = t.IsInvoiced.Value,
                             InvoiceAMT = t.InvoiceAMT ?? 0,
                             InvoiceCode = t.InvoiceCode,
                             IsSubmited = t.IsSubmited,
                             SubmitBy = t.SubmitBy,
                             SubmitByName = (from e in context.Employees where e.Id == t.SubmitBy select e.Name).FirstOrDefault(),

                             CreateByName = (from e in context.Employees where e.Id == t.CreateBy select e.Name).FirstOrDefault(),
                             UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),


                             ExchangeRate = t.ExchangeRate.Value,
                             DiscountVal = t.DiscountVal == null ? 0 : t.DiscountVal.Value,
                             DiscountType = t.DiscountType == null ? "P" : t.DiscountType,
                             DiscountAMT = Math.Round(t.DiscountAMT ?? 0),

                             ExpenseAMT = ((context.Expenses.Where(exp => exp.TimeSheetID == t.Id).Sum(o => (decimal?)o.Amount)) ?? 0),
                             ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && (pf.CreateBy == EmployeeID))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  
                             ProFeeAMTVND = (Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && (pf.CreateBy == EmployeeID))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0))), //hvtam-22042015

                             //ProFeeAMTUSD = ((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.Id)))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0), //hvtam-22042015  
                             //ProFeeAMTVND = (t.IsIssued == true ? t.ProFeeAMT : Math.Round(((context.ProfessionalFees.Where(pf => ((pf.TimeSheetID == t.Id) && ((curr_role == "Admin") || (pf.CreateBy == curr_user.Id)))).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0))), //hvtam-22042015

                             //ProFeeGrandAMT = Math.Round(t.ProFeeGrandAMT ?? 0), //Neu db co gia tri thi lay theo db, ko thi lay theo cong thuc trong (ProFeeAMT- DiscountAMT)
                             TaxAMT = Math.Round(t.TaxAMT ?? 0),
                             GrandAMT = Math.Round(t.GrandAMT ?? 0),

                             AssignmentDate = (from e in context.Claims where e.Id == t.ClaimID select e.DateOfAssignment).FirstOrDefault(),


                         });
            TimeSheetView info = query.FirstOrDefault();
            info.ListProFee = _proService.GetListProfeeofTimesheetByEmp(info.TimeSheetID, EmployeeID);
            info.ListExpense = _expService.GetListExpensesOfTimeSheet(info.TimeSheetID);
            info.ActualFeeVND = info.ListProFee.Sum(o =>
            {
                var actualFeeVND = ProfeeValueCalVND(o.WorkTime, o.ChargedFeePerHour, info.ExchangeRate.Value);
                return actualFeeVND;
            });
            return info;

        }

        public string UpdateTimesheet(UpdateTimesheetInput input)
        {
            var currUser = GetCurrentUser();
            TimeSheetView tsInfo = GetTSInfo(input.TimeSheetID);
            if (tsInfo == null)
                throw new Exception("TimeSheet not found in DB");

            tsInfo.UpdateBy = (int)currUser.EmployeeId;
            tsInfo.UpdateDate = DateTime.Now;
            if (input.ExchangeRate.HasValue)
            {
                tsInfo.ExchangeRate = input.ExchangeRate;
            }            
            if (input.DiscountVal.HasValue && input.DiscountVal.Value >= 0)
            {
                if (string.IsNullOrEmpty(input.DiscountType))
                {
                    input.DiscountType = "P";
                }
                tsInfo.DiscountType = input.DiscountType;
                tsInfo.DiscountVal = input.DiscountVal;
            }

            if (input.ListProFee.Count > 0)
            {
                var listProFeeIDs = input.ListProFee.Select(x => x.ProfessionalFeeID).ToList();
                var listDBProFee = _professionalFeeRepo.GetAll()
                    .Where(x => listProFeeIDs.Contains(x.Id))
                    .ToList();
                listDBProFee.ForEach(x =>
                {
                    var at = input.ListProFee.First(y => y.ProfessionalFeeID == x.Id).ApproveTime;
                    if (!string.IsNullOrEmpty(at))
                    {   
                        var tsat = ToTimeSpan2(at);
                        x.ApproveTime = tsat;                        
                        x.ChargedProFeeValue = ProfeeValueCalUSD(x.ApproveTime ?? x.WorkTime, x.ChargedFeePerHour ?? x.FeePerHour);                        
                        x.UpdateBy = (int)currUser.EmployeeId;
                        x.UpdateDate = DateTime.Now;
                    }
                });
            }
            CurrentUnitOfWork.SaveChanges();
            SaveNoIssue(tsInfo);
            return "ok";
        }

        public string TransferTimesheet(UpdateTimesheetInput input)
        {
            var currUser = GetCurrentUser();
            TimeSheetView tsInfo = GetTSInfo(input.TimeSheetID);
            if (tsInfo == null)
                throw new Exception("TimeSheet not found in DB");

            tsInfo.UpdateBy = (int)currUser.EmployeeId;
            tsInfo.UpdateDate = DateTime.Now;            

            if (input.ListProFee.Count > 0)
            {
                var listProFeeIDs = input.ListProFee.Select(x => x.ProfessionalFeeID).ToList();
                var listDBProFee = _professionalFeeRepo.GetAll()
                    .Where(x => listProFeeIDs.Contains(x.Id))
                    .ToList();
                listDBProFee.ForEach(x =>
                {                    
                    var pf = input.ListProFee.First(y => y.ProfessionalFeeID == x.Id);
                    if (x.ChargedBy != pf.ChargedBy)
                    {
                        Logger.Debug("old charged " + x.ChargedProFeeValue + " " + x.Id);
                        x.ChargedBy = pf.ChargedBy;
                        x.ChargedFeePerHour = pf.ChargedFeePerHour;
                        x.ChargedProFeeValue = ProfeeValueCalUSD(x.ApproveTime ?? x.WorkTime, x.ChargedFeePerHour ?? x.FeePerHour);
                        Logger.Debug("new charged " + x.ChargedProFeeValue + " " + x.Id);                        
                        x.UpdateBy = (int)currUser.EmployeeId;
                        x.UpdateDate = DateTime.Now;
                    }                    
                });
            }
            CurrentUnitOfWork.SaveChanges();
            SaveNoIssue(tsInfo);
            return "ok";
        }

        private void SaveNoIssue(TimeSheetView p_info)
        {
            decimal TaxRate = 10m; //hvtam-22042015
            var context = _sqlExecuter.GetTLSDBContext();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    TimeSheet tsInfo = context.TimeSheets.Find(p_info.TimeSheetID);
                    if (tsInfo == null)
                        throw new Exception("TimeSheet not found in DB");

                    tsInfo.TimeSheetName = p_info.TimeSheetName;
                    tsInfo.UpdateBy = p_info.UpdateBy;
                    tsInfo.UpdateDate = p_info.UpdateDate;

                    tsInfo.DiscountType = p_info.DiscountType;
                    tsInfo.DiscountVal = p_info.DiscountVal;
                    tsInfo.ExchangeRate = p_info.ExchangeRate;

                    _professionalFeeRepo.GetAll().Where(x => x.TimeSheetID == tsInfo.Id)
                        .ToList().ForEach(y => Logger.Debug(y.Id + " " + y.ChargedProFeeValue));
                    if(p_info.ListProFee.Count > 0)
                    {
                        decimal TotalProFee = _professionalFeeRepo.GetAll().Where(x => x.TimeSheetID == tsInfo.Id)
                        .Sum(o => o.ChargedProFeeValue).Value;
                        TotalProFee = TotalProFee * (decimal)tsInfo.ExchangeRate;
                        TotalProFee = Math.Round(TotalProFee, MidpointRounding.AwayFromZero);
                        decimal ExpenseAMT = p_info.ListExpense.Sum(o => o.Amount).Value;
                        ExpenseAMT = Math.Round(ExpenseAMT, MidpointRounding.AwayFromZero);//hvtam-15092015

                        decimal DiscountAMT = 0;
                        if (p_info.DiscountVal.GetValueOrDefault(0) > 0)
                            DiscountAMT = (p_info.DiscountType == "P" ? p_info.DiscountVal.Value * TotalProFee / 100 : p_info.DiscountVal.Value);
                        DiscountAMT = Math.Round(DiscountAMT, MidpointRounding.AwayFromZero); //hvtam-15092015
                                                                                              //hvtam-22042015: Chi luu gia tri Discount xuong
                        decimal ProFeeGrandAMT = TotalProFee - DiscountAMT;

                        decimal TaxAMT = Math.Round((ProFeeGrandAMT + ExpenseAMT) * TaxRate / 100, MidpointRounding.AwayFromZero);
                        decimal GrandAMT = Math.Round(ProFeeGrandAMT + ExpenseAMT + TaxAMT, MidpointRounding.AwayFromZero);

                        tsInfo.ProFeeAMT = TotalProFee;
                        tsInfo.DiscountAMT = DiscountAMT;
                        tsInfo.DiscountType = p_info.DiscountType;
                        tsInfo.ProFeeGrandAMT = ProFeeGrandAMT;
                        tsInfo.ExpenseAMT = ExpenseAMT;
                        tsInfo.TaxAMT = TaxAMT;
                        tsInfo.GrandAMT = GrandAMT;
                        //endhvtam-22042015: Chi luu gia tri Discount xuong
                        context.SaveChanges();
                    }
                    else
                    {                        
                        context.SaveChanges();
                    }
                    

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }

        }
        private decimal ProfeeValueCalUSD(TimeSpan? worktime, decimal? feeperhour)
        {

            decimal decimalworktime = CConvert.ToDecimal(worktime.Value.Hours) + CConvert.ToDecimal(worktime.Value.Minutes) / 60; //hvtam-16122014: around de lam trong, ko thi bi sai        return 
            return Math.Round((decimalworktime * feeperhour.Value), 2);

        }

        public string CreateTimesheet(CreateTimesheetInput input)
        {
            var currUser = GetCurrentUser();
            TimeSheet info = new TimeSheet();
            info.ClaimID = input.ClaimID;
            info.TSCode = "TS";
            info.TSSeqNo = input.TSSeqNo;
            info.TimeSheetName = input.TimeSheetName;
            info.ExchangeRate = input.ExchangeRate;
            info.CreateBy = (int)currUser.EmployeeId;
            info.CreateDate = DateTime.Now;
            info.UpdateBy = info.CreateBy; ;
            info.UpdateDate = DateTime.Now;
            info.IsIssued = false;
            info.IsInvoiced = false;


            info.IsDebitNote = false;
            info.DebitNoteBy = 0;
            info.InvoiceBy = 0;
            var ts = _timesheetRepo.InsertAndGetId(info);
            return string.Format("The Timesheet {0} has been created successfully", info.TimeSheetName);
        }
        public string DeleteTimeSheet(int TimeSheetID)
        {
            TimeSheet tsInfo = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (tsInfo == null) return (string.Format("TimeSheet {0} not found in DB", TimeSheetID));
            if (tsInfo.IsDebitNote.Value) return (string.Format("TimeSheet '{0}' has debit note.", tsInfo.TimeSheetName)); ; // Have DebitNote
            if (tsInfo.IsInvoiced.Value) return (string.Format("TimeSheet '{0}' has been Invoice.", tsInfo.TimeSheetName));
            // Check Have Professional Fee
            int CountProFee = _professionalFeeRepo.GetAll().Where(p => p.TimeSheetID == TimeSheetID).Count();
            if (CountProFee > 0) return string.Format("TimeSheet '{0}' has professional fee.", tsInfo.TimeSheetName);

            int CountExpense = _expenseRepo.GetAll().Where(e => e.TimeSheetID == TimeSheetID).Count();
            if (CountExpense > 0) return string.Format("TimeSheet '{0}' has expense.", tsInfo.TimeSheetName);

            tsInfo.IsDeleted = true;
            _timesheetRepo.Update(tsInfo);
            return string.Format("Delete timeSheet '{0}' successfully.", tsInfo.TimeSheetName);
        }

        public List<TimeSheetView> GetTimesheetToCreateInvoice(string claimID)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            return context.TimeSheets.Where(t => t.ClaimID == claimID
              && t.IsIssued == true && t.IsInvoiced == false)
                                   .Select(t => new TimeSheetView
                                   {
                                       TimeSheetID = t.Id,
                                       ClaimID = t.ClaimID,
                                       TimeSheetName = t.TimeSheetName,
                                       IsIssued = t.IsIssued.Value,
                                       IsInvoiced = t.IsInvoiced.Value,

                                       CreateBy = t.CreateBy.Value,
                                       CreatedDate = t.CreateDate.Value,
                                       UpdateBy = t.UpdateBy.Value,
                                       UpdateDate = t.UpdateDate.Value,

                                       CreateByName = (from e in context.Employees where e.Id == t.CreateBy select e.Name).FirstOrDefault(),
                                       UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),

                                       ExpenseAMT = ((context.Expenses.Where(exp => exp.TimeSheetID == t.Id).Sum(o => (decimal?)o.Amount)) ?? 0),
                                       ProFeeAMTVND = (Math.Round(((context.ProfessionalFees.Where(pf => pf.TimeSheetID == t.Id).Sum(o => o.ChargedProFeeValue != null ? (decimal?)o.ChargedProFeeValue : (decimal?)o.ProFeeValue)) ?? 0) * (t.ExchangeRate ?? 0))), //hvtam-22042015

                                       ExchangeRate = t.ExchangeRate.Value,

                                       DiscountVal = t.DiscountVal.Value,
                                       DiscountType = t.DiscountType,
                                       DiscountAMT = Math.Round(t.DiscountAMT ?? 0),

                                       ProFeeGrandAMT = Math.Round(t.ProFeeGrandAMT.Value),

                                       TaxAMT = Math.Round(t.TaxAMT.Value),
                                       GrandAMT = Math.Round(t.GrandAMT.Value),

                                       IsDebitNote = t.IsDebitNote.Value,
                                       DebitNoteCode = t.DebitNoteCode,
                                       DebitNoteDate = t.DebitNoteDate.Value,
                                       DebitNoteBy = t.DebitNoteBy.Value,
                                       DebitNoteByName = (from e in context.Employees where e.Id == t.DebitNoteBy select e.Name).FirstOrDefault(),
                                       TSCode = t.TSCode,
                                       TSSeqNo = t.TSSeqNo.Value,
                                       InvoiceAMT = t.InvoiceAMT,
                                   })
                                    .ToList();

        }

        /// <summary>
        /// copy from old project
        /// </summary>
        /// <param name="InvoiceID"></param>
        /// <returns></returns>
        public List<TimeSheetView> GetListByInvoiceID(int InvoiceID)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            var query = from t in context.TimeSheets
                        from id in context.Invoice_Timesheets
                        where t.Id == id.TimeSheetID
                        && id.InvoiceID == InvoiceID
                        select new TimeSheetView
                        {
                            TimeSheetID = t.Id,
                            ClaimID = t.ClaimID,
                            TimeSheetName = t.TimeSheetName,
                            IsIssued = t.IsIssued.Value,
                            IsInvoiced = t.IsInvoiced.Value,

                            CreateBy = t.CreateBy.Value,
                            CreatedDate = t.CreateDate.Value,
                            UpdateBy = t.UpdateBy.Value,
                            UpdateDate = t.UpdateDate.Value,

                            CreateByName = (from e in context.Employees where e.Id == t.CreateBy select e.Name).FirstOrDefault(),
                            UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),

                            ProFeeAMTVND = t.ProFeeAMT.Value,
                            ExchangeRate = t.ExchangeRate.Value,
                            DiscountVal = t.DiscountVal.Value,
                            DiscountType = t.DiscountType,
                            DiscountAMT = t.DiscountAMT ?? 0,

                            ProFeeGrandAMT = t.ProFeeGrandAMT.Value,
                            ExpenseAMT = t.ExpenseAMT.Value,
                            TaxAMT = t.TaxAMT.Value,
                            GrandAMT = t.GrandAMT.Value,

                            IsDebitNote = t.IsDebitNote.Value,
                            DebitNoteCode = t.DebitNoteCode,
                            DebitNoteDate = t.DebitNoteDate.Value,
                            DebitNoteBy = t.DebitNoteBy.Value,
                            DebitNoteByName = (from e in context.Employees where e.Id == t.DebitNoteBy select e.Name).FirstOrDefault(),

                            TSCode = t.TSCode,
                            TSSeqNo = t.TSSeqNo.Value
                        };
            return query.ToList();

        }

        public string IsSubmit(int TimeSheetID)
        {
            var currUser = GetCurrentUser();
            TimeSheet tsInfo = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (tsInfo == null)
                return "TimeSheetID not found in DB";
            //throw new ClaimException(string.Format("TimeSheet {0} not found in DB", p_info.TimeSheetID));

            tsInfo.IsSubmited = true;
            tsInfo.SubmitDate = DateTime.Now;
            tsInfo.SubmitBy = (int)currUser.EmployeeId;
            return "ok";
        }

        public string UnIsSubmit(int TimeSheetID)
        {
            var currUser = GetCurrentUser();
            TimeSheet tsInfo = _timesheetRepo.FirstOrDefault(TimeSheetID);
            if (tsInfo == null)
                return "TimeSheetID not found in DB";
            //throw new ClaimException(string.Format("TimeSheet {0} not found in DB", p_info.TimeSheetID));

            tsInfo.IsSubmited = false;
            tsInfo.SubmitDate = DateTime.Now;
            tsInfo.SubmitBy = (int)currUser.EmployeeId;
            return "ok";
        }
        public List<TimeSheetView> GetListApproved()
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
                curr_role = role.Name;
            }

            var query = from t in context.TimeSheets
                        join ec in context.EmployeeClaims on t.ClaimID equals ec.ClaimID
                        where t.IsSubmited == true && t.IsIssued == false && ((curr_role == "Admin") || (ec.EmployeeID == curr_user.EmployeeId))
                        select new TimeSheetView
                        {                          
                            ClaimID = t.ClaimID,
                            TimeSheetName = t.TimeSheetName,                 
                            UpdateByName = (from e in context.Employees where e.Id == t.UpdateBy select e.Name).FirstOrDefault(),
                            SubmitDate = t.SubmitDate,
                            SubmitByName = (from e in context.Employees where e.Id == t.SubmitBy select e.Name).FirstOrDefault(),
                        };
            return query.Distinct().ToList();
        }

        public List<VClaimInsurerDto> GetListSubmission()
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

            var query = from cp in context.ClaimProcess
                        join c in context.Claims on cp.ClaimID equals c.Id
                        join cus in context.Customers on c.InsurerID equals cus.Id
                        join ec in context.EmployeeClaims on cp.ClaimID equals ec.ClaimID
                        join user in context.Users on (int)c.AccountManagerID equals user.EmployeeId
                        where ((cp.IsFINL1 == true) && (cp.IsSubmit == null))
                        && (c.RefStatusID != 2) && ((curr_role == "Admin") || (ec.EmployeeID == curr_user.EmployeeId))
                        select new VClaimInsurerDto
                        {
                            ID = cp.ClaimID,
                            InsurerName = cus.BrandName,
                            TheInsured = c.TheInsured,
                            AccManName = user.UserName,
                        };
            //var query = from c in context.Claims
            //            join cus in context.Customers on c.InsurerID equals cus.Id
            //            join t in context.TimeSheets on c.Id equals t.ClaimID
            //            join ec in context.EmployeeClaims on t.ClaimID equals ec.ClaimID
            //            join user in context.Users on (int)c.AccountManagerID equals user.EmployeeId
            //            where (c.RefStatusID != 2) && (t.IsSubmited == false) && ((curr_role == "Admin") || (ec.EmployeeID == curr_user.EmployeeId))
            //            select new VClaimInsurerDto
            //            {
            //                ID = t.ClaimID,
            //                InsurerName = cus.BrandName,
            //                TheInsured = c.TheInsured,
            //                AccManName = user.UserName,                            
            //            };
            return query.Distinct().ToList();
        }

        public bool checkPermission_AMManagementTimeSheet()
        {
            bool am_permission = false;
            var curr_user = GetCurrentUser();
            var permission_amclaim = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageAMTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            am_permission = permissionlist.Result.Contains(permission_amclaim);            
            return am_permission;
        }

        public bool checkPermission_MyManagementTimeSheet()
        {
            bool my_permission = false;
            var curr_user = GetCurrentUser();
            var permission_myclaim = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageMyTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            my_permission = permissionlist.Result.Contains(permission_myclaim);            
            return my_permission;
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

        public bool checkPermission_CreateTimeSheet()
        {
            bool create_permission = false;
            var curr_user = GetCurrentUser();
            var permission_createtimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_CreateTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            create_permission = permissionlist.Result.Contains(permission_createtimesheet);            
            return create_permission;
        }

        public bool checkPermission_EnterTimeSheet()
        {
            bool enter_permission = false;
            var curr_user = GetCurrentUser();
            var permission_entertimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_EnterMyProfFeeExpense);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            enter_permission = permissionlist.Result.Contains(permission_entertimesheet);            
            return enter_permission;
        }

        public bool checkPermission_IssueTimeSheet()
        {
            bool issue_permission = false;
            var curr_user = GetCurrentUser();
            var permission_issuetimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_IssueMyTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            issue_permission = permissionlist.Result.Contains(permission_issuetimesheet);            
            return issue_permission;
        }

        public bool checkPermission_DeleteTimeSheet()
        {
            bool delete_permission = false;
            var curr_user = GetCurrentUser();
            var permission_deletetimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_DeleteTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            delete_permission = permissionlist.Result.Contains(permission_deletetimesheet);            
            return delete_permission;
        }

        public bool checkPermission_EditTimeSheet()
        {
            bool edit_permission = false;
            var curr_user = GetCurrentUser();
            var permission_edittimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_EditTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            edit_permission = permissionlist.Result.Contains(permission_edittimesheet);            
            return edit_permission;
        }

        public bool checkPermission_TransferTimeSheet()
        {
            bool trans_permission = false;
            var curr_user = GetCurrentUser();
            var permission_transtimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_TransferTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            trans_permission = permissionlist.Result.Contains(permission_transtimesheet);            
            return trans_permission;
        }

        public bool checkPermission_SubmitTimeSheet()
        {
            bool submit_permission = false;
            var curr_user = GetCurrentUser();
            var permission_submittimesheet = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_SubmitTimesheet);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            submit_permission = permissionlist.Result.Contains(permission_submittimesheet);            
            return submit_permission;
        }

        public bool checkPermission_InvoiceTimeSheet()
        {
            bool invoice_permission = false;
            var curr_user = GetCurrentUser();
            var permission_invoice = PermissionManager.GetPermission(AppPermissions.Pages_FinanceAccounting_CreateInvoice);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            invoice_permission = permissionlist.Result.Contains(permission_invoice);            
            return invoice_permission;
        }

        public bool checkPermission_IssuedTimeSheetOnly()
        {
            bool issuedonly_permission = false;
            var curr_user = GetCurrentUser();
            var permission_issuedonly = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageIssuedTimesheetOnly);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            issuedonly_permission = permissionlist.Result.Contains(permission_issuedonly);            
            return issuedonly_permission;
        }

        public bool checkPermission_NotIssuedTimeSheet()
        {
            bool notissuedonly_permission = false;
            var curr_user = GetCurrentUser();
            var permission_notissuedonly = PermissionManager.GetPermission(AppPermissions.Pages_TimesheetsManagement_ManageNotIssuedTimesheetOnly);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            notissuedonly_permission = permissionlist.Result.Contains(permission_notissuedonly);            
            return notissuedonly_permission;
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
    }
}
