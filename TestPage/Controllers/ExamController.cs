using Model.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestPage.Controllers
{
    public class ExamController : BaseController
    {
        //
        // GET: /Exam/
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
             var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
             var thi = new UserDao().ViewDetails(session.UserID);
            if(thi.Thi==true)
            {
                return RedirectToAction("Annoucement");
            }
            ViewBag.Files = Directory.EnumerateFiles(Server.MapPath("~/Data"));
            var dao = new ExamDao();
            var model = dao.GetDeThi(page, pageSize);
            ViewBag.count = dao.count();
            return View(model);
        }

        public ActionResult Annoucement()
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            return View();
        }
    }
}
