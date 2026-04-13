using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Dto;
using System.Configuration;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.Authorization;
using triluatsoft.tls.ClaimManagement;
using triluatsoft.tls.Net.MimeTypes;
using triluatsoft.tls.DataExporting;

namespace triluatsoft.tls.OldTable
{
    public class ClaimAppService : tlsAppServiceBase, IClaimAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Claim, string> _claimTableRepo;
        private readonly IRepository<CoOwnerClaim> _coOwnerRepo;
        private readonly IRepository<EmployeeClaim> _employeeClaimRepo;
        private readonly IRepository<Bordereaux> _borderauxRepo;
        private readonly IRepository<BordereauxStatus, string> _borderauxStatusRepo;
        private readonly ITimeSheetAppService _timesheetService;
        private readonly ICustomerAppService _customerService;
        private readonly IEmployeeAppService _employeeService;
        private readonly IRepository<Report> _reportRepo;
        private readonly IRepository<FollowUp> _followUpRepo;
        private readonly IRepository<F> _tblFolderRepository;
        private readonly ITasMailAppService _emailSenderVAJ;
        private readonly IRepository<ClaimProcess> _claimProcess;
        public IAppFolders AppFolders { get; set; }

        public ClaimAppService(ISqlExecuter sqlExecuter, IRepository<Claim, string> claimTableRepo
            , IRepository<CoOwnerClaim> coOwnerRepo
            , IRepository<EmployeeClaim> employeeClaimRepo
            , IRepository<Bordereaux> borderauxRepo
            , IRepository<BordereauxStatus, string> borderauxStatusRepo
            , ITimeSheetAppService timesheetService
            , ICustomerAppService customerService
            , IEmployeeAppService employeeService
            , IRepository<Report> reportRepo
            , IRepository<FollowUp> followUpRepo
            , IRepository<F> tblFolderRepository
            , ITasMailAppService emailSenderVAJ
            , IRepository<ClaimProcess> claimProcess            
            )
        {
            _sqlExecuter = sqlExecuter;
            _claimTableRepo = claimTableRepo;
            _coOwnerRepo = coOwnerRepo;
            _employeeClaimRepo = employeeClaimRepo;
            _borderauxRepo = borderauxRepo;
            _timesheetService = timesheetService;
            _customerService = customerService;
            _employeeService = employeeService;
            _reportRepo = reportRepo;
            _borderauxStatusRepo = borderauxStatusRepo;
            _followUpRepo = followUpRepo;
            _tblFolderRepository = tblFolderRepository;
            _emailSenderVAJ = emailSenderVAJ;
            _claimProcess = claimProcess;            
        }
        public PagedResultDto<VClaimInsurerDto> GetAll(SearchClaimInput input)
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

            string where = " WHERE 1 = 1 ";
            //string where = " ";

            var permission_allclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_ViewAllClaim);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            var allclaim = permissionlist.Result.Contains(permission_allclaim);            

            if ((curr_role != "Admin") && !allclaim)
            {
                where += "AND (ID IN (SELECT CLaimID FROM EmployeeClaim WHERE EmployeeID = " + curr_user.EmployeeId + ")) ";
            }
            if (input.StartDate != null && input.EndDate != null)
            {
                where += string.Format("AND (DateOfAssignment >= '{0:yyyy-MM-dd HH:mm:ss}' AND DateOfAssignment < '{1:yyyy-MM-dd HH:mm:ss}' ) ", input.StartDate, input.EndDate);

            }
            if (!string.IsNullOrEmpty(input.ClaimRefId))
            {
                where += string.Format(" AND ID like '%{0}%' ", input.ClaimRefId);
            }
            if (!string.IsNullOrEmpty(input.ClaimTypeCode))
            {
                where += string.Format(" AND ClaimTypeCode = '{0}' ", input.ClaimTypeCode);
            }
            if (!string.IsNullOrEmpty(input.TheInsured))
            {
                where += string.Format(" AND TheInsured like '%{0}%' ", input.TheInsured);
            }
            if (!string.IsNullOrEmpty(input.PolicyNo))
            {
                where += string.Format(" AND PolicyNo LIKE N'%{0}%' ", input.PolicyNo);
            }
            if (input.InsurerId != -1 && input.InsurerId != null)
            {
                List<string> claims = _coOwnerRepo.GetAll().Where(x => x.CustomerID == input.InsurerId).Select(x => x.ClaimID).ToList();
                if (claims.Count == 0)
                {
                    where += string.Format(" AND InsurerID = {0} ", input.InsurerId);
                }
                else
                {
                    where += string.Format(" AND (InsurerID = {0} ", input.InsurerId);
                    claims = claims.Select(x => "'" + x + "'").ToList();
                    string claimToStr = claims.JoinAsString(",");
                    where += " OR ID IN (" + claimToStr + ") )";
                }
            }
            if (input.RefStatusId != -1 && input.RefStatusId != null)
            {
                where += string.Format(" AND RefStatusID = {0} ", input.RefStatusId);
            }
            if (input.OfficeId != -1 && input.OfficeId != null)
            {
                where += string.Format(" AND OfficeID = {0} ", input.OfficeId);
            }
            if (input.AccountManagerId != -1 && input.AccountManagerId != null)
            {
                where += string.Format(" AND AccountManagerID = {0} ", input.AccountManagerId);
            }
            if (input.CauseId != -1 && input.CauseId != null)
            {
                where += string.Format(" AND CauseID = {0} ", input.CauseId);
            }
            if (input.TypeOfLossId != -1 && input.TypeOfLossId != null)
            {
                where += string.Format(" AND TypeOfLossID = {0} ", input.TypeOfLossId);
            }
            if (input.BrokerId != -1 && input.BrokerId != null)
            {
                where += string.Format(" AND BrokerID = {0} ", input.BrokerId);
            }

            string order = " ORDER BY DateOfAssignment DESC ";
            if (input.RefStatusId == REF_STATUS_DEFINE.CLOSE)
            {

            }
            var page = Math.Max(input.Page, 1);
            var rowNumT = input.PageSize * page;
            var rowNumF = rowNumT - (input.PageSize - 1);
            rowNumF = Math.Max(rowNumF, 0);

            string from = string.Format(" (SELECT ROW_NUMBER() OVER(" + order + ") AS Row#, * FROM VClaimInsurer " + where + " ) AS ClaimOrder WHERE (Row# Between {0} AND {1}) ", rowNumF, rowNumT);
            List<VClaimInsurerDto> list = _sqlExecuter.GetDatabase().SqlQuery<VClaimInsurerDto>("Select * from  "
                + from
                + order
                //" OFFSET @p0 * (@p1 - 1) ROWS " +
                //" FETCH NEXT @p2 ROWS ONLY ", input.PageSize, input.Page, input.PageSize)
                ).ToList();

            var total = _sqlExecuter.GetDatabase().SqlQuery<int>("SELECT COUNT(ID) from VClaimInsurer " + where).First();
            var result = new PagedResultDto<VClaimInsurerDto>()
            {
                TotalCount = total,
                Items = list
            };
            return result;
        }

        public VClaimInsurerDto GetById(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return null;
            }
            Id = Id.Trim();
            VClaimInsurerDto dto = _sqlExecuter.GetDatabase().SqlQuery<VClaimInsurerDto>("Select * from VClaimInsurer "
                + "WHERE ID = @p0", Id).FirstOrDefault();

            var coIds = _coOwnerRepo.GetAll().Where(x => x.ClaimID == Id).ToList();
            List<CoOwner> coOwners = new List<CoOwner>();
            foreach (var c in coIds)
            {
                var temp = _customerService.GetById(c.CustomerID);
                coOwners.Add(new CoOwner { Id = temp.ID, Name = temp.Name, BrandName = temp.BrandName });
            }
            dto.CoOwners = coOwners;
            List<CustomerDBObj> listIns = _customerService.GetByType("C");
            List<CoOwner> avaiCoOwners = listIns.Select(x =>
            {
                return new CoOwner { Id = x.ID, Name = x.Name, BrandName = x.BrandName };
            }).ToList();
            avaiCoOwners.RemoveAll(x => coOwners.Contains(x));
            dto.AvaiCoOwners = avaiCoOwners;

            List<AccExec> listAccExecs = new List<AccExec>();
            var listAEs = _employeeClaimRepo.GetAll().Where(x => x.ClaimID == Id).ToList();
            foreach (var ae in listAEs)
            {
                var u = UserManager.Users.Where(x => x.EmployeeId == ae.EmployeeID).FirstOrDefault();
                if (u == null)
                {
                    Logger.Error("no employee claim for Id " + ae.Id + " " + ae.EmployeeID);
                    continue;
                }
                listAccExecs.Add(new AccExec { EmployeeId = ae.EmployeeID, Name = u.Name });
            }
            dto.AEs = listAccExecs;
            List<AccExec> listAvaiAE = _employeeService.GetEmployeesForClaim().Select(x =>
            {
                return new AccExec { EmployeeId = (int)x.EmployeeId, Name = x.Name };
            }).ToList();
            listAvaiAE = listAvaiAE.OrderBy(x => x.Name).ToList();
            listAvaiAE.RemoveAll(x => listAccExecs.Contains(x));
            dto.AvaiAEs = listAvaiAE;
            return dto;
        }
        /// <summary>
        /// create or update
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Create(CreateOrUpdateClaimInput input)
        {
            string SPName = "CreateClaimID";
            //ClaimFolderService mainSQLclass = new ClaimFolderService();


            //**********************Create claim*************************
            string claimID;
            if (input.ClaimId != null)
            {
                claimID = input.ClaimId;
            }
            else
            {
                claimID = CreateNewClaimID(SPName, input.OfficeID);
            }

            Claim claim = _claimTableRepo.Get(claimID);
            claim.OfficeID = input.OfficeID;
            claim.AccountManagerID = input.AccountManagerID;
            claim.InsurerID = input.InsurerID;
            claim.BrokerID = input.BrokerID;
            claim.CtypeID = input.CtypeID;
            claim.TheInsured = input.TheInsured?.Trim();
            claim.InsuredProject = input.InsuredProject?.Trim();
            claim.RiskLocation = input.RiskLocation?.Trim();
            claim.ClientsRef = input.ClientsRef?.Trim();
            claim.PolicyNo = input.PolicyNo?.Trim();
            //if (input.DateOfAssignment.HasValue)
            //{
            claim.DateOfAssignment = input.DateOfAssignment; //hvtam-02022016 DateOfAssignment
                                                             //}
                                                             //if (input.DateOfLoss.HasValue)
                                                             //{
            claim.DateOfLoss = input.DateOfLoss;
            //}
            claim.TypeOfLossID = input.TypeOfLossId;
            claim.CauseID = input.CauseID;

            //cause id = 18 = cause other
            if (input.CauseID == 18)
                claim.OtherCause = (input.OtherCause != null) ? input.OtherCause.Trim() : "";
            else
                claim.OtherCause = string.Empty;

            if (string.IsNullOrWhiteSpace(input.Estimate))
                claim.Estimate = null;
            else
                claim.Estimate = Convert.ToDecimal(input.Estimate);

            if (string.IsNullOrWhiteSpace(input.Currency) || input.Currency == "NON")
                claim.Currency = null;
            else
                claim.Currency = input.Currency;

            User user = GetCurrentUser();

            if (input.ClaimId == null)
            {
                claim.CreatedBy = (int)user.EmployeeId; //hvtam-28092014
                claim.UpdatedBy = claim.CreatedBy; //hvtam-28092014: Mac dinh cho lan tao dau tien de luu vao bang history
                claim.CreatedDate = DateTime.Now;  //hvtam-28092014
            }

            int claimStatus = 1;

            if (input.RefStatusID.HasValue)
            {
                claim.RefStatusID = input.RefStatusID.Value;
                claimStatus = input.RefStatusID.Value;
            }
            else
            {
                claim.RefStatusID = REF_STATUS_DEFINE.OPEN; //hvtam-23102014: mac dinh Refstatus = open
            }
            _claimTableRepo.Update(claim);

            SaveUserClaim(claimID, input.ListAE);
            SaveCoOwner(claimID, input.ListCoOwner);

            //**********************Create timesheet*************************
            if (string.IsNullOrEmpty(input.ClaimId))
            {
                _timesheetService.Create(claimID, (int)user.EmployeeId);
            }                

            //Save default status, history
            if (!claim.BordereauxID.HasValue)
            {
                Bordereaux hist = new Bordereaux();
                hist.ClaimID = claim.Id;
                hist.CreatedDate = claim.CreatedDate;
                hist.CreatedBy = claim.CreatedBy;
                var newId = _borderauxRepo.InsertAndGetId(hist);
                claim.BordereauxID = hist.Id;
            }

            CreateClaimDirectory(claimID, claimStatus);

            if (input.ClaimId == null)
            {
                DateTime deadlineTime = DateTime.Now.AddHours(2);
                ClaimProcess clpro = new ClaimProcess();
                clpro.ClaimID = claimID;
                clpro.IsAck = false;
                clpro.AckDeadline = deadlineTime;
                
                _claimProcess.InsertOrUpdate(clpro);
                List<EmployeeView> adj_list = GetEmployeeByClaim(claimID);
                VAJMailList mailList = new VAJMailList();
                foreach(EmployeeView adj in adj_list)
                {
                    if (adj.Email != null)
                    {
                        mailList.toList.Add(adj.Email);                        
                    }
                }
                mailList.toList.Add("bod@vietadjusters.com");                
                var Insurer = _customerService.GetById(claim.InsurerID.Value);
                _emailSenderVAJ.SendClaimConfirm(claimID, Insurer.BrandName, deadlineTime.ToString("HH:mm, dd-MM-yyyy"), mailList);
            }

            return claimID;
        }

        private string CreateNewClaimID(string SPName, int OfficeID)
        {

            var param1 = new SqlParameter
            {
                ParameterName = "@OfficeID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = OfficeID
            };

            //Second input parameter
            var outParam = new SqlParameter
            {
                ParameterName = "@ClaimID",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 25
            };

            //compose the SQL
            var SQLString = "EXEC CreateClaimID @OfficeID, @ClaimID OUT ";

            //Execute the stored procedure 
            var data = _sqlExecuter.GetDatabase().ExecuteSqlCommand(SQLString, param1, outParam);
            var newClaimID = (string)outParam.Value;


            return newClaimID;
        }

        private void SaveUserClaim(string claimId, List<int> listAE)
        {
            //Save user list
            foreach (int ae in listAE)
            {
                var ec = _employeeClaimRepo.FirstOrDefault(x => x.ClaimID == claimId && x.EmployeeID == ae);

                if (ec != null) continue;

                EmployeeClaim newEC = new EmployeeClaim();
                newEC.EmployeeID = ae;
                newEC.ClaimID = claimId;
                _employeeClaimRepo.Insert(newEC);
            }

            List<EmployeeClaim> ecList = _employeeClaimRepo.GetAll().Where(e => e.ClaimID == claimId).ToList();
            foreach (EmployeeClaim ec in ecList)
            {
                if (!listAE.Any(e => e == ec.EmployeeID))
                    _employeeClaimRepo.Delete(ec);
            }
        }

        private void SaveCoOwner(string claimId, List<int> listCoOwners)
        {
            //Save Co-owner
            foreach (int coowner in listCoOwners)
            {
                var ec = _coOwnerRepo.FirstOrDefault(x => x.ClaimID == claimId && x.CustomerID == coowner);

                if (ec != null) continue;

                CoOwnerClaim newCoOwner = new CoOwnerClaim();
                newCoOwner.CustomerID = coowner;
                newCoOwner.ClaimID = claimId;
                _coOwnerRepo.Insert(newCoOwner);
            }


            List<CoOwnerClaim> ecCoList = _coOwnerRepo.GetAll().Where(e => e.ClaimID == claimId).ToList();
            foreach (CoOwnerClaim ec in ecCoList)
            {
                if (!listCoOwners.Any(e => e == ec.CustomerID))
                    _coOwnerRepo.Delete(ec);
            }
        }

        private int SaveBordeaux(int orderauxId, string claimId, DateTime createdDate, int createdBy)
        {
            //Save default status, history
            Bordereaux hist = new Bordereaux();
            hist.ClaimID = claimId;
            hist.CreatedDate = createdDate;
            hist.CreatedBy = createdBy;
            return _borderauxRepo.InsertAndGetId(hist);
        }

        public BorderauxView GetBorderauxInfo(string claimID)
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

            bool all_bdr = true;
            //var all_bdr = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_ClaimsManagement_UpdateAllBorderauxStatus).Result;

            var query = from c in _claimTableRepo.GetAll()
                        join h in _borderauxRepo.GetAll() on c.BordereauxID equals h.Id
                        where (c.Id == claimID) && ((curr_role == "Admin") || all_bdr || (h.CreatedBy == curr_user.EmployeeId))
                        select new BorderauxView
                        {
                            ID = h.Id,
                            ClaimID = h.ClaimID,
                            DateOfSurvey = h.DateOfSurvey,
                            ReportID = h.ReportID,
                            ReportName = (h.Report == null ? string.Empty : h.Report.Name),
                            DateOfReport = h.DateOfReport,
                            ClaimStatusID = h.BordereauxStatusID,
                            //StatusName = (h.OtherStatus == null || h.OtherStatus == string.Empty
                            //                ? (h.ClaimStatus == null ? string.Empty : h.ClaimStatus.Name)
                            //                : h.OtherStatus),
                            OtherStatus = h.OtherStatus,  //hvtam-29022016

                            FollowUpID = h.FollowUpID,
                            //FollowUp = (h.OtherFollowUp == null || h.OtherFollowUp == string.Empty
                            //                ? (h.FollowUp == null ? string.Empty : h.FollowUp.Name)
                            //                : h.OtherFollowUp),
                            OtherFollowUp = h.OtherFollowUp, //hvtam-29022016
                            CreatedDate = h.CreatedDate,
                            CreatedBy = h.CreatedBy,
                            Reserve = h.Reserve,
                            ProgressPayment = h.ProgressPayment,
                            RecentCorrespondence = h.RecentCorrespondence,
                        };
            BorderauxView result = query.SingleOrDefault();
            return result;

        }


        /// <summary>
        /// TODO add permission
        /// </summary>
        /// <returns></returns>
        public List<string> GetOpenClaim()
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

            List<string> myClaims = new List<string>();
            var list_claim = _employeeClaimRepo.GetAll()
                .Where(ec => ec.EmployeeID == curr_user.EmployeeId)
                .Select(ec => ec.ClaimID)
                .ToList();
            return _claimTableRepo.GetAll()
                       // .Where(c => c.Closed == null || c.Closed == false) hvtam-17122014
                       .Where(c => (c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE) 
                       && (curr_role == "Admin" || list_claim.Contains(c.Id))) //open
                        .Select(c => c.Id)
                        .ToList();

            //if (HasPermission(PageCapability.Claim_UpdateBorderauxStatus_Admin)) //hvtam-291222014 Claim_UpdateStatus
            //    myClaims = ClaimServiceUOW.GetAllOpenClaimIDs();//hvtam-20022016 myClaims = ClaimServiceUOW.GetAllClaims();

            //else
            //    if (HasPermission(PageCapability.Claim_UpdateBorderauxStatus_Mine))//PageCapability.Claim_UpdateBorderauxStatus_Mine
            //    myClaims = ClaimServiceUOW.GetAllMyOpenWorkingClaimIDs(User.EmployeeID.Value); //hvtam-16032016 cho phep tat ca cac user deu co quyen update nhu email cua Nam myClaims = ClaimServiceUOW.GetAllMyAMOpenClaims(User.EmployeeID.Value);

            //ClaimDropDown.BindDataWithEmptyItem(myClaims);
        }

        public List<string> GetOpenClaimToBorderaux()
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

            List<string> myClaims = new List<string>();
            var list_claim = _employeeClaimRepo.GetAll()
                .Where(ec => ec.EmployeeID == curr_user.EmployeeId)
                .Select(ec => ec.ClaimID)
                .ToList();

            bool GetAllClaimPermission = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_ClaimsManagement_UpdateAllBorderauxStatus).Result;

            bool GetAllClaim = false;

            if ((curr_role == "Admin") || (GetAllClaimPermission))
            {
                GetAllClaim = true;
            }

            return _claimTableRepo.GetAll()
                       // .Where(c => c.Closed == null || c.Closed == false) hvtam-17122014
                       .Where(c => (c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE)
                       && (GetAllClaim == true || c.AccountManagerID == curr_user.EmployeeId)) //open
                       .OrderBy(c => c.Id)
                       .Select(c => c.Id)
                       .ToList();

            //if (HasPermission(PageCapability.Claim_UpdateBorderauxStatus_Admin)) //hvtam-291222014 Claim_UpdateStatus
            //    myClaims = ClaimServiceUOW.GetAllOpenClaimIDs();//hvtam-20022016 myClaims = ClaimServiceUOW.GetAllClaims();

            //else
            //    if (HasPermission(PageCapability.Claim_UpdateBorderauxStatus_Mine))//PageCapability.Claim_UpdateBorderauxStatus_Mine
            //    myClaims = ClaimServiceUOW.GetAllMyOpenWorkingClaimIDs(User.EmployeeID.Value); //hvtam-16032016 cho phep tat ca cac user deu co quyen update nhu email cua Nam myClaims = ClaimServiceUOW.GetAllMyAMOpenClaims(User.EmployeeID.Value);

            //ClaimDropDown.BindDataWithEmptyItem(myClaims);
        }

        public List<string> GetOpenClaimToCreateTask()
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

            List<string> myAllClaims = new List<string>();
            myAllClaims = _employeeClaimRepo.GetAll()
                .Where(ec => ec.EmployeeID == curr_user.EmployeeId).Select(mc => mc.ClaimID).ToList();
            List<string> myClaims = new List<string>();
            return _claimTableRepo.GetAll()
                       // .Where(c => c.Closed == null || c.Closed == false) hvtam-17122014
                       .Where(c => (c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE)
                                && (curr_role == "Admin" || c.AccountManagerID == curr_user.EmployeeId
                                || myAllClaims.Contains(c.Id))
                                && (c.Issued == null || c.Issued == false))
                        .Select(c => c.Id)
                        .ToList();
            //TODO
            //if (HasPermission(PageCapability.Task_Edit))
            //    claimList = TaskServiceUOW.GetOpenList();
            //else
            //    claimList = TaskServiceUOW.GetOpenListByAccMgr(User.EmployeeID.Value);
            //context.Claims
            //                //.Where(c => (c.Closed == null || c.Closed == false) hvtam-17122014
            //                .Where(c => (c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE)
            //                            && c.AccountManagerID == accMgrID
            //                            && (c.Issued == null || c.Issued == false))
            //                .Select(c => c.ID)
            //                .ToList();
        }
        public List<string> GetOpenClaimToCreateTimesheet()
        {

            List<string> myClaims = new List<string>();
            return _claimTableRepo.GetAll()
                       // .Where(c => c.Closed == null || c.Closed == false) hvtam-17122014
                       .Where(c => c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE)
                       .OrderBy(c => c.Id) //open
                       .Select(c => c.Id)
                       .ToList();
            //TODO ADD PERMISSION
            //if (HasPermission(PageCapability.TimeSheet_Create_TimeSheet))
            //{
            //    claimList = ClaimService.GetAllOpenClaimIDList();
            //}
            //else
            //{
            //    claimList = ClaimService.GetAllMyOpenWorkingClaimIDs(User.EmployeeID.Value);
            //}
            //ddlClaim_Popup.BindDataWithEmptyItem(claimList);
        }
        public List<string> GetOpenClaimToCreateInvoice()
        {

            List<string> myClaims = new List<string>();
            return _claimTableRepo.GetAll()
                       .Where(c => c.RefStatusID == null || c.RefStatusID != REF_STATUS_DEFINE.CLOSE) //open
                       .OrderBy(c => c.Id)
                       .Select(c => c.Id)
                       .ToList();
        }

        public List<ReportView> GetListReport()
        {
            var query = from r in _reportRepo.GetAll()
                        select new ReportView
                        {
                            ReportID = r.Id,
                            Name = r.Name,
                            IsUsed = r.Bordereaux.Any()
                        };
            return query.OrderBy(r => r.Name).ToList();

        }
        public List<BordereauxStatusView> GetActiveBordereauxStatus()
        {
            var query = (from bstatus in _borderauxStatusRepo.GetAll()
                         where bstatus.isActive == true
                         select new BordereauxStatusView
                         {
                             StatusID = bstatus.Id,
                             Name = bstatus.Name,
                             IsActive = bstatus.isActive,   //hvtam-19032016
                             IsUsed = bstatus.Bordereaux.Any()
                         });

            return query.OrderBy(s => s.StatusID).ToList();

        }

        /// <summary>
        /// Gets the list of follow-ups
        /// </summary>
        /// <returns>The list of follow-ups</returns>
        public List<FollowUpView> GetFollowUpList()
        {
            var query = from f in _followUpRepo.GetAll()
                        select new FollowUpView
                        {
                            FollowUpID = f.Id,
                            Name = f.Name,
                            Code = f.Code,
                            IsUsed = f.Bordereaux.Any()
                        };
            return query.OrderBy(f => f.Name).ToList();
        }
        public List<BorderauxView> GetBordereauxHistories(string claimID)
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

            bool all_bdr2 = true;
            //var all_bdr2 = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_ClaimsManagement_UpdateAllBorderauxStatus).Result;

            var query = from h in _borderauxRepo.GetAll()
                        where (h.ClaimID == claimID) && ((curr_role == "Admin") || all_bdr2 || (h.CreatedBy == curr_user.EmployeeId))
                        //&& h.isActive == true   //hvtam-20022016

                        select new BorderauxView
                        {
                            ID = h.Id,
                            ClaimID = h.ClaimID,
                            DateOfSurvey = h.DateOfSurvey,
                            ReportID = h.ReportID,
                            ReportName = (h.Report == null ? null : h.Report.Name),
                            DateOfReport = h.DateOfReport,
                            FollowUpID = h.FollowUpID,
                            FollowUp = (h.OtherFollowUp == null || h.OtherFollowUp == string.Empty
                                            ? (h.FollowUp == null ? string.Empty : h.FollowUp.Name)
                                            : h.OtherFollowUp),
                            OtherStatus = h.OtherStatus,
                            ClaimStatusID = h.BordereauxStatusID,
                            StatusName = (h.OtherStatus == null || h.OtherStatus == string.Empty
                                            ? (h.BordereauxStatusID == null ? string.Empty : h.BordereauxStatus.Name)
                                            : h.OtherStatus),
                            CreatedDate = h.CreatedDate,
                            CreatedBy = h.CreatedBy,
                            Reserve = h.Reserve,
                            ProgressPayment = h.ProgressPayment
                        };
            List<BorderauxView> historyList = query.ToList();

            historyList = historyList.OrderByDescending(h => h.CreatedDate).ToList();
            return historyList;
        }

        public string CreateBorderaux(UpdateBorderauxStatusInput input)
        {
            User currUser = GetCurrentUser();
            BorderauxView lastHistoryBordereaux = GetBorderauxInfo(input.ClaimId);
            BorderauxView bordereauxView = new BorderauxView();
            bordereauxView.ClaimID = input.ClaimId;
            bordereauxView.ReportID = input.ReportID;
            bordereauxView.ClaimStatusID = input.ClaimStatusID;

            if (input.ClaimStatusID == "N")//other
                bordereauxView.OtherStatus = input.OtherStatus.Trim();
            else
            {
                bordereauxView.IsClosed = input.ClaimStatusID == "L"; //hvtam???? closed
                bordereauxView.OtherStatus = string.Empty;
            }

            bordereauxView.FollowUpID = input.FollowUpID;

            if (input.FollowUpID == 13)//other
                if (!string.IsNullOrEmpty(input.OtherFollowUp))
                {
                    bordereauxView.OtherFollowUp = input.OtherFollowUp.Trim();
                }                
            else
                bordereauxView.OtherFollowUp = string.Empty;

            if (input.DateOfReport == DateTime.MinValue)
            {
                bordereauxView.DateOfReport = null;
            }
            else
                bordereauxView.DateOfReport = input.DateOfReport;
            
            if (input.RecentCorrespondence == DateTime.MinValue)
            {
                bordereauxView.RecentCorrespondence = null;
            }
            else
                bordereauxView.RecentCorrespondence = input.RecentCorrespondence;
            
            if (bordereauxView.DateOfReport.HasValue && bordereauxView.RecentCorrespondence.HasValue)
            {
                if (bordereauxView.DateOfReport > bordereauxView.RecentCorrespondence)
                {
                    bordereauxView.RecentCorrespondence = bordereauxView.DateOfReport;
                }
            }
            
            if (input.DateOfSurvey == DateTime.MinValue)
                bordereauxView.DateOfSurvey = null;
            else
                bordereauxView.DateOfSurvey = input.DateOfSurvey;
            bordereauxView.CreatedBy = (int)currUser.EmployeeId;
            bordereauxView.CreatedDate = DateTime.Now;
            //borderauxView.isActive = true; //hvtam-20022016 update active

            if (!input.Reserve.HasValue)
                bordereauxView.Reserve = null;
            else
                bordereauxView.Reserve = input.Reserve;

            if (!input.ProgressPayment.HasValue)
                bordereauxView.ProgressPayment = null;
            else
                bordereauxView.ProgressPayment = input.ProgressPayment;//hvtam-20022016 Them field ProgressPayment;

            if (lastHistoryBordereaux != null)
            {
                if (lastHistoryBordereaux.GetHashCode() == bordereauxView.GetHashCode())
                {
                    if (lastHistoryBordereaux.RecentCorrespondence != bordereauxView.RecentCorrespondence)
                    {
                        Bordereaux bd_update = _borderauxRepo.Get(lastHistoryBordereaux.ID);
                        MapBordereaux(bd_update, bordereauxView);
                        _borderauxRepo.Update(bd_update);                        
                    } else
                    {
                        return "Nothing changed";
                    }
                } else
                {
                    Bordereaux borderaux = new Bordereaux();
                    MapBordereaux(borderaux, bordereauxView);
                    int newID = _borderauxRepo.InsertAndGetId(borderaux);

                    bordereauxView.ID = borderaux.Id;
                    //update into claim table
                    Claim claim = _claimTableRepo.FirstOrDefault(x => x.Id == borderaux.ClaimID);
                    claim.BordereauxID = newID;
                }
            } else
            {
                Bordereaux borderaux = new Bordereaux();
                MapBordereaux(borderaux, bordereauxView);
                int newID = _borderauxRepo.InsertAndGetId(borderaux);

                bordereauxView.ID = borderaux.Id;
                //update into claim table
                Claim claim = _claimTableRepo.FirstOrDefault(x => x.Id == borderaux.ClaimID);
                claim.BordereauxID = newID;
                
            }
            return "ok";
        }
        private void MapBordereaux(Bordereaux borderaux, BorderauxView bordereauxView)
        {
            borderaux.ClaimID = bordereauxView.ClaimID;
            borderaux.BordereauxStatusID = bordereauxView.ClaimStatusID;
            borderaux.DateOfSurvey = bordereauxView.DateOfSurvey;
            borderaux.RecentCorrespondence = bordereauxView.RecentCorrespondence;
            borderaux.ReportID = bordereauxView.ReportID;
            borderaux.DateOfReport = bordereauxView.DateOfReport;
            borderaux.OtherStatus = bordereauxView.OtherStatus;
            borderaux.FollowUpID = bordereauxView.FollowUpID;
            borderaux.OtherFollowUp = bordereauxView.OtherFollowUp;
            borderaux.CreatedDate = bordereauxView.CreatedDate;
            borderaux.CreatedBy = bordereauxView.CreatedBy;
            borderaux.Reserve = bordereauxView.Reserve;
            borderaux.ProgressPayment = bordereauxView.ProgressPayment;
        }
        private void MapBordereaux2(BordereauxDBObj borderaux, BorderauxView bordereauxView)
        {
            borderaux.ClaimID = bordereauxView.ClaimID;
            borderaux.BordereauxStatusID = bordereauxView.ClaimStatusID;
            borderaux.DateOfSurvey = bordereauxView.DateOfSurvey;
            borderaux.RecentCorrespondence = bordereauxView.RecentCorrespondence;
            borderaux.ReportID = bordereauxView.ReportID;
            borderaux.DateOfReport = bordereauxView.DateOfReport;
            borderaux.OtherStatus = bordereauxView.OtherStatus;
            borderaux.FollowUpID = bordereauxView.FollowUpID;
            borderaux.OtherFollowUp = bordereauxView.OtherFollowUp;
            borderaux.CreatedDate = bordereauxView.CreatedDate;
            borderaux.CreatedBy = bordereauxView.CreatedBy;
            borderaux.Reserve = bordereauxView.Reserve;
            borderaux.ProgressPayment = bordereauxView.ProgressPayment;
        }
        private int InsertBordereaux(BordereauxDBObj input)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("" +
                "INSERT INTO Bordereaux(ClaimID, DateOfSurvey, ReportID, DateOfReport, BordereauxStatusID, OtherStatus" +
                ", FollowUpID, OtherFollowUp, CreatedDate, CreatedBy, Reserve, ProgressPayment" + "RecentCorrespondence" +
                " )" +
                " VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12)"
                , input.ClaimID, input.DateOfSurvey, input.ReportID, input.DateOfReport, input.BordereauxStatusID, input.OtherStatus
                , input.FollowUpID, input.OtherFollowUp, input.CreatedDate, input.CreatedBy, input.Reserve, input.ProgressPayment, input.RecentCorrespondence);
        }

        public List<EmployeeView> GetEmployeeByClaim(string claimID)
        {
            var users = UserManager.Users.ToList();
            var list = _employeeClaimRepo.GetAll()
                .Where(x => x.ClaimID == claimID)
                .Select(x => new EmployeeView { EmployeeID = x.EmployeeID })
                .ToList();
            list.ForEach(x =>
            {
                //x.Name = users.FirstOrDefault(y => y.EmployeeId == x.EmployeeID).Name;
                x.Name = (users.FirstOrDefault(y => y.Id == x.EmployeeID) != null) ? users.FirstOrDefault(y => y.Id == x.EmployeeID).Name : "";
                x.Email = (users.FirstOrDefault(y => y.Id == x.EmployeeID) != null) ? users.FirstOrDefault(y => y.Id == x.EmployeeID).EmailAddress : "";
            });

            return list;
        }

        public int CreateClaimDirectory(string foldername, int claimSTT)
        {
            //var RootFolder = @"C:\vietadjuster\CMCLAIMDOCS\";
            var RootFolder = ConfigurationManager.AppSettings["RootFolder"];
            string fullfoldername = RootFolder + foldername;

            try
            {
                // Checking the existance of directory
                if (!Directory.Exists(fullfoldername))
                {
                    //If No any such directory then creates the new one
                    Directory.CreateDirectory(fullfoldername);

                    System.Security.AccessControl.FileSecurity fSec = File.GetAccessControl(fullfoldername); //??? de lam gi                    

                    List<Subfolders> listtmp = new List<Subfolders>();
                    listtmp = GetSubfoldersList();

                    foreach (Subfolders subfolder in listtmp)
                    {
                        string fullsubfolder = RootFolder + foldername + @"\" + subfolder.Name; //hvtam-25122014- use physical path to 

                        Directory.CreateDirectory(fullsubfolder);

                        if (subfolder.Name == SubFolderNameVolumn.F_TIMESHEET)
                        {
                            DirectoryInfo di = new DirectoryInfo(fullsubfolder);                            
                            DirectorySecurity ds = di.GetAccessControl();
                            ds.SetAccessRuleProtection(true, true);
                            di.SetAccessControl(ds);
                            ds = di.GetAccessControl();

                            // Remove the FileSystemAccessRule from the security settings.
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                            ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                            di.SetAccessControl(ds);

                            di.Attributes = FileAttributes.Hidden;
                        }
                        //FileSystemRights Rights = FileSystemRights.Modify;
                        //AccessControlType ACType = AccessControlType.Allow;
                    }
                    F tblFolder = new F();
                    tblFolder.FName = foldername;
                    tblFolder.FPath = fullfoldername;
                    tblFolder.FSecurity = "OUTSTANDING";
                    tblFolder.CreatedDate = DateTime.Now;
                    tblFolder.isUpload = null;
                    _tblFolderRepository.Insert(tblFolder);
                    return 1;
                }
                else
                {                    
                    DirectoryInfo di = new DirectoryInfo(fullfoldername);
                    DirectorySecurity ds = di.GetAccessControl();

                    List<Subfolders> listtmp = new List<Subfolders>();
                    listtmp = GetSubfoldersList();

                    if (claimSTT == 2)
                    {                        
                        ds.SetAccessRuleProtection(true, true);
                        di.SetAccessControl(ds);
                        ds = di.GetAccessControl();
                        // Remove the FileSystemAccessRule from the security settings.
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        // Add the FileSystemAccessRule to the security settings.
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.AddAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        di.SetAccessControl(ds);

                        foreach (Subfolders subfolder in listtmp)
                        {
                            string fullsubfolder = RootFolder + foldername + @"\" + subfolder.Name;                            
                            if (Directory.Exists(fullsubfolder))
                            {                                
                                if (subfolder.Name == SubFolderNameVolumn.F_TIMESHEET)
                                {
                                    DirectoryInfo sub_di = new DirectoryInfo(fullsubfolder);                                    
                                    DirectorySecurity sub_ds = sub_di.GetAccessControl();
                                    sub_ds.SetAccessRuleProtection(true, true);
                                    sub_di.SetAccessControl(sub_ds);
                                    sub_ds = sub_di.GetAccessControl();

                                    // Remove the FileSystemAccessRule to the security settings.
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_di.SetAccessControl(sub_ds);

                                    sub_di.Attributes = FileAttributes.Hidden;
                                }
                            }                            
                        }

                        F tblFolder = _tblFolderRepository.GetAll().Where(x => x.FName == foldername).FirstOrDefault();
                        //tblFolder.FName = foldername;
                        //tblFolder.FPath = fullfoldername;
                        tblFolder.FSecurity = "CLOSED";
                        //tblFolder.CreatedDate = DateTime.Now;
                        //tblFolder.isUpload = null;
                        _tblFolderRepository.Update(tblFolder);
                    }
                    else
                    {
                        // Remove the FileSystemAccessRule to the security settings.
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.Read, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                        ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.ReadAndExecute, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        di.SetAccessControl(ds);

                        ds = di.GetAccessControl();
                        ds.SetAccessRuleProtection(false, false);
                        di.SetAccessControl(ds);

                        foreach (Subfolders subfolder in listtmp)
                        {
                            string fullsubfolder = RootFolder + foldername + @"\" + subfolder.Name;                            
                            if (Directory.Exists(fullsubfolder))
                            {                                
                                if (subfolder.Name == SubFolderNameVolumn.F_TIMESHEET)
                                {
                                    DirectoryInfo sub_di = new DirectoryInfo(fullsubfolder);
                                    DirectorySecurity sub_ds = sub_di.GetAccessControl();
                                    sub_ds.SetAccessRuleProtection(true, true);
                                    sub_di.SetAccessControl(sub_ds);
                                    sub_ds = sub_di.GetAccessControl();

                                    // Remove the FileSystemAccessRule from the security settings.
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"VIETRISKS\Permission", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"BUILTIN\Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\Authenticated Users", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_ds.RemoveAccessRule(new FileSystemAccessRule(@"NT AUTHORITY\NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    sub_di.SetAccessControl(sub_ds);

                                    sub_di.Attributes = FileAttributes.Hidden;
                                }
                            }
                        }

                        F tblFolder = _tblFolderRepository.GetAll().Where(x => x.FName == foldername).FirstOrDefault();
                        //tblFolder.FName = foldername;
                        //tblFolder.FPath = fullfoldername;
                        tblFolder.FSecurity = "OUTSTANDING";
                        //tblFolder.CreatedDate = DateTime.Now;
                        //tblFolder.isUpload = null;
                        _tblFolderRepository.Update(tblFolder);
                    }                    

                    return 0;
                }
            }
            catch (IOException _err)
            {
                Logger.Error("Create folder error:", _err);
                return 0;
            }
        }

        // Removes an ACL entry on the specified directory for the specified account.
        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            dSecurity.SetAccessRuleProtection(true, false);
            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);
        }

        // Adds an ACL entry on the specified directory for the specified account.
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            dSecurity.SetAccessRuleProtection(true, false);
            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }

        public List<Subfolders> GetSubfoldersList()
        {
            List<Subfolders> dList = new List<Subfolders>();
            Subfolders objSub = null;
            objSub = new Subfolders();
            objSub.Id = 1;
            objSub.Name = SubFolderNameVolumn.F_CORRESPONDENCE;
            dList.Add(objSub);
            objSub = new Subfolders();
            objSub.Id = 2;
            objSub.Name = SubFolderNameVolumn.F_POLICY;
            dList.Add(objSub);
            objSub = new Subfolders();
            objSub.Id = 3;
            objSub.Name = SubFolderNameVolumn.F_REFERENCE;
            dList.Add(objSub);
            //report
            objSub = new Subfolders();
            objSub.Id = 4;
            objSub.Name = SubFolderNameVolumn.F_REPORT;
            dList.Add(objSub);
            objSub = new Subfolders();
            objSub.Id = 5;
            objSub.Name = SubFolderNameVolumn.F_SITE_SURVEY_MINUTES;
            dList.Add(objSub);
            objSub = new Subfolders();
            objSub.Id = 6;
            objSub.Name = SubFolderNameVolumn.F_QUOTATION;
            dList.Add(objSub);

            objSub = new Subfolders();
            objSub.Id = 7;
            objSub.Name = SubFolderNameVolumn.F_SUPPORTING_DOCUMENTS;
            dList.Add(objSub);

            objSub = new Subfolders();
            objSub.Id = 8;
            objSub.Name = SubFolderNameVolumn.F_DRAWINGS;   //hvtam-16092015
            dList.Add(objSub);

            objSub = new Subfolders();
            objSub.Id = 9;
            objSub.Name = SubFolderNameVolumn.F_CAUSE_OF_LOSS; //hvtam-16092015
            dList.Add(objSub);

            objSub = new Subfolders();
            objSub.Id = 9;
            objSub.Name = SubFolderNameVolumn.F_TIMESHEET; //hvtam-20032016
            dList.Add(objSub);
            return dList;
        }

        public List<OutStandingView> GetTotalOutStanding()
        {
            OutStandingView outStanding = new OutStandingView();
            var context = _sqlExecuter.GetTLSDBContext();

            foreach (var line in context.Claims.Where(status => status.RefStatusID == REF_STATUS_DEFINE.OPEN)
                    .GroupBy(info => info.OfficeID)
                    .Select(group => new
                    {
                        OfficeID = group.Key,
                        Count = group.Count()
                    })
                    .OrderBy(x => x.OfficeID))
            {
                if (line.OfficeID.Equals(1))
                {
                    outStanding.SG = line.Count;
                }
                else
                {
                    outStanding.HN = line.Count;
                }
            }
            outStanding.Total = outStanding.SG + outStanding.HN;

            List<OutStandingView> list = new List<OutStandingView>();
            list.Add(outStanding);
            return list;
        }

        public List<ClaimView> GetClaimsByEmployee(int rtype)
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

            if (rtype == 0)
            {
                var query = from claim in context.Claims
                            join ec in context.EmployeeClaims on claim.Id equals ec.ClaimID
                            join em in context.Employees on ec.EmployeeID equals em.Id
                            join am in context.Employees on claim.AccountManagerID equals am.Id
                            join cus in context.Customers on claim.InsurerID equals cus.Id
                            join refstat in context.RefStatuses on claim.RefStatusID equals refstat.Id
                            where ((curr_role == "Admin" || claim.AccountManagerID == curr_user.EmployeeId) && (claim.RefStatusID != 2))
                            join hist in context.Bordereauxs on claim.Id equals hist.ClaimID into joinGroup
                            from i in joinGroup.DefaultIfEmpty()
                            orderby claim.CreatedDate descending //hvtam-12102014
                            select new ClaimView
                            {
                                //AccountManagerID = claim.AccountManagerID,
                                AccountManager = am.Name,
                                //Broker = cus.Name,
                                //ClaimID = claim.ID, //hvtam-28022016
                                ID = claim.Id,
                                //CurrentStatus =
                                //    (i == null || i.BordereauxStatus == null
                                //        ? null
                                //        : i.OtherStatus == null || i.OtherStatus == ""
                                //            ? i.BordereauxStatus.Name
                                //            : i.OtherStatus),
                                //FollowUp =
                                //    (i == null || i.FollowUp == null
                                //        ? null
                                //        : i.OtherFollowUp == null || i.OtherFollowUp == ""
                                //            ? i.FollowUp.Name
                                //            : i.OtherFollowUp),
                                //Report = (i == null || i.Report == null ? null : i.Report.Name),
                                Insurer = cus.Name,
                                //insurerbrandname = cus.BrandName,
                                //Issued = claim.Issued,
                                //DateOfAssignment = claim.DateOfAssignment,  //hvtam-02022016 DateOfAssignment
                                //CreatedDate = claim.CreatedDate,
                                //Closed = claim.Closed,
                                //RefStatusID = claim.RefStatusID, //hvtam-22102014
                                //RefStatus = refstat.Name, //hvtam-17122014
                                //StatusID = i.BordereauxStatusID, //hvtam-21102014
                                Reserve = string.IsNullOrEmpty(claim.Reserve) ? "0" : claim.Reserve 
                            };

                return query.Distinct().ToList();
            } else
            {
                if (rtype == 1)
                {
                    var query = from claim in context.Claims
                                join ec in context.EmployeeClaims on claim.Id equals ec.ClaimID
                                join em in context.Employees on ec.EmployeeID equals em.Id
                                join am in context.Employees on claim.AccountManagerID equals am.Id
                                join cus in context.Customers on claim.InsurerID equals cus.Id
                                join refstat in context.RefStatuses on claim.RefStatusID equals refstat.Id                                
                                where ((curr_role == "Admin" || (em.Id == curr_user.EmployeeId && claim.AccountManagerID != curr_user.EmployeeId)) && (claim.RefStatusID != 2))
                                join hist in context.Bordereauxs on claim.Id equals hist.ClaimID into joinGroup
                                from i in joinGroup.DefaultIfEmpty()
                                orderby claim.CreatedDate descending //hvtam-12102014
                                select new ClaimView
                                {
                                    //AccountManagerID = claim.AccountManagerID,
                                    AccountManager = am.Name,
                                    //Broker = cus.Name,
                                    //ClaimID = claim.ID, //hvtam-28022016
                                    ID = claim.Id,
                                    //CurrentStatus =
                                    //    (i == null || i.BordereauxStatus == null
                                    //        ? null
                                    //        : i.OtherStatus == null || i.OtherStatus == ""
                                    //            ? i.BordereauxStatus.Name
                                    //            : i.OtherStatus),
                                    //FollowUp =
                                    //    (i == null || i.FollowUp == null
                                    //        ? null
                                    //        : i.OtherFollowUp == null || i.OtherFollowUp == ""
                                    //            ? i.FollowUp.Name
                                    //            : i.OtherFollowUp),
                                    //Report = (i == null || i.Report == null ? null : i.Report.Name),
                                    Insurer = cus.Name,
                                    //insurerbrandname = cus.BrandName,
                                    //Issued = claim.Issued,
                                    //DateOfAssignment = claim.DateOfAssignment,  //hvtam-02022016 DateOfAssignment
                                    //CreatedDate = claim.CreatedDate,
                                    //Closed = claim.Closed,
                                    //RefStatusID = claim.RefStatusID, //hvtam-22102014
                                    //RefStatus = refstat.Name, //hvtam-17122014
                                    //StatusID = i.BordereauxStatusID, //hvtam-21102014
                                    Reserve = string.IsNullOrEmpty(claim.Reserve) ? "0" : claim.Reserve
                                };

                    return query.Distinct().ToList();
                } else
                {
                    var query = from claim in context.Claims
                                join ec in context.EmployeeClaims on claim.Id equals ec.ClaimID
                                join em in context.Employees on ec.EmployeeID equals em.Id
                                join am in context.Employees on claim.AccountManagerID equals am.Id
                                join cus in context.Customers on claim.InsurerID equals cus.Id
                                join refstat in context.RefStatuses on claim.RefStatusID equals refstat.Id                                
                                where ((curr_role == "Admin" || claim.AccountManagerID == curr_user.EmployeeId || em.Id == curr_user.EmployeeId) && (claim.RefStatusID != 2))
                                join hist in context.Bordereauxs on claim.Id equals hist.ClaimID into joinGroup
                                from i in joinGroup.DefaultIfEmpty()
                                orderby claim.CreatedDate descending //hvtam-12102014
                                select new ClaimView
                                {
                                    //AccountManagerID = claim.AccountManagerID,
                                    AccountManager = am.Name,
                                    //Broker = cus.Name,
                                    //ClaimID = claim.ID, //hvtam-28022016
                                    ID = claim.Id,
                                    //CurrentStatus =
                                    //    (i == null || i.BordereauxStatus == null
                                    //        ? null
                                    //        : i.OtherStatus == null || i.OtherStatus == ""
                                    //            ? i.BordereauxStatus.Name
                                    //            : i.OtherStatus),
                                    //FollowUp =
                                    //    (i == null || i.FollowUp == null
                                    //        ? null
                                    //       : i.OtherFollowUp == null || i.OtherFollowUp == ""
                                    //            ? i.FollowUp.Name
                                    //            : i.OtherFollowUp),
                                    //Report = (i == null || i.Report == null ? null : i.Report.Name),
                                    Insurer = cus.Name,
                                    //insurerbrandname = cus.BrandName,
                                    //Issued = claim.Issued,
                                    //DateOfAssignment = claim.DateOfAssignment,  //hvtam-02022016 DateOfAssignment
                                    //CreatedDate = claim.CreatedDate,
                                    //Closed = claim.Closed,
                                    //RefStatusID = claim.RefStatusID, //hvtam-22102014
                                    //RefStatus = refstat.Name, //hvtam-17122014
                                    //StatusID = i.BordereauxStatusID, //hvtam-21102014
                                    Reserve = string.IsNullOrEmpty(claim.Reserve) ? "0" : claim.Reserve
                                };

                    return query.Distinct().ToList();
                }
            }
        }

        public bool checkPermission_AllClaim()
        {
            bool all_permission = false;
            var curr_user = GetCurrentUser();
            var permission_allclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_ViewAllClaim);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            all_permission = permissionlist.Result.Contains(permission_allclaim);            
            return all_permission;
        }
        public bool checkPermission_SearchClaim()
        {
            bool search_permission = false;
            var curr_user = GetCurrentUser();
            var permission_searchclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_SearchClaim);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);            
            search_permission = permissionlist.Result.Contains(permission_searchclaim);
            return search_permission;
        }

        public bool checkPermission_CreateClaim()
        {
            bool create_permission = false;
            var curr_user = GetCurrentUser();
            var permission_createclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_CreateClaim);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            create_permission = permissionlist.Result.Contains(permission_createclaim);            
            return create_permission;
        }

        public bool checkPermission_EditClaim()
        {
            bool edit_permission = false;
            var curr_user = GetCurrentUser();
            var permission_editclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_EditMyClaims);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            edit_permission = permissionlist.Result.Contains(permission_editclaim);            
            return edit_permission;
        }

        public bool checkPermission_AssignClaim()
        {
            bool assign_permission = false;
            var curr_user = GetCurrentUser();
            var permission_assignclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_AssignEmployeeToClaim);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            assign_permission = permissionlist.Result.Contains(permission_assignclaim);            
            return assign_permission;
        }

        public bool checkPermission_UpdateStatusClaim()
        {
            bool update_status_permission = false;
            var curr_user = GetCurrentUser();
            var permission_updateclaim = PermissionManager.GetPermission(AppPermissions.Pages_ClaimsManagement_UpdateMyClaimStatus);
            var permissionlist = UserManager.GetGrantedPermissionsAsync(curr_user);
            update_status_permission = permissionlist.Result.Contains(permission_updateclaim);            
            return update_status_permission;
        }

        public FileDto ExporClaimFunc(ExportClaimInput input)
        {            
            SqlConnection connection = (SqlConnection)_sqlExecuter.GetDatabase().Connection;

            SqlCommand comm = new SqlCommand("ExportClaim", connection);
            comm.CommandTimeout = 18000;
            connection.Open();

            comm.Connection = connection;
            comm.CommandType = CommandType.StoredProcedure;
            if (input.OfficeID.HasValue)
            {
                comm.Parameters.Add("@OFFICEID", SqlDbType.Int).Value = input.OfficeID;
            } else
            {
                comm.Parameters.Add("@OFFICEID", SqlDbType.Int).Value = DBNull.Value;
            }
            
            if (input.StartDate.HasValue)
            {
                comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = input.StartDate;
            } else
            {
                comm.Parameters.Add("@FROMDATE", SqlDbType.DateTime).Value = DBNull.Value;
            }
            
            if (input.EndDate.HasValue)
            {
                comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = input.EndDate.Value.AddDays(-1);
            } else
            {
                comm.Parameters.Add("@TODATE", SqlDbType.DateTime).Value = DBNull.Value;
            }
                        
            if (input.StatusID.HasValue)
            {
                comm.Parameters.Add("@STATUS", SqlDbType.Int).Value = input.StatusID;
            } else
            {
                comm.Parameters.Add("@STATUS", SqlDbType.Int).Value = DBNull.Value;
            }
            
            string path = AppFolders.TempFileDownloadFolder;
            //System.Configuration.ConfigurationManager.AppSettings["ExportFolder"];
            string filename = "ExportClaim_" + input.StartDate?.ToString("yyyyMMdd") + "_" + input.EndDate?.ToString("yyyyMMdd") + ".xlsx";

            var file = new FileDto(filename, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, filename);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(comm);

                DataSet rs = new DataSet();
                da.Fill(rs);
                DataTable dt = rs.Tables[0].Copy();                                
                string tempFile = System.Configuration.ConfigurationManager.AppSettings["TemplateFoler"] + "TML_Claim_Export.xlsx";
                ExcelService.FillDatatableToExcel(dt, Path.Combine(path, file.FileToken), tempFile, "ClaimList", 4, 2, 6, 10, 20, 3, 6,
                    input.StartDate?.ToString("dd/MM/yyyy"), 3, 8, input.EndDate?.ToString("dd/MM/yyyy"), 0, 2, 6, "");

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
    }
}
