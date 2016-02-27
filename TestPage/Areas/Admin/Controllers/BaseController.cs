using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestPage.Common;

namespace TestPage.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin) Session[CommonConstant.USER_SESSION];
            if(session==null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if(type=="success")
            {
                TempData["AlertType"] = "alert-success alert-dismissable";
            }
            else if (type=="warning")
            {
                TempData["AlertType"] = "alert-warning alert-dismissable";
            }
            else if(type=="error")
            {
                TempData["AlertType"] = "alert-info alert-dismissable";
            }
        }
    }
}
