using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Authorization.Users;
using triluatsoft.tls.Configuration.Host.Dto;

namespace triluatsoft.tls.ClaimManagement
{
    public interface ITasMailAppService : IApplicationService
    {
        Task SendAsync(List<string> to, string subject, string body, bool isBodyHtml = true);
        void Send(List<string> to, string subject, string body, bool isBodyHtml = true);
        Task SendAsyncTLS(VAJMailList mailList, string subject, string body, bool isBodyHtml = true);
        void SendTLS(VAJMailList mailList, string subject, string body, bool isBodyHtml = true);
        Task SendTestEmail(SendTestEmailInput input);        
        Task SendClaimConfirm(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendClaimConfirmRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendDocumentRequest(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendDocumentRequestRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendFIN(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendFINRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendFINHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendFINHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendILA(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendILARemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendINT(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendINTRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendINTHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendINTHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendMeetingNote(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendMeetingNoteRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendPRE(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendPRERemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendPREHardCopy(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendPREHardCopyRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
        void SendTimeSheet(string claimID, string TheInsurer, string deadlineTime, VAJMailList mailList);
        Task SendTimeSheetRemind(string claimID, string TheInsurer, string deadlineTime, string remindNo, VAJMailList mailList);
    }
}
