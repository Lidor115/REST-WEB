using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                url: "display/{ip}/{port}",
                defaults: new { controller = "My", action = "display" }
            );

            routes.MapRoute(
                name: "displayWithTime",
                url: "display/{ip}/{port}/{time}",
                defaults: new { controller = "My", action = "displayWithTime", time = 0 }
            );

            routes.MapRoute(
                name: "save",
                url: "save/{ip}/{port}/{second}/{time}/{name}",
                defaults: new { controller = "My", action = "save"}
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "My", action = "Def", id = UrlParameter.Optional }
    );
        }

    }

}
