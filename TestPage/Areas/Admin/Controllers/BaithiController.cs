using Model.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestPage.Common;

namespace TestPage.Areas.Admin.Controllers
{
    public class BaithiController : BaseController
    {
        //
        // GET: /Admin/Baithi/
         [HasCredential(RoleID = "VIEW_BT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var dao = new BtDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
          [HasCredential(RoleID = "DELETE_BT")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var dao= new BtDao();
          
            var name = dao.GetByID(id);
            string bt = Server.MapPath(name.DuongDanBT);
            string wc = Server.MapPath(name.DuongDanWC);
            string mh = Server.MapPath(name.DuongDanMH);
            string at = Server.MapPath(name.DuongDanAT);
            FileInfo filebt = new FileInfo(bt);
            FileInfo filewc = new FileInfo(wc);
            FileInfo filemh = new FileInfo(mh);
            FileInfo fileat = new FileInfo(at);
           if(filebt.Exists && filewc.Exists && fileat.Exists && filemh.Exists)
           {
                filebt.Delete();
                filewc.Delete();
                filemh.Delete();
                fileat.Delete();
           
                SetAlert("Xóa bài thi thành công", "success");
           }
        else
            {
                ModelState.AddModelError("", "Xóa bài thi không thành công");
            }
           dao.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
