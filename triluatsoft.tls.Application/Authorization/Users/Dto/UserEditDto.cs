using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using System;

namespace triluatsoft.tls.Authorization.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    public class UserEditDto : IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }


        public string JobTitle { get; set; }


        public string JobPosition { get; set; }

        public string Birthday { get; set; }
        public string Address { get; set; }


        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(User.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public virtual bool IsTwoFactorEnabled { get; set; }

        public virtual bool IsLockoutEnabled { get; set; }

        public decimal? Fee { get; set; }

        public int EmployeeId { get; set; }
    }
}