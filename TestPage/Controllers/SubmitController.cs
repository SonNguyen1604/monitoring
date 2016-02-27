using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;

namespace TestPage.Controllers
{
    public class SubmitController : BaseController
    {
        //
        // GET: /Screen/
     
        public ActionResult Index()
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            var thi = new UserDao().ViewDetails(session.UserID);
            if (thi.Thi == true)
            {
                return RedirectToAction("../Exam/Annoucement");
            }
            SetViewBagDT();
            return View();
        }

        
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
            var thi = new UserDao().ViewDetails(session.UserID);
            if (thi.Thi == false)
            {
                try
                {
                    /*Lopp for multiple files*/
                    foreach (HttpPostedFileBase file in files)
                    {
                        /*Geting the file name*/
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        /*Saving the file in server folder*/
                        file.SaveAs(Server.MapPath("/ExamUpload/" + filename));
                        string filepathtosave = "/ExamUpload/" + filename;
                        /*HERE WILL BE YOUR CODE TO SAVE THE FILE DETAIL IN DATA BASE*/
                        ViewBag.BT = "/ExamUpload/" + filename;
                    }

                    ViewBag.Message = "Tải lên thành công.";
                }
                catch
                {
                    ViewBag.Message = "Đã có lỗi xảy ra khi tải lên. Vui lòng thử lại.";
                }

                SetViewBagDT();
                return View("Index");
            }
            else
            {
               // return RedirectToAction("Index");
                //return Redirect("../Exam/Annoucement");
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Create(TbBaiThi bt)
        {
            var session = (TestPage.Common.UserLogin)Session[TestPage.Common.CommonConstant.USER_SESSION];
                var thi = new UserDao().ViewDetails(session.UserID);
            if (thi.Thi == true)
            {
                return Redirect("../Exam/Annoucement");
            }
            var dao = new BtDao();
            var id = dao.Insert(bt);
            if (id > 0)
            {
                string wc = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
                string mh = "/uploads/" + "SC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".webm";
                string at = "/uploads/" + "WC" + session.UserID + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".wav";
                var daobt = new BtDao().GetID(session.UserID).FirstOrDefault();
                var stt = dao.Update(daobt.MaBaiThi, wc, mh, at);

                var user = new UserDao();
                user.UpdateThi(session.UserID);
                SetAlert("Nộp thi thành công", "success");
                return RedirectToAction("Index", "Submit");
            }
            else
            {
                ModelState.AddModelError("", "Nộp bài thi không thành công");
            }
            SetViewBagDT();
            return View("Index");
        }

        public void SetViewBagDT(int? selected = null)
        {
            var dao = new BtDao();
            ViewBag.MaDeThi = new SelectList(dao.ListAll(), "MaDeThi", "TenDeThi", selected);
        }
    }
}
