using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Roles;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.OldTable
{
    public class OldUserAppService : tlsAppServiceBase, IOldUserAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        private UserManager _userManager;
        private RoleManager _roleManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public OldUserAppService(
            IRepository<UserRole, long> userRoleRepository,
            RoleManager roleManager,
            UserManager userManager, 
            ISqlExecuter sqlExecuter)
        {
            _userManager = userManager;
            _sqlExecuter = sqlExecuter;
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
        }

        public virtual async System.Threading.Tasks.Task MigrateOldUsers()
        {
            Logger.Debug(">> start >>");
            var userCredentials = _sqlExecuter.GetDatabase()
                .SqlQuery<UserView>("SELECT * " +
                   " from UserCredential Order By EmployeeID ").ToList();

            var userGroups = _sqlExecuter.GetDatabase()
                .SqlQuery<UserGroupView>("SELECT * " +
                   " from SecUserGroup").ToList();

            var employees = _sqlExecuter.GetDatabase()
                .SqlQuery<EmployeeView>("SELECT * " +
                   " from Employee").ToList();

            foreach (var u in userCredentials)
            {
                var userGroup = userGroups.Where(x => x.SecUserID.ToLower() == u.ID.ToLower()).ToList();

                if (userGroup.Count() == 0)
                {
                    Logger.Debug("user is not in any group " + u.ID);
                    continue;
                }
                if (userGroup.Count() > 2)
                {
                    Logger.Debug("user is in more than 2 group " + u.ID);
                    continue;
                }
                var em = employees.FirstOrDefault(x => x.ID == u.EmployeeID);
                var group = userGroup.First().SecGroupID;
                group++;

                Logger.Debug(u.ID + " " + group + " " + em.Name);

                var password = new PasswordHasher().HashPassword(u.Password);
                if (em.Email == null || string.IsNullOrEmpty(em.Email))
                {
                    em.Email = u.ID + "@vietadjusters.com";
                }
                var existed = _userManager.FindByEmail(em.Email);
                if (existed != null)
                {
                    Logger.Debug("existed user email " + em.Email);
                    continue;
                }
                existed = _userManager.FindByName(u.ID);

                if (existed != null)
                {
                    Logger.Debug("existed user name " + u.ID);
                    if (existed.EmployeeId == 0)
                    {
                        existed.EmployeeId = u.EmployeeID;
                    }
                    continue;
                }
                var insertUser = new User
                {
                    Name = em.Name,
                    Surname = em.Name,
                    UserName = u.ID,
                    IsActive = u.IsActive,
                    ShouldChangePasswordOnNextLogin = false,
                    Password = password,
                    EmailAddress = em.Email,
                    JobTitle = em.JobTitle,
                    JobPosition = em.JobPosition,
                    JoinDate = em.JoinDate,
                    Address = em.Address,
                    Phone = em.Phone,
                    DateOfBirth = em.DateOfBirth,
                    Fee = em.Fee,
                    EmployeeId = em.ID
                };
                Logger.Debug("inserting user " + insertUser.Name + " em ID " + insertUser.EmployeeId + " userID " + insertUser.Id);
                await _userManager.CreateAsync(insertUser);
                await CurrentUnitOfWork.SaveChangesAsync();
                Logger.Info("inserted user " + insertUser.Name + " em ID " + insertUser.EmployeeId + " userID " + insertUser.Id);
                var insertRole = new UserRole(null, insertUser.Id, 3);
                await _userRoleRepository.InsertAsync(insertRole);
                await CurrentUnitOfWork.SaveChangesAsync();
                Logger.Info(insertRole.RoleId + " " + insertRole.Id);
            }
            Logger.Debug(">> end >>");
        }
    }
}
