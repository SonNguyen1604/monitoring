using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TestPage.Common;

namespace TestPage.Areas.Admin.Controllers
{
    public class ObjectController : BaseController
    {
        //
        // GET: /Admin/Monthi/
        [HasCredential(RoleID = "VIEW_SUBJECT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ObjectDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HasCredential(RoleID = "VIEW_SUBJECT")]
        public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ObjectDao();
            var model = dao.ListAllPagingActive(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HasCredential(RoleID = "ADD_SUBJECT")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HasCredential(RoleID = "EDIT_SUBJECT")]
        public ActionResult Edit(int id)
        {
            var Object = new ObjectDao().GetByID(id);
            return View(Object);
        }

        [HttpPost]
        public ActionResult Create(TbMT Object)
        {
            var dao = new ObjectDao();
            int id = dao.Insert(Object);
            if(id>0)
            {
                SetAlert("Thêm môn mới thành công","success");
                return RedirectToAction("Index", "Object");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới không thành công");
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(TbMT Object)
        {
            if(ModelState.IsValid)
            {
                var dao = new ObjectDao();
                var result = dao.Update(Object);
                if (result)
                {
                    SetAlert("Sửa môn thi thành công", "success");
                    return RedirectToAction("Index", "Object");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ObjectDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "EDIT_SUBJECT")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new ObjectDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
