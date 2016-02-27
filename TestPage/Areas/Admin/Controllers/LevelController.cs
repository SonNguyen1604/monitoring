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
    public class LevelController : BaseController
    {
        //
        // GET: /Admin/Level/
        [HasCredential(RoleID = "VIEW_LV")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new LevelDao();
            var model = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HasCredential(RoleID = "VIEW_LV")]
        public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new LevelDao();
            var model = dao.ListAllPagingActive(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HasCredential(RoleID = "ADD_LV")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HasCredential(RoleID = "EDIT_LV")]
        public ActionResult Edit(int id)
        {
            var level = new LevelDao().GetByID(id);
            return View(level);
        }

        [HttpPost]
        public ActionResult Create(TbCapdo level)
        {
            var dao = new LevelDao();
            var id = dao.Insert(level);
            if(id>0)
            {
                SetAlert("Thêm cấp độ mới thành công", "success");
                return RedirectToAction("Index", "Level");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới không thành công");
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(TbCapdo level)
        {
            if (ModelState.IsValid)
            {
                var dao = new LevelDao();
                var result = dao.Update(level);
                if (result)
                {
                    SetAlert("Sửa cấp độ thành công", "success");
                    return RedirectToAction("Index", "Level");
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
            new LevelDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HasCredential(RoleID = "EDIT_LV")]
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new LevelDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

    }
}
