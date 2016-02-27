using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class UserDao
    {
        TestPageDbContext db = null;
        public UserDao()
        {
            db= new TestPageDbContext();
        }
        public int Insert(TbStudent entity)
        {
            db.TbStudents.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(TbStudent entity)
        {
            try
            {
                var user = db.TbStudents.SingleOrDefault(x=>x.ID==entity.ID);
                if(!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Ho = entity.Ho;
                user.Ten = entity.Ten;
                user.NgaySinh = entity.NgaySinh;
                user.GioiTinh = entity.GioiTinh;
                user.Active = entity.Active;
                user.Thi = entity.Thi;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
          
        }

        public bool UpdateThi(int ID)
        {
            try
            {
                var user = db.TbStudents.SingleOrDefault(x => x.ID == ID);
                user.Thi = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<TbStudent>ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<TbStudent> model = db.TbStudents;
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Ho.Contains(searchString) || x.Ten.Contains(searchString));
            }
            return model.OrderByDescending(x=>x.Username).ToPagedList(page,pageSize);
        }

        public IEnumerable<TbStudent> ListAllPagingActive(string searchString, int page, int pageSize)
        {
            IQueryable<TbStudent> model = db.TbStudents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Ho.Contains(searchString) || x.Ten.Contains(searchString));
            }
            return model.Where(x=>x.Active==true).OrderByDescending(x => x.Username).ToPagedList(page, pageSize);
        }

        public TbStudent GetByID(string username)
        {
            return db.TbStudents.SingleOrDefault(x=>x.Username==username);
        }

        public TbStudent ViewDetails(int id)
        {    
                return db.TbStudents.SingleOrDefault(x=>x.ID==id);          
        }

        public List<string> GetListCredential(string userName)
        {
            var user = db.TbStudents.Single(x => x.Username == userName);
            var data = (from a in db.Credentials
                       join b in db.UserGroups on a.UserGroupID equals b.ID
                       join c in db.Roles on a.RoleID equals c.ID
                       where b.ID==user.GroupID
                       select new 
                       {
                           RoleID = a.RoleID,
                           UserGroupID = a.UserGroupID
                       }).AsEnumerable().Select(x=> new Credential()
                       {
                           RoleID= x.RoleID,
                           UserGroupID = x.UserGroupID
                       });
            return data.Select(x=>x.RoleID).ToList();
        }

        public int Login(string username, string password, bool isAdminLogin=false)
        {
            var result = db.TbStudents.SingleOrDefault(x => x.Username == username);
            if(result==null)
            {
                return 0;
            }
            else
            {
                if(isAdminLogin==true)
                {
                    if(result.GroupID==CommonConstants.ADMIN_GROUP || result.GroupID==CommonConstants.MOD_GROUP)
                    {
                        if (result.Active == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == password)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Active == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == password)
                            return 1;
                        else
                            return -2;
                    }

                }
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var user = db.TbStudents.SingleOrDefault(x=>x.ID==id);
                db.TbStudents.Remove(user);
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
            var user = db.TbStudents.SingleOrDefault(x => x.ID==id);
            user.Active = !user.Active;
            db.SaveChanges();
            return !user.Active;
        }

        public bool ChangeStatusThi(int id)
        {
            var user = db.TbStudents.SingleOrDefault(x => x.ID == id);
            user.Thi = !user.Thi;
            db.SaveChanges();
            return !user.Thi;
        }

        public bool CheckUserName(string userName)
        {
            return db.TbStudents.Count(x => x.Username == userName) > 0;
        }
    }
}
