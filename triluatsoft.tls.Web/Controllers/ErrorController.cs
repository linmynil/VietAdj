using System.Web.Mvc;
using Abp.Auditing;

namespace triluatsoft.tls.Web.Controllers
{
    public class ErrorController : tlsControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}