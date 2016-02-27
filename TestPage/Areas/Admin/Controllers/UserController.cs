using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;
using PagedList;

namespace TestPage.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Admin/User/
       //   [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page=1, int pageSize=10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

          [HasCredential(RoleID = "VIEW_USER")]
          public ActionResult Active(string searchString, int page = 1, int pageSize = 10)
          {
              var dao = new UserDao();
              var model = dao.ListAllPagingActive(searchString, page, pageSize);
              ViewBag.SearchString = searchString;
              return View(model);
          }

          [HasCredential(RoleID = "ADD_USER")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
         [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetails(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(TbStudent student)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryp = Encryptor.MD5Hash(student.Password);
                student.Password = encryp;
                int id = dao.Insert(student);
                if (id > 0)
                {
                    SetAlert("Thêm người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("","Thêm mới không thành công");
                }
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(TbStudent student)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if(!string.IsNullOrEmpty(student.Password))
                {
                    var encryp = Encryptor.MD5Hash(student.Password);
                    student.Password = encryp;
                }
        

                var result = dao.Update(student);
                if (result)
                {
                    SetAlert("Sửa thông tin người dùng thành công", "success");
                    return RedirectToAction("Index", "User");
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
         
            var delete=new UserDao().Delete(id);
            if(delete==true)
            {
                SetAlert("Xóa người dùng thành công", "success");
                return RedirectToAction("Annoucement");
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật không thành công");
            }
            return RedirectToAction("Index");
               
        }

        public ActionResult Annoucement()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
                {
                    status= result
                });
        }

        [HttpPost]
        public JsonResult ChangeStatusThi(int id)
        {
            var result = new UserDao().ChangeStatusThi(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
