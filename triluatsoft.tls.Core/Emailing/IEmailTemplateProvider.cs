namespace triluatsoft.tls.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate();

        string GetTemplate(string template);
    }
}
