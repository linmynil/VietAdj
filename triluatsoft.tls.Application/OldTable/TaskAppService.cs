using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.OldTable
{
    public class TaskAppService : tlsAppServiceBase, ITaskAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Task> _taskRepo;


        public TaskAppService(ISqlExecuter sqlExecuter
            , IRepository<Task> taskRepo)
        {
            _sqlExecuter = sqlExecuter;
            _taskRepo = taskRepo;
        }
        public List<TaskView> GetAll(TaskSearchOptions options)
        {

            //TODO ADD PERMISSION
            //List<SecGroupView> group = SecurityServiceUOW.GetGroupsByUser(User.UserID);
            //foreach (SecGroupView sgv in group)
            //{
            //    if (sgv.ID == 1)
            //    {
            //        options.SearchAll = true;
            //        break;
            //    }
            //    options.SearchAll = false;
            //}
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
                curr_role = role.Name;
            }

            if (curr_role == "Admin")
            {
                options.SearchAll = true;
            } else
            {
                options.SearchAll = false;
            }

            //var context = _sqlExecuter.GetTLSDBContext();

            var query = from t in context.Tasks
                        join u in UserManager.Users on t.EmployeeID equals (int)u.EmployeeId
                        join e in UserManager.Users on t.CreatedBy equals (int)e.EmployeeId
                        where (options.TaskName == null || options.TaskName == ""
                                || (u.UserName != null && u.UserName.Contains(options.TaskName))
                                || (e.UserName != null && e.UserName.Contains(options.TaskName))
                                || (t.TaskName != null && t.TaskName.Name.Contains(options.TaskName))
                                || (t.TaskName != null && t.TaskName.JobCode.Contains(options.TaskName)))
                            && (t.ClaimID.Contains(options.ClaimID)
                                || options.ClaimID == null || options.ClaimID == "")
                            && (options.SearchAll //Admin: Select all tasks
                                || (t.CreatedBy == curr_user.EmployeeId //User: Select their own tasks or assigned tasks
                                    || t.EmployeeID == curr_user.EmployeeId
                                    //Account Manager: Select tasks of their claims (can be created by Admin)
                                    || (t.Claim != null && t.Claim.AccountManagerID == curr_user.EmployeeId)
                                    ))
                                    && (options.StartDate == null || t.EndDate >= options.StartDate)
                               && (options.EndDate == null || t.EndDate <= options.EndDate)
                            && (options.Status == null //all status
                                || (options.Status == 1 && t.IsCompleted == true) //Completed
                                || (options.Status == 2 && (!t.IsCompleted.HasValue || t.IsCompleted == false))) //In-progress

                        select new TaskView
                        {
                            ID = t.Id,
                            TaskNameID = t.TaskNameID,
                            TaskName = (t.TaskName != null ? t.TaskName.Name : string.Empty),
                            JobCode = (t.TaskName != null ? t.TaskName.JobCode : string.Empty),
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Type = t.Type,
                            IsCompleted = t.IsCompleted,
                            EmployeeID = t.EmployeeID,
                            //Assignee = (t.Employee == null ? string.Empty : t.Employee.Name),
                            //Modify hien thi ten viet tat user
                            Assignee = (t.Employee == null ? string.Empty : u.UserName),
                            //WorkingHour = t.WorkingHour,
                            CreatedBy = t.CreatedBy,
                            //Modify hien thi ten viet tat user
                            //CreatedUser = (i != null ? i.Name : string.Empty),
                            CreatedUser = (e != null ? e.UserName : string.Empty),
                            CreatedDate = t.CreatedDate,
                            ClaimID = t.ClaimID,    //hvtam-04032016
                            IsEditable = (t.Claim.Closed == null || t.Claim.Closed == false),    //hvtam-04032016
                            IsMissed = (((t.EndDate.Value < DateTime.Now) && (t.IsCompleted == false)) ? true : false)
                        };
            if (options.Type == 1)
            {
                query = query.Where(p => p.CreatedUser == options.TaskName);
            }
            else if (options.Type == 2)
            {
                query = query.Where(p => p.Assignee == options.TaskName);
            }

            var result = query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.StartDate).ToList();
            return result;
        }

        public string Save(CreateOrUpdateTaskInput input)
        {
            Task task;
            if (input.Id.HasValue)
            {
                task = _taskRepo.FirstOrDefault(input.Id.Value);
            }
            else
            {
                task = new Task();
            }
            task.ClaimID = input.ClaimID;
            var currUser = GetCurrentUser();
            if (!input.EmployeeID.HasValue)
            {
                task.EmployeeID = (int)currUser.EmployeeId;
                task.Type = false;
            }
            else
            {
                task.EmployeeID = input.EmployeeID.Value;
                task.Type = true;
            }

            task.TaskNameID = input.TaskNameID;
            task.Description = input.Description.Trim();
            task.StartDate = input.StartDate;
            task.EndDate = input.EndDate;
            task.IsCompleted = input.IsCompleted;
            task.CreatedDate = DateTime.Now;
            task.CreatedBy = (int)currUser.EmployeeId;
            if (!input.Id.HasValue)
            {
                _taskRepo.InsertAndGetId(task);
            }
            else
            {
                _taskRepo.Update(task);
            }


            return "ok";
        }

        public TaskView GetById(int taskId)
        {
            Task task = _taskRepo.FirstOrDefault(taskId);
            if (task == null) return null;

            TaskView tv = new TaskView
            {
                ID = task.Id,
                ClaimID = task.ClaimID,
                EmployeeID = task.EmployeeID,
                TaskNameID = task.TaskNameID,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                IsCompleted = task.IsCompleted
                
            };
            return tv;
        }

        public string DeleteById(int taskId)
        {

            Task task = _taskRepo.FirstOrDefault(taskId);
            _taskRepo.Delete(task);
            return "ok";
        }

        public List<TaskView> GetToDashboard()
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

            var query = from t in context.Tasks
                        join u in context.Users on t.EmployeeID equals (int)u.EmployeeId
                        join e in context.Users on t.CreatedBy equals (int)e.EmployeeId
                        where (curr_role == "Admin" //Admin: Select all tasks
                                || (t.CreatedBy == curr_user.EmployeeId //User: Select their own tasks or assigned tasks
                                    || t.EmployeeID == curr_user.EmployeeId
                                    //Account Manager: Select tasks of their claims (can be created by Admin)
                                    || (t.Claim != null && t.Claim.AccountManagerID == curr_user.EmployeeId)
                                    ))
                        select new TaskView
                        {
                            ID = t.Id,
                            TaskNameID = t.TaskNameID,
                            TaskName = (t.TaskName != null ? t.TaskName.Name : string.Empty),
                            JobCode = (t.TaskName != null ? t.TaskName.JobCode : string.Empty),
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Type = t.Type,
                            IsCompleted = t.IsCompleted,
                            EmployeeID = t.EmployeeID,
                            //Assignee = (t.Employee == null ? string.Empty : t.Employee.Name),
                            //Modify hien thi ten viet tat user
                            Assignee = (t.Employee == null ? string.Empty : u.UserName),
                            //WorkingHour = t.WorkingHour,
                            CreatedBy = t.CreatedBy,
                            //Modify hien thi ten viet tat user
                            //CreatedUser = (i != null ? i.Name : string.Empty),
                            CreatedUser = (e != null ? e.UserName : string.Empty),
                            CreatedDate = t.CreatedDate,
                            ClaimID = t.ClaimID,    //hvtam-04032016
                            IsEditable = (t.Claim.Closed == null || t.Claim.Closed == false),    //hvtam-04032016
                            IsMissed = (((t.EndDate.Value < DateTime.Now) && (t.IsCompleted == false)) ? true : false)
                        };
            var result = query.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.StartDate).ToList();
            return result;
        }

        public bool CheckPermission_CreateDeadline()
        {
            bool create_permission = false;
            create_permission = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DeadlinesManagement_CreateDeadline).Result;
            return create_permission;
        }

        public bool CheckPermission_SearchDeadline()
        {
            bool search_permission = false;
            search_permission = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DeadlinesManagement_SearchDeadline).Result;
            return search_permission;
        }

        public bool CheckPermission_EditDeadline()
        {
            bool edit_permission = false;
            edit_permission = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DeadlinesManagement_EditMyDeadline).Result;
            return edit_permission;
        }

        public bool CheckPermission_DeleteDeadline()
        {
            bool delete_permission = false;
            delete_permission = PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DeadlinesManagement_EditMyDeadline).Result;
            return delete_permission;
        }
    }
}
