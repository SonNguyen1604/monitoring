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
    public class DtController : BaseController
    {
        //
        // GET: /Admin/Dt/
          [HasCredential(RoleID = "VIEW_DT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DtDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

          [HasCredential(RoleID = "VIEW_DT")]
          public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
          {
              var dao = new DtDao();
              var model = dao.ListAllPagingActive(searchString, page, pageSize);
              ViewBag.SearchString = searchString;
              return View(model);
          }

         [HasCredential(RoleID = "ADD_DT")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

         [HasCredential(RoleID = "EDIT_DT")]
        public ActionResult Edit(int id)
        {
            var dt = new DtDao().GetByID(id);
            return View(dt);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new DtDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(TbDotthi dt)
        {
            var dao = new DtDao();
            var id = dao.Insert(dt);
            if (id > 0)
            {
                SetAlert("Thêm đợt thi mới thành công", "success");
                return RedirectToAction("Index", "Dt");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới không thành công");
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(TbDotthi dt)
        {
            if (ModelState.IsValid)
            {
                var dao = new DtDao();
                var result = dao.Update(dt);
                if (result)
                {
                    SetAlert("Sửa đợt thi thành công", "success");
                    return RedirectToAction("Index", "Dt");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View("Index");
        }

         [HasCredential(RoleID = "EDIT_DT")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new DtDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

    }
}
