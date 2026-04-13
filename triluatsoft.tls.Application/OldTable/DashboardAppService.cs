using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.OldViewClass;
using triluatsoft.tls.OldTable.View;
using triluatsoft.tls.OldUtils;

namespace triluatsoft.tls.OldTable
{
    public class DashBoardAppService : tlsAppServiceBase, IDashBoardAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public DashBoardAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }

        public List<BorderauxView> GetAll()
        {
            var cur_user = GetCurrentUser();
            var curr_role = "";
            var context = _sqlExecuter.GetTLSDBContext();

            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == cur_user.Id
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

            DateTime startDate = new DateTime(2017, 1, 1);

            var query = from c in context.Claims
                        join h in context.Bordereauxs on c.BordereauxID equals h.Id
                        join ec in context.EmployeeClaims on c.Id equals ec.ClaimID
                        where (curr_role == "Admin" || ec.EmployeeID == cur_user.EmployeeId)
                            && (c.Closed == null || c.Closed == false)
                            //&& (ec.EmployeeID == GetCurrentUser().Id)
                            && (h.Report.Name != "ILA")
                            //Modidfied
                            && (c.RefStatusID == REF_STATUS_DEFINE.OPEN)
                            && (h.CreatedDate >= startDate)
                        select new BorderauxView
                        {
                            ClaimID = c.Id,
                            RecentCorrespondence = h.RecentCorrespondence,
                            CreatedBy = h.CreatedBy,
                            CreatedDate = h.CreatedDate,
                            DateOfReport = h.DateOfReport,
                            ReportName = h.Report.Name,
                            FollowUpID = h.FollowUpID,
                            FollowUp = h.FollowUp.Name,
                        };
            return query.Distinct().ToList();

        }

        public List<NotificationView> LoadNotifications()
        {
            int dayLimit = ConfigHelper.GetIntOrDefault(ConfigKeys.DAY_LIMIT, 14);
            int ILADayLimit = ConfigHelper.GetIntOrDefault(ConfigKeys.ILA_DAY_LIMIT, 4);
            int prepDayLimit = ConfigHelper.GetIntOrDefault(ConfigKeys.PREP_DAY_LIMIT, 5);

            //List<BorderauxView> histList = HomeServiceUOW.GetNotifications(
            //    HasPermission(PageCapability.Dashboard_Notification_View)
            //        ? 0 : (User.EmployeeID.HasValue ? User.EmployeeID.Value : 0));

            List<BorderauxView> histList = GetAll();

            List<NotificationView> notificationList = new List<NotificationView>();
            foreach (var item in histList)
            {
                NotificationView n = new NotificationView();
                n.ClaimID = item.ClaimID;
                if (item.RecentCorrespondence.HasValue)
                {
                    n.LastUpdate = item.RecentCorrespondence.Value;
                } else
                {
                    if (item.DateOfReport.HasValue)
                    {
                        n.LastUpdate = item.DateOfReport.Value;
                    } else
                    {
                        n.LastUpdate = item.CreatedDate.Value;
                    }
                    
                }
                
                //n.RecentCorrespondence = item.RecentCorrespondence.Value;
                n.Remark = "OK";

                TimeSpan days = new TimeSpan();
                if (string.IsNullOrEmpty(item.RecentCorrespondence.ToString()))
                {                    
                    days = DateTime.Now.Date - (item.DateOfReport.HasValue ? item.DateOfReport.Value.Date : item.CreatedDate.Value.Date);
                    n.RecentCorrespondence = item.DateOfReport.HasValue ? item.DateOfReport : item.CreatedDate;

                    if (days.Days > 15)
                    {
                        //NotificationView n = new NotificationView();
                        //n.ClaimID = item.ClaimID;
                        //n.LastUpdate = item.CreatedDate.Value;
                        n.Message = string.Format("No update for {0} working days since the last update.", days.Days);
                        n.Remark = "MISSED";
                        //n.RecentCorrespondence = item.RecentCorrespondence.Value;                    
                        //notificationList.Add(n);
                    }
                } else
                {
                    days = DateTime.Now.Date - item.RecentCorrespondence.Value.Date;
                    n.RecentCorrespondence = item.RecentCorrespondence;
                    if (item.ReportName != null && item.ReportName.ToUpper() == "ILA" && days.Days > 15)
                    {
                        //NotificationView n = new NotificationView();
                        //n.ClaimID = item.ClaimID;
                        //n.LastUpdate = item.CreatedDate.Value;
                        n.Message = string.Format("No update for {0} working days since the issuance date of ILA.", days.Days);
                        n.Remark = "MISSED";
                        //n.RecentCorrespondence = item.RecentCorrespondence.Value;
                        //notificationList.Add(n);
                    } else
                    {
                        string followUp = item.FollowUp == null ? string.Empty : item.FollowUp.ToLower();
                        if (followUp.StartsWith("prepare")
                            && (followUp.Contains("final report")
                                || followUp.Contains("interim report")
                                || followUp.Contains("preliminary report")))
                        {
                            //NotificationView n = new NotificationView();
                            //n.ClaimID = item.ClaimID;
                            //n.LastUpdate = item.CreatedDate.Value;
                            if (days.Days <= 15)
                            {
                                n.Style = "primary";
                                n.Message = string.Format("Deadline will be on {0}",
                                    (n.LastUpdate.Date.AddDays(15)));
                            }
                            else
                            {
                                n.Style = "warning";
                                n.Message = "Deadline for issuing PRE/INT/FIN was missed.";
                                n.Remark = "MISSED";
                            }
                            //notificationList.Add(n);
                        } else
                        {
                            n.Message = followUp;
                        }
                    }
                }
                if (n.Remark == "MISSED")
                {
                    notificationList.Add(n);
                }                
            }
            List<NotificationView> BorderauxNotifications = notificationList.OrderBy(o => o.Remark).ThenBy(o => o.RecentCorrespondence).ToList(); //hvtam-21022016

            return BorderauxNotifications;
        }

        public bool isAdminCurrentUser()
        {
            var curr_user = GetCurrentUser();
            var context = _sqlExecuter.GetTLSDBContext();
            var role_query = from r in context.Roles
                             join ur in context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == curr_user.Id
                             select new triluatsoft.tls.OldTable.RoleView
                             {
                                 ID = r.Id,
                                 Name = r.Name,
                                 DisplayName = r.DisplayName
                             };
            var roles = role_query.ToList();
            foreach (var role in roles)
            {
                if (role.Name == "Admin")
                {
                    return true;
                }
            }

            return false;
        }

    }
}
