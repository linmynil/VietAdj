using System.ComponentModel.DataAnnotations;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.MultiTenancy;

namespace triluatsoft.tls.Web.Models.TenantRegistration
{
    public class TenantRegistrationViewModel
    {
        [Required]
        [StringLength(Tenant.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        public string AdminPassword { get; set; }
    }
}