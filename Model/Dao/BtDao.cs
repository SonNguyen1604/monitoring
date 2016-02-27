using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class BtDao
    {
        TestPageDbContext db = null;

        public BtDao()
        {
            db = new TestPageDbContext();
        }

        public int Insert(TbBaiThi entity)
        {
            db.TbBaiThis.Add(entity);
            db.SaveChanges();
            return entity.MaBaiThi;
        }

        public TbBaiThi GetByID(int idBaiThi)
        {
            return db.TbBaiThis.SingleOrDefault(x => x.MaBaiThi == idBaiThi);
        }

        public List<TbDeThi> ListAll()
        {
            return db.TbDeThis.Where(x => x.TrangThai == true).ToList();
        }


        public IEnumerable<TbBaiThi> GetID(int ID)
        {
            var bt = from a in db.TbBaiThis
                     where a.ID == ID
                     orderby a.MaBaiThi descending
                     select a;
           return bt;
        }

        public IEnumerable<TbBaiThi> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TbBaiThi> model = db.TbBaiThis;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaBaiThi.ToString().Contains(searchString) || x.ID.ToString().Contains(searchString) || x.MaDeThi.ToString().Contains(searchString));
            }

            return model.OrderByDescending(x => x.MaBaiThi).ToPagedList(page, pageSize);
        }

        public bool Update(int id, string wc, string mh, string at)
        {
            try
            {
                var baithi = db.TbBaiThis.SingleOrDefault(x => x.MaBaiThi == id);
                baithi.DuongDanWC = wc;
                baithi.DuongDanMH = mh;
                baithi.DuongDanAT = at;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBT(int id, string bt)
        {
            try
            {
                var baithi = db.TbBaiThis.SingleOrDefault(x => x.MaBaiThi == id);
                baithi.DuongDanBT = bt;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var dt = db.TbBaiThis.SingleOrDefault(x => x.MaBaiThi == id);
                db.TbBaiThis.Remove(dt);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
