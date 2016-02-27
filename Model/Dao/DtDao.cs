using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class DtDao
    {
        TestPageDbContext db = null;

          public DtDao()
        {
            db = new TestPageDbContext();
        }

          public int Insert(TbDotthi entity)
          {
              db.TbDotthis.Add(entity);
              db.SaveChanges();
              return entity.MaDT;
          }

          public bool Delete(int id)
          {
              try
              {
                  var dt = db.TbDotthis.SingleOrDefault(x => x.MaDT == id);
                  db.TbDotthis.Remove(dt);
                  db.SaveChanges();
                  return true;
              }
              catch (Exception)
              {
                  return false;
              }
          }

          public TbDotthi GetByID(int idDotthi)
          {
              return db.TbDotthis.SingleOrDefault(x => x.MaDT == idDotthi);
          }

          public List<TbDotthi> ListAll()
          {
              return db.TbDotthis.Where(x => x.TrangThai == true).ToList();
          }

          public IEnumerable<TbDotthi> ListAllPaging(string searchString, int page, int pageSize)
          {
              IQueryable<TbDotthi> model = db.TbDotthis;
              if (!string.IsNullOrEmpty(searchString))
              {
                  model = model.Where(x => x.MaDT.ToString().Contains(searchString) || x.TenDT.Contains(searchString));
              }

              return model.OrderByDescending(x => x.MaDT).ToPagedList(page, pageSize);
          }

          public IEnumerable<TbDotthi> ListAllPagingActive(string searchString, int page, int pageSize)
          {
              IQueryable<TbDotthi> model = db.TbDotthis;
              if (!string.IsNullOrEmpty(searchString))
              {
                  model = model.Where(x => x.MaDT.ToString().Contains(searchString) || x.TenDT.Contains(searchString));
              }

              return model.Where(x=>x.TrangThai==true).OrderByDescending(x => x.MaDT).ToPagedList(page, pageSize);
          }

          public bool Update(TbDotthi entity)
          {
              try
              {
                  var dotthi = db.TbDotthis.SingleOrDefault(x => x.MaDT == entity.MaDT);
                  dotthi.MaDT = entity.MaDT;
                  dotthi.TenDT = entity.TenDT;
                  dotthi.TrangThai = entity.TrangThai;
                  db.SaveChanges();
                  return true;
              }
              catch (Exception)
              {
                  return false;
              }
          }

          public bool ChangeStatus(int id)
          {
              var dt = db.TbDotthis.SingleOrDefault(x => x.MaDT == id);
              dt.TrangThai = !dt.TrangThai;
              db.SaveChanges();
              return !dt.TrangThai;
          }
    }
}
