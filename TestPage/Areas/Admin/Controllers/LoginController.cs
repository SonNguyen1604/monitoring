using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Areas.Admin.Models;
using TestPage.Common;

namespace TestPage.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.Username, Encryptor.MD5Hash(model.Password), true);
                if(result==1)
                {
                    var user = dao.GetByID(model.Username);
                    var userSesssion = new UserLogin();
                    userSesssion.Username=user.Username;
                    userSesssion.UserID=user.ID;
                    userSesssion.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.Username);
                    Session.Add(CommonConstant.SESSION_CREDENTIAL, listCredentials);
                    Session["userlogin"] = user.Ho +" "+ user.Ten;
                    Session.Add(Common.CommonConstant.USER_SESSION, userSesssion);
                    return RedirectToAction("Index", "Home",userSesssion.Username);
                }
                else if (result==0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result==-1)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                }
                else if (result==-2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Không thể đăng nhập.");
                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session[CommonConstant.USER_SESSION] = null;
            return RedirectToAction("Index");
        }

    }
}
