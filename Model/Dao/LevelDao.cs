using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class LevelDao
    {
        TestPageDbContext db = null;

        public LevelDao()
        {
            db = new TestPageDbContext();
        }

        public int Insert(TbCapdo entity)
        {
            db.TbCapdoes.Add(entity);
            db.SaveChanges();
            return entity.MaCD;
        }

        public bool Delete(int id)
        {
            try
            {
                var level = db.TbCapdoes.SingleOrDefault(x => x.MaCD == id);
                db.TbCapdoes.Remove(level);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public TbCapdo GetByID(int idCapdo)
        {
            return db.TbCapdoes.SingleOrDefault(x => x.MaCD == idCapdo);
        }

        public List<TbCapdo>ListAll()
        {
            return db.TbCapdoes.Where(x => x.TrangThai == true).ToList();
        }

        public IEnumerable<TbCapdo> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TbCapdo> model = db.TbCapdoes;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaCD.ToString().Contains(searchString) || x.TenCD.Contains(searchString));
            }
            
            return model.OrderByDescending(x => x.MaCD).ToPagedList(page, pageSize);
        }

        public IEnumerable<TbCapdo> ListAllPagingActive(string searchString, int page, int pageSize)
        {
            IQueryable<TbCapdo> model = db.TbCapdoes;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaCD.ToString().Contains(searchString) || x.TenCD.Contains(searchString));
            }

            return model.Where(x=>x.TrangThai==true).OrderByDescending(x => x.MaCD).ToPagedList(page, pageSize);
        }

      

        public bool Update(TbCapdo entity)
        {
            try
            {
                var level = db.TbCapdoes.SingleOrDefault(x => x.MaCD == entity.MaCD);
                level.TenCD = entity.TenCD;
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
            var level = db.TbCapdoes.SingleOrDefault(x => x.MaCD == id);
            level.TrangThai = !level.TrangThai;
            db.SaveChanges();
            return !level.TrangThai;
        }
    }

}
