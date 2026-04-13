using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.OldTable
{
    public class ProfessionalFeeAppService : tlsAppServiceBase, IProfessionalFeeAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public ProfessionalFeeAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<ProfessionalFeeView> GetList(string ClaimID, int TimeSheetID)
        {
            var context = _sqlExecuter.GetTLSDBContext();
            
                var query = (from pf in context.ProfessionalFees
                             from t in context.TimeSheets
                             from j in context.TaskNames
                             where pf.TimeSheetID == t.Id && pf.JobCodeID == j.Id &&
                               t.ClaimID == ClaimID && t.Id == TimeSheetID
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
                                 CreateByName = (from e in context.Employees where e.Id == pf.CreateBy select e.Name).FirstOrDefault(),
                                 //hvtam-27082015
                                 //FeePerHour = (from e in context.Employees where e.ID == pf.CreateBy select e.Fee).FirstOrDefault().Value,
                                 FeePerHour = pf.FeePerHour.Value,
                                 //endhvtam-27082015

                                 ProFeeValue = pf.ProFeeValue.Value,

                                 WorkTime = pf.WorkTime,
                                 ApproveTime = pf.ApproveTime,

                             }).OrderByDescending(t => t.CreateDate);
                return query.ToList();
            
        }
        public List<ProfessionalFeeView> GetListProfeeofTimesheet(int TimeSheetID)
        {
            var context = _sqlExecuter.GetTLSDBContext();

            var curr_user = GetCurrentUser();
            var curr_role = "";            

            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == curr_user.EmployeeId
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

            var query = (from pf in context.ProfessionalFees
                             join t in context.TimeSheets on pf.TimeSheetID equals t.Id
                             join j in context.TaskNames on pf.JobCodeID equals j.Id
                             join c in context.Claims on t.ClaimID equals c.Id
                             where (t.Id == TimeSheetID) && 
                               (getAllFee || (pf.CreateBy == curr_user.EmployeeId) || (CheckGetAllFeeAM && (c.AccountManagerID == curr_user.EmployeeId)))
                             select new ProfessionalFeeView
                             {
                                 ProfessionalFeeID = pf.Id,
                                 TimeSheetID = pf.TimeSheetID.Value,
                                 InputDate = pf.InputDate.Value,
                                 JobCodeID = pf.JobCodeID,
                                 Notes = pf.Notes,
                                 //WorkingHour = pf.WorkingHour,

                                 //ApprovedHour = (pf.ApprovedHour==null ? pf.WorkingHour : pf.ApprovedHour),
                                 CreateDate = pf.CreateDate.Value,
                                 CreateBy = pf.CreateBy.Value,
                                 UpdateBy = pf.UpdateBy.Value,
                                 UpdateDate = pf.UpdateDate.Value,

                                 JobName = j.Name,
                                 StandardTime = j.StandardTime,

                                 CreateByName = (from e in context.Employees where e.Id == pf.CreateBy select e.Name).FirstOrDefault(),
                                 //hvtam-27082015
                                 //FeePerHour = (from e in context.Employees where e.ID == pf.CreateBy select e.Fee).FirstOrDefault().Value,
                                 FeePerHour = pf.FeePerHour.Value,
                                 //endhvtam-27082015

                                 ProFeeValue = pf.ProFeeValue ?? 0,


                                 WorkTime = pf.WorkTime,
                                 //ApproveTime = pf.WorkTime,                                 
                                 ApproveTime = (pf.ApproveTime == null ? pf.WorkTime : pf.ApproveTime),
                                 //ApproveTime = pf.ApproveTime,

                                 //hvtam-09052015: Charged fee
                                 ChargedBy = (pf.ChargedBy == null ? pf.CreateBy : pf.ChargedBy),
                                 ChargedByName = (from e in context.Employees where e.Id == (pf.ChargedBy == null ? pf.CreateBy : pf.ChargedBy) select e.Name).FirstOrDefault(),
                                 ChargedFeePerHour = (pf.ChargedFeePerHour == null ? pf.FeePerHour : pf.ChargedFeePerHour),
                                 ChargedProFeeValue = (pf.ChargedProFeeValue == null ? pf.ProFeeValue : pf.ChargedProFeeValue),


                             }).OrderByDescending(t => t.InputDate); //hvtam-08082015: t => t.CreateDate
                return query.ToList();
            
        }

        public List<ProfessionalFeeView> GetListProfeeofTimesheetByEmp(int TimeSheetID, int EmployeeID)
        {
            var context = _sqlExecuter.GetTLSDBContext();            
            var query = (from pf in context.ProfessionalFees
                         join t in context.TimeSheets on pf.TimeSheetID equals t.Id
                         join j in context.TaskNames on pf.JobCodeID equals j.Id
                         join c in context.Claims on t.ClaimID equals c.Id
                         where (t.Id == TimeSheetID) && (pf.CreateBy == EmployeeID)
                         select new ProfessionalFeeView
                         {
                             ProfessionalFeeID = pf.Id,
                             TimeSheetID = pf.TimeSheetID.Value,
                             InputDate = pf.InputDate.Value,
                             JobCodeID = pf.JobCodeID,
                             Notes = pf.Notes,
                             //WorkingHour = pf.WorkingHour,

                             //ApprovedHour = (pf.ApprovedHour==null ? pf.WorkingHour : pf.ApprovedHour),
                             CreateDate = pf.CreateDate.Value,
                             CreateBy = pf.CreateBy.Value,
                             UpdateBy = pf.UpdateBy.Value,
                             UpdateDate = pf.UpdateDate.Value,

                             JobName = j.Name,
                             StandardTime = j.StandardTime,

                             CreateByName = (from e in context.Employees where e.Id == pf.CreateBy select e.Name).FirstOrDefault(),
                             //hvtam-27082015
                             //FeePerHour = (from e in context.Employees where e.ID == pf.CreateBy select e.Fee).FirstOrDefault().Value,
                             FeePerHour = pf.FeePerHour.Value,
                             //endhvtam-27082015

                             ProFeeValue = pf.ProFeeValue ?? 0,


                             WorkTime = pf.WorkTime,
                             //ApproveTime = pf.WorkTime,                                 
                             ApproveTime = (pf.ApproveTime == null ? pf.WorkTime : pf.ApproveTime),
                             //ApproveTime = pf.ApproveTime,

                             //hvtam-09052015: Charged fee
                             ChargedBy = (pf.ChargedBy == null ? pf.CreateBy : pf.ChargedBy),
                             ChargedByName = (from e in context.Employees where e.Id == (pf.ChargedBy == null ? pf.CreateBy : pf.ChargedBy) select e.Name).FirstOrDefault(),
                             ChargedFeePerHour = (pf.ChargedFeePerHour == null ? pf.FeePerHour : pf.ChargedFeePerHour),
                             ChargedProFeeValue = (pf.ChargedProFeeValue == null ? pf.ProFeeValue : pf.ChargedProFeeValue),


                         }).OrderByDescending(t => t.InputDate); //hvtam-08082015: t => t.CreateDate
            return query.ToList();
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
