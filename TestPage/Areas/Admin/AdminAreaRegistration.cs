using System.Web.Mvc;

namespace TestPage.Areas.Admin
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
                "Admin_default",
                "Admin/{controller}/{action}/{id}/{pro}",
                new { action = "Index", id = UrlParameter.Optional, pro="" }
                //new []{"TestPage.Areas.Admin.Controller"}
            );
        }
    }
}
