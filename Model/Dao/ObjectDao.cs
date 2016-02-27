using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace Model.Dao
{
    public class ObjectDao
    {
        TestPageDbContext db = null;

        public ObjectDao()
        {
            db = new TestPageDbContext();
        }

        public int Insert(TbMT entity)
        {
            db.TbMTs.Add(entity);
            db.SaveChanges();
            return entity.MaMT;
        }

        public TbMT GetByID(int idMT)
        {
            return db.TbMTs.SingleOrDefault(x => x.MaMT == idMT);
        }

        public List<TbMT> ListAll()
        {
            return db.TbMTs.Where(x => x.TrangThai == true).ToList();
        }

        public IEnumerable<TbMT>ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TbMT> model = db.TbMTs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x=> x.MaMT.ToString().Contains(searchString)|| x.TenMT.Contains(searchString));
            }
            return model.OrderByDescending(x => x.MaMT).ToPagedList(page, pageSize);
        }

        public IEnumerable<TbMT> ListAllPagingActive(string searchString, int page, int pageSize)
        {
            IQueryable<TbMT> model = db.TbMTs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaMT.ToString().Contains(searchString) || x.TenMT.Contains(searchString));
            }
            return model.Where(x=>x.TrangThai==true).OrderByDescending(x => x.MaMT).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var Object = db.TbMTs.SingleOrDefault(x => x.MaMT == id);
                db.TbMTs.Remove(Object);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(TbMT entity)
        {
            try
            {
                var Object = db.TbMTs.SingleOrDefault(x => x.MaMT == entity.MaMT);
                Object.TenMT = entity.TenMT;
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public bool ChangeStatus(int id)
        {
            var Object = db.TbMTs.SingleOrDefault(x => x.MaMT == id);
            Object.TrangThai = !Object.TrangThai;
            db.SaveChanges();
            return !Object.TrangThai;
        }
    }
}
