using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace triluatsoft.tls.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public const int MinPlainPasswordLength = 6;

        public const int MaxPhoneNumberLength = 24;
   
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public virtual string JobTitle { get; set; }
        public virtual string JobPosition { get; set; }
        public virtual string Address { get; set; }
        public virtual string Phone { get; set; }
        public virtual DateTime? JoinDate { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public decimal? Fee { get; set; }

        public int EmployeeId { get; set; }
        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }
        
        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="password">Password</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
                   {
                       TenantId = tenantId,
                       UserName = AdminUserName,
                       Name = AdminUserName,
                       Surname = AdminUserName,
                       EmailAddress = emailAddress,
                       Password = new PasswordHasher().HashPassword(password)
                   };
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
            Password = new PasswordHasher().HashPassword("123qwe");//defaut
        }
    }
}