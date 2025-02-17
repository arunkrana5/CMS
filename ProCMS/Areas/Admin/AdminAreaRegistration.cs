using System.Web.Mvc;

namespace ProCMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
      name: "Admin_Login",
      url: "Admin/Login",
      defaults: new { controller = "Accounts", action = "Login" }
  );

            context.MapRoute(
     name: "Admin_Dashboard",
     url: "Admin/Dashboard",
     defaults: new { controller = "Index", action = "Dashboard" }
 );

            context.MapRoute(
            name: "Admin_LogoutRoute",
            url: "Admin/Logout",
            defaults: new { controller = "Accounts", action = "Logout" }
        );


            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Accounts",action = "Login", id = UrlParameter.Optional }

            );
        }

        
    }

}