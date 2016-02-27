namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Model.Dao;

    public partial class TestPageDbContext : DbContext
    {
        public TestPageDbContext()
            : base("name=TestPageDbContext")
        {
        }

        public virtual DbSet<TbBaiThi> TbBaiThis { get; set; }
        public virtual DbSet<TbCapdo> TbCapdoes { get; set; }
        public virtual DbSet<TbDeThi> TbDeThis { get; set; }
        public virtual DbSet<TbDotthi> TbDotthis { get; set; }
        public virtual DbSet<TbMT> TbMTs { get; set; }
        public virtual DbSet<TbStudent> TbStudents { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
