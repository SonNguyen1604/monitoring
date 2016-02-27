using BotDetect.Web.UI.Mvc;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;
using TestPage.Models;

namespace TestPage.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Info()
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            var dao = new UserDao();
            var model = dao.ViewDetails(session.UserID);
            ViewBag.ID = session.UserID;
            return View(model);
        }

      
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetails(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(TbStudent user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Info", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View("Info");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "RegisterCaptcha", "Captcha nhập không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                if(dao.CheckUserName(model.Username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                }
                else
                {
                    var user = new TbStudent();
                    user.Username = model.Username;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Ho = model.Ho;
                    user.Ten = model.Ten;
                    user.NgaySinh = model.NgaySinh;
                    user.GioiTinh = model.GioiTinh;
                    user.Active = true;
                    var result = dao.Insert(user);
                    if(result >0)
                    {
                        ViewBag.Success = "Đăng ký thành công.";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstant.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var dao = new UserDao();
            var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
            if (result == 1)
            {
                var user = dao.GetByID(model.UserName);
                var userSesssion = new UserLogin();
                userSesssion.Username = user.Username;
                userSesssion.UserID = user.ID;
                Session["userlogin"] = user.Ho + " " + user.Ten;
                Session.Add(Common.CommonConstant.USER_SESSION, userSesssion);
                return RedirectToAction("Index", "Home", userSesssion.Username);
            }
            else if (result == 0)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại.");
            }
            else if (result == -1)
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa.");
            }
            else if (result == -2)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng.");
            }
            else
            {
                ModelState.AddModelError("", "Không thể đăng nhập.");
            }
            return View(model);
        }
    }
}
