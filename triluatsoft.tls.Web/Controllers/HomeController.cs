using System.Web.Mvc;

namespace triluatsoft.tls.Web.Controllers
{
    public class HomeController : tlsControllerBase
    {
        public ActionResult Index()
        {
            if (AbpSession.UserId.HasValue)
            {
                return Redirect(Url.Action("Index", "Application"));
            }
            else
            {
                return Redirect(Url.Action("Index", "Account/Login"));
            }
        }
	}
}