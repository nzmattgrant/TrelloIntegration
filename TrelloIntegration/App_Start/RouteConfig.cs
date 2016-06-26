using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TrelloIntegration
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Board",
               url: "Dashboard/Board/{boardID}",
               defaults: new { controller = "Dashboard", action = "Board", boardID = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Card",
               url: "Dashboard/Card/{cardID}",
               defaults: new { controller = "Dashboard", action = "Card", cardID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
