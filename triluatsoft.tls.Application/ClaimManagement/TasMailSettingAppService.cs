using Abp.Configuration;
using Abp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Configuration.Host.Dto;

namespace triluatsoft.tls.ClaimManagement
{
    public class TasMailSettingAppService : tlsAppServiceBase, ITasMailSettingAppService
    {

        public EmailSettingsEditDto GetEmailSettings()
        {
            return new EmailSettingsEditDto
            {
                DefaultFromAddress = SettingManager.GetSettingValue(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = SettingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = SettingManager.GetSettingValue(EmailSettingNames.Smtp.Host),
                SmtpPort = SettingManager.GetSettingValue<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = SettingManager.GetSettingValue(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SettingManager.GetSettingValue(EmailSettingNames.Smtp.Password),
                SmtpDomain = SettingManager.GetSettingValue(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = SettingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = SettingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }
    }
}
