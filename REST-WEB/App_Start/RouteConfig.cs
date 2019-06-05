using System.Web.Mvc;
using System.Web.Routing;

namespace REST_WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "display",
                url: "display/{ip1}.{ip2}.{ip3}.{ip4}/{port}/{time}",
                 defaults: new { controller = "My", action = "display", time = 0 }
            );

            routes.MapRoute(
                name: "save",
                url: "save/{ip}/{port}/{second}/{time}/{name}",
                defaults: new { controller = "My", action = "save" }
            );

            routes.MapRoute(
                name: "DisplayFile",
                url: "display/{name}/{interval}",
                defaults: new { controller = "My", action = "DisplayFile" }
            );

            routes.MapRoute(
            name: "Default",
            url: "{action}/{id}",
            defaults: new { controller = "My", action = "Def", id = UrlParameter.Optional }
    );
        }

    }

}
