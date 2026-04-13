using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using triluatsoft.tls.Authorization.Users;

namespace triluatsoft.tls.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}