using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;

namespace TestPage.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        //
        // GET: /Admin/Exam/
         [HasCredential(RoleID = "VIEW_DETHI")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ExamDao();
            var model = dao.GetDeThiAdmin(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

         [HasCredential(RoleID = "VIEW_DETHI")]
         public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
         {
             var dao = new ExamDao();
             var model = dao.GetDeThiAdminActive(searchString, page, pageSize);
             ViewBag.SearchString = searchString;
             return View(model);
         }

        [HasCredential(RoleID = "ADD_DETHI")]
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBagLevel();
            SetViewBagObject();
            SetViewBagDt();
            return View();
        }
        [HasCredential(RoleID = "EDIT_DETHI")]
        public ActionResult Edit(int id)
        {
            var exam = new ExamDao().GetByID(id);
            SetViewBagLevel(exam.MaCD);
            SetViewBagObject(exam.MaMT);
            SetViewBagDt(exam.MaDT);
            return View(exam);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            //new ExamDao().Delete(id);
            var dao = new ExamDao();
            var path = dao.GetByID(id);
            var fullpath = Server.MapPath(path.DuongDanDT);
            if(System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }
            dao.Delete(id);
            return RedirectToAction("Index");
        }

        public void SetViewBagLevel(int? selected=null)
        {
            var dao = new LevelDao();
            ViewBag.MaCD = new SelectList(dao.ListAll(), "MaCD", "TenCD", selected);
        }

        public void SetViewBagObject(int? selected =null)
        {
            var dao = new ObjectDao();
            ViewBag.MaMT = new SelectList(dao.ListAll(), "MaMT", "TenMT", selected);
        }

        public void SetViewBagDt(int? selected = null)
        {
            var dao = new DtDao();
            ViewBag.MaDT = new SelectList(dao.ListAll(), "MaDT", "TenDT", selected);
        }


        [HttpPost]
        public ActionResult Create(TbDeThi exam)
        {
            var dao = new ExamDao();
            var id = dao.Insert(exam);
            if (id > 0)
            {
                SetAlert("Thêm đề thi mới thành công", "success");
                return RedirectToAction("Index", "Exam");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới không thành công");
            }

            SetViewBagLevel();
            SetViewBagObject();
            SetViewBagDt();
            return View("Index");
        }


        [HttpPost]
        public ActionResult Edit(TbDeThi exam)
        {
            if (ModelState.IsValid)
            {
                var dao = new ExamDao();
                var result = dao.Update(exam);
                if (result)
                {
                    SetAlert("Sửa đề thi thành công", "success");
                    return RedirectToAction("Index", "Exam");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View("Index");
        }

        [HasCredential(RoleID = "EDIT_DETHI")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new ExamDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

    }
}
