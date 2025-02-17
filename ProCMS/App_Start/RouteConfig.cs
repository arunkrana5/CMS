using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "BlogListRoute",
                url: "BlogList",
                defaults: new { controller = "Home", action = "BlogList" }
            );

            routes.MapRoute(
        name: "_ProductDemoRoute",
        url: "_ProductDemo",
        defaults: new { controller = "Home", action = "_ProductDemo" }
    );


            routes.MapRoute(
               name: "GalleryDetailsRoute",
               url: "GalleryDetails/{GalleryGroupID}",
               defaults: new { controller = "Home", action = "GalleryDetails" }
           );


            routes.MapRoute(
             name: "CategoryRoute",
             url: "Category/{Parameter1}",
             defaults: new { controller = "Home", action = "ProductIndex", Parameter1 = UrlParameter.Optional }
         );

            routes.MapRoute(
             name: "ProductDetailsRoute",
             url: "PD/{Parameter1}",
             defaults: new { controller = "Home", action = "ProductDetail", Parameter1 = UrlParameter.Optional }
         );

            routes.MapRoute(
                   name: "MyActionRoute",
                   url: "{Parameter1}/{Parameter2}",
                   defaults: new { controller = "Home", action = "MyAction", Parameter2 = UrlParameter.Optional }
                    );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            );
        }
    }
}
