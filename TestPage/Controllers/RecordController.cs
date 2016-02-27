using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;
using PagedList;

namespace TestPage.Controllers
{
    public class RecordController : BaseController
    {
        //
        // GET: /Exam/

        public ActionResult Index()
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            var exam = new BtDao().GetID(session.UserID).FirstOrDefault();
            var thi = new UserDao().ViewDetails(session.UserID);
            if (thi.Thi == true)
            {
                return RedirectToAction("../Exam/Annoucement");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(TbBaiThi bt)
        {
            ViewBag.Files = Directory.EnumerateFiles(Server.MapPath("~/Data"));
            var session = (UserLogin)Session[CommonConstant.USER_SESSION];
            var daobt = new BtDao().GetID(session.UserID).FirstOrDefault();
            ViewBag.WC = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
            ViewBag.MH = "/uploads/" + "SC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
            ViewBag.AT = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".wav";
            var dao = new BtDao();
            var stt = dao.Update(daobt.MaBaiThi, ViewBag.WC, ViewBag.MH, ViewBag.AT);
         
            if(stt==true)
            {
                SetAlert("Nộp video thành công", "success");
            }
          else
            {
                ModelState.AddModelError("", "Nộp video không thành công");
            }
          
            return View("Edit");
        }

        public ActionResult Edit()
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            var exam = new BtDao().GetID(session.UserID).FirstOrDefault();
            ViewBag.id = exam.MaBaiThi;
            ViewBag.WC = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
            ViewBag.MH = "/uploads/" + "SC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
            ViewBag.AT = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".wav";

            return View(exam);
        }

        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            foreach (string upload in Request.Files)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                var file = Request.Files[upload];
                if (file == null) continue;

                file.SaveAs(Path.Combine(path, Request.Form[0]));
            }
            return Json(Request.Form[0]);
        }
    }
}
