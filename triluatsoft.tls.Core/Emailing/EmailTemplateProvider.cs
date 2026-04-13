using System.Reflection;
using System.Text;
using Abp.Dependency;
using Abp.IO.Extensions;

namespace triluatsoft.tls.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ITransientDependency
    {
        public string GetDefaultTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("triluatsoft.tls.Emailing.EmailTemplates.default.html"))
            {
                var bytes = stream.GetAllBytes();
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
        }

        public string GetTemplate(string template)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(template))
            {
                var bytes = stream.GetAllBytes();
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
        }

        public class EmailTemplate
        {
            public const string MAIL_CLAIMCONFIRM = "triluatsoft.tls.Emailing.EmailTemplates.ClaimConfirm.html";
            public const string MAIL_CLAIMCONFIRMREMIND = "triluatsoft.tls.Emailing.EmailTemplates.ClaimConfirmRemind.html";
            public const string MAIL_DOCUMENTREQUEST = "triluatsoft.tls.Emailing.EmailTemplates.DocumentRequest.html";
            public const string MAIL_DOCUMENTREQUESTREMIND = "triluatsoft.tls.Emailing.EmailTemplates.DocumentRequestRemind.html";
            public const string MAIL_FIN = "triluatsoft.tls.Emailing.EmailTemplates.FIN.html";
            public const string MAIL_FINREMIND = "triluatsoft.tls.Emailing.EmailTemplates.FINRemind.html";
            public const string MAIL_FINHARDCOPY = "triluatsoft.tls.Emailing.EmailTemplates.FINHardCopy.html";
            public const string MAIL_FINHARDCOPYREMIND = "triluatsoft.tls.Emailing.EmailTemplates.FINHardCopyRemind.html";
            public const string MAIL_ILA = "triluatsoft.tls.Emailing.EmailTemplates.ILA.html";
            public const string MAIL_ILAREMIND = "triluatsoft.tls.Emailing.EmailTemplates.ILARemind.html";
            public const string MAIL_INT = "triluatsoft.tls.Emailing.EmailTemplates.INT.html";
            public const string MAIL_INTREMIND = "triluatsoft.tls.Emailing.EmailTemplates.INTRemind.html";
            public const string MAIL_INTHARDCOPY = "triluatsoft.tls.Emailing.EmailTemplates.INTHardCopy.html";
            public const string MAIL_INTHARDCOPYREMIND = "triluatsoft.tls.Emailing.EmailTemplates.INTHardCopyRemind.html";
            public const string MAIL_MEETINGNOTE = "triluatsoft.tls.Emailing.EmailTemplates.MeetingNote.html";
            public const string MAIL_MEETINGNOTEREMIND = "triluatsoft.tls.Emailing.EmailTemplates.MeetingNoteRemind.html";
            public const string MAIL_PRE = "triluatsoft.tls.Emailing.EmailTemplates.PRE.html";
            public const string MAIL_PREREMIND = "triluatsoft.tls.Emailing.EmailTemplates.PRERemind.html";
            public const string MAIL_PREHARDCOPY = "triluatsoft.tls.Emailing.EmailTemplates.PREHardCopy.html";
            public const string MAIL_PREHARDCOPYREMIND = "triluatsoft.tls.Emailing.EmailTemplates.PREHardCopyRemind.html";
            public const string MAIL_TIMESHEET = "triluatsoft.tls.Emailing.EmailTemplates.TimeSheet.html";
            public const string MAIL_TIMESHEETREMIND = "triluatsoft.tls.Emailing.EmailTemplates.TimeSheetRemind.html";
        }
    }
}