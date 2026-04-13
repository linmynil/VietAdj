using Abp.IO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.Configuration.Host;
using triluatsoft.tls.Configuration.Host.Dto;
using triluatsoft.tls.Emailing;
using static triluatsoft.tls.Emailing.EmailTemplateProvider;

namespace triluatsoft.tls.ClaimManagement
{
    public class TasMailAppService : tlsAppServiceBase, ITasMailAppService
    {
        private SmtpClient client;
        private readonly ITasMailSettingAppService _tasMailSettingAppService;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private EmailSettingsEditDto email;

        public TasMailAppService(ITasMailSettingAppService tasMailSettingAppService
            , IEmailTemplateProvider emailTemplateProvider)
        {
            _tasMailSettingAppService = tasMailSettingAppService;
            _emailTemplateProvider = emailTemplateProvider;
            client = new SmtpClient();
            email = _tasMailSettingAppService.GetEmailSettings();
            client.Host = email.SmtpHost;            
            client.UseDefaultCredentials = email.SmtpUseDefaultCredentials;
            client.EnableSsl = email.SmtpEnableSsl;
            client.Port = email.SmtpPort;
            client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName
                , email.SmtpPassword);
            //client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName, email.SmtpPassword, email.SmtpDomain);
            client.Timeout = 10000;
        }

        public Task SendAsync(List<string> to, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(email.DefaultFromAddress, email.DefaultFromDisplayName, System.Text.Encoding.UTF8)
            };

            foreach (string t in to)
            {
                mail.To.Add(new MailAddress(t));
            }            

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            if (client.Credentials == null)
            {
                client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName
                    , email.SmtpPassword);
            }
            return client.SendMailAsync(mail);
        }

        public void Send(List<string> to, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(email.DefaultFromAddress, email.DefaultFromDisplayName, System.Text.Encoding.UTF8)
            };

            foreach (string t in to)
            {
                mail.To.Add(new MailAddress(t));
            }            

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            if (client.Credentials == null)
            {
                client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName
                    , email.SmtpPassword);
            }
            client.Send(mail);
        }

        public Task SendAsyncTLS(VAJMailList mailList, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(email.DefaultFromAddress, email.DefaultFromDisplayName, System.Text.Encoding.UTF8)
            };

            foreach(string t in mailList.toList)
            {
                mail.To.Add(new MailAddress(t));
            }
            
            foreach (string c in mailList.ccList)
            {
                mail.CC.Add(new MailAddress(c));
            }
            
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            if (client.Credentials == null)
            {
                client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName
                    , email.SmtpPassword);
            }            
            return client.SendMailAsync(mail);
        }

        public void SendTLS(VAJMailList mailList, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(email.DefaultFromAddress, email.DefaultFromDisplayName, System.Text.Encoding.UTF8)
            };

            foreach (string t in mailList.toList)
            {
                mail.To.Add(new MailAddress(t));
            }            
            foreach (string c in mailList.ccList)
            {
                mail.CC.Add(new MailAddress(c));
            }
            
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            if (client.Credentials == null)
            {
                client.Credentials = new System.Net.NetworkCredential(email.SmtpUserName
                    , email.SmtpPassword);
            }            
            client.Send(mail);
        }
        
        public async Task SendTestEmail(SendTestEmailInput input)
        {
            VAJMailList mail_list = new VAJMailList();            
            mail_list.toList.Add(input.EmailAddress);            
            mail_list.ccList.Add(input.EmailAddress);
            await SendAsyncTLS(
                mail_list,
                L("TestEmail_Subject"),
                L("TestEmail_Body")
            );
        }
        public async Task SendClaimConfirmAuto(string claimID, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_CLAIMCONFIRM));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);                        

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - INSURER - ACKNOWLEDGEMENT", emailTemplate.ToString());
        }
        public async Task SendClaimConfirm(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_CLAIMCONFIRM));            

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{THE_INSURED}", TheInsurer);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            VAJMailList mail_list = new VAJMailList();            
            foreach(string t in mailList.toList)
            {
                mail_list.toList.Add(t);
            }
            foreach(string c in mailList.ccList)
            {
                mail_list.ccList.Add(c);
            }
            await SendAsyncTLS(mail_list, "CLAIM NO. " + claimID + " - " + TheInsurer + " - ACKNOWLEDGEMENT", emailTemplate.ToString());
        }
        public async Task SendClaimConfirmRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_CLAIMCONFIRMREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{THE_INSURED}", TheInsurer);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            VAJMailList mail_list = new VAJMailList();            
            foreach (string t in mailList.toList)
            {
                mail_list.toList.Add(t);
            }
            foreach (string c in mailList.ccList)
            {
                mailList.toList.Add(c);
            }

            await SendAsyncTLS(mail_list, "CLAIM NO. " + claimID + " - " + TheInsurer + " - ACKNOWLEDGEMENT - REMINDER NO. " + remindNo, emailTemplate.ToString());
        }
        public void SendDocumentRequest(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_DOCUMENTREQUEST));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - DOCUMENT REQUEST", emailTemplate.ToString());
        }
        public async Task SendDocumentRequestRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_DOCUMENTREQUESTREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - DOCUMENT REQUEST - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendFIN(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_FIN));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - FIN", emailTemplate.ToString());
        }
        public async Task SendFINRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_FINREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - FIN - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendFINHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_FINHARDCOPY));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - FIN", emailTemplate.ToString());
        }
        public async Task SendFINHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_FINHARDCOPYREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " = " + TheInsurer + " - FIN - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendILA(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_ILA));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - ILA", emailTemplate.ToString());
        }
        public async Task SendILARemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_ILAREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - ILA - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendINT(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_INT));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - INT", emailTemplate.ToString());
        }
        public async Task SendINTRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_INTREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - INT - REMINDER NO. " + remindNo, emailTemplate.ToString());
        }
        public void SendINTHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_INTHARDCOPY));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - INT", emailTemplate.ToString());
        }
        public async Task SendINTHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_INTHARDCOPYREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - INT - REMINDER NO. " + remindNo, emailTemplate.ToString());
        }
        public void SendMeetingNote(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_MEETINGNOTE));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            VAJMailList mail_list = new VAJMailList();            
            foreach (string t in mailList.toList)
            {
                mail_list.toList.Add(t);
            }
            foreach (string c in mailList.ccList)
            {
                mail_list.ccList.Add(c);
            }

            SendTLS(mail_list, "CLAIM NO. " + claimID + " - " + TheInsurer + " - MEETING NOTE", emailTemplate.ToString());
        }
        public async Task SendMeetingNoteRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_MEETINGNOTEREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            VAJMailList mail_list = new VAJMailList();            
            foreach (string t in mailList.toList)
            {
                mail_list.toList.Add(t);
            }
            foreach (string c in mailList.ccList)
            {
                mail_list.ccList.Add(c);
            }

            await SendAsyncTLS(mail_list, "CLAIM NO. " + claimID + " - " + TheInsurer + " - MEETING NOTE - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendPRE(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_PRE));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - PRE", emailTemplate.ToString());
        }
        public async Task SendPRERemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_PREREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - PRE - REMINDER NO. " + remindNo, emailTemplate.ToString());
        }
        public void SendPREHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_PREHARDCOPY));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - PRE", emailTemplate.ToString());
        }
        public async Task SendPREHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_PREHARDCOPYREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - PRE - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
        public void SendTimeSheet(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_TIMESHEET));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);

            SendTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - TIMESHEET", emailTemplate.ToString());
        }
        public async Task SendTimeSheetRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetTemplate(EmailTemplate.MAIL_TIMESHEETREMIND));

            emailTemplate.Replace("{CLAIM_ID}", claimID);
            emailTemplate.Replace("{DEADLINE_TIME}", deadlineTime);
            emailTemplate.Replace("{REMINDER_NO}", remindNo);

            await SendAsyncTLS(mailList, "CLAIM NO. " + claimID + " - " + TheInsurer + " - TIMESHEET - REMINDER NO. " + remindNo, emailTemplate.ToString());            
        }
    }
}
