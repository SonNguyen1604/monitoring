using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;

namespace TestPage.Common
{
    public class HasCredentialAttribute:AuthorizeAttribute
    {
        public string RoleID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext) 
        {
            var session = (UserLogin)HttpContext.Current.Session[Common.CommonConstant.USER_SESSION];
            if(session==null)
            {
                return false;
            }
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.Username);
            if(privilegeLevels.Contains(this.RoleID) || session.GroupID== Model.CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result= new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
            };
        }

        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credential = (List<String>)HttpContext.Current.Session[CommonConstant.SESSION_CREDENTIAL];
            return credential;
        }
    }
}