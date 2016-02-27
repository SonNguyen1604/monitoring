using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Model.ViewModels;
using System.IO;

namespace Model.Dao
{
    public class ExamDao
    {
        TestPageDbContext db = null;
        
        public ExamDao()
        {
            db = new TestPageDbContext();
        }

        public int Insert(TbDeThi entity)
        {
            db.TbDeThis.Add(entity);
            db.SaveChanges();
            return entity.MaDeThi;
        }


        public bool Delete(int id)
        {
            try
            {
                var exam = db.TbDeThis.SingleOrDefault(x => x.MaDeThi == id);
                db.TbDeThis.Remove(exam);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TbDeThi GetByID(int idDeThi)
        {
            return db.TbDeThis.SingleOrDefault(x => x.MaDeThi == idDeThi);
        }

        public List<TbDeThi> ListAll()
        {
            return db.TbDeThis.Where(x => x.TrangThai == true).ToList();
        }

        public int count()
        {
            return db.TbDeThis.Where(x=>x.TrangThai==true).Count();
        }

        public IEnumerable<ExamViewModel> GetDeThi( int page, int pageSize)
        {
            var dethi = from a in db.TbDeThis
                        join b in db.TbMTs on a.MaMT equals b.MaMT
                        join c in db.TbCapdoes on a.MaCD equals c.MaCD
                        join d in db.TbDotthis on a.MaDT equals d.MaDT
                        where (a.TrangThai==true)
                        select new ExamViewModel()
                        {
                           MaDeThi=a.MaDeThi,
                           TenDeThi = a.TenDeThi,
                           TenMT = b.TenMT,
                           Capdo = c.TenCD,
                           Dotthi = d.TenDT,
                           Duongdan= a.DuongDanDT
                        };
            return dethi.OrderByDescending(x=>x.TenMT).ToPagedList(page,pageSize);
        }

        public IEnumerable<ExamViewModel> GetDeThiAdmin(string searchString, int page, int pageSize)
        {
            var dethi = from a in db.TbDeThis
                        join b in db.TbMTs on a.MaMT equals b.MaMT
                        join c in db.TbCapdoes on a.MaCD equals c.MaCD
                        join d in db.TbDotthis on a.MaDT equals d.MaDT
                        select new ExamViewModel()
                        {
                            MaDeThi = a.MaDeThi,
                            TenDeThi = a.TenDeThi,
                            TenMT = b.TenMT,
                            Capdo = c.TenCD,
                            Dotthi = d.TenDT,
                            Duongdan = a.DuongDanDT,
                            TrangThai=a.TrangThai
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                dethi = dethi.Where(x => x.MaDeThi.ToString().Contains(searchString));
            }
            return dethi.OrderByDescending(x => x.MaDeThi).ToPagedList(page, pageSize);
        }

        public IEnumerable<ExamViewModel> GetDeThiAdminActive(string searchString, int page, int pageSize)
        {
            var dethi = from a in db.TbDeThis
                        join b in db.TbMTs on a.MaMT equals b.MaMT
                        join c in db.TbCapdoes on a.MaCD equals c.MaCD
                        join d in db.TbDotthis on a.MaDT equals d.MaDT
                        select new ExamViewModel()
                        {
                            MaDeThi = a.MaDeThi,
                            TenDeThi = a.TenDeThi,
                            TenMT = b.TenMT,
                            Capdo = c.TenCD,
                            Dotthi = d.TenDT,
                            Duongdan = a.DuongDanDT,
                            TrangThai = a.TrangThai
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                dethi = dethi.Where(x => x.MaDeThi.ToString().Contains(searchString));
            }
            return dethi.Where(x=>x.TrangThai==true).OrderByDescending(x => x.MaDeThi).ToPagedList(page, pageSize);
        }

        public IEnumerable<TbDeThi> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TbDeThi> model = db.TbDeThis;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaDeThi.ToString().Contains(searchString) || x.MaMT.ToString().Contains(searchString));
            }

            return model.OrderByDescending(x => x.MaDeThi).ToPagedList(page, pageSize);
        }

        public IEnumerable<TbDeThi> ListAllPage(int page, int pageSize)
        {
            IQueryable<TbDeThi> model = db.TbDeThis.Where(x=>x.TrangThai==true);
            return model.OrderByDescending(x => x.MaDeThi).ToPagedList(page, pageSize);
        }

        public bool Update(TbDeThi entity)
        {
            try
            {
                var dethi = db.TbDeThis.SingleOrDefault(x => x.MaDeThi == entity.MaDeThi);
                dethi.MaCD = entity.MaCD;
                dethi.MaMT = entity.MaMT;
                dethi.MaDT = entity.MaDT;
                dethi.DuongDanDT = entity.DuongDanDT;
                dethi.TrangThai = entity.TrangThai;
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
            var exam = db.TbDeThis.SingleOrDefault(x => x.MaDeThi == id);
            exam.TrangThai = !exam.TrangThai;
            db.SaveChanges();
            return !exam.TrangThai;
        }
    }
}
