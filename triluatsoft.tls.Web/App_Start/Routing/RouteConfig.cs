using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace triluatsoft.tls.Web.Routing
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //fix bug 404 CrystalImageHandler.aspx
            //routes.IgnoreRoute("Reports/{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*(CrystalImageHandler).*" });
            //end fix

            //ASP.NET Web API Route Config
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "triluatsoft.tls.Web.Controllers" }
            );

        }
    }
}
