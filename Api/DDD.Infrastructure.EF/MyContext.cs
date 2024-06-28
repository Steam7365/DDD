using DDD.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DDD.Infrastructure.EF
{
    public class MyContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //连接的数据库字符串
        //    string connStr = "Server=.;Database=EFCoreDemo1;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    optionsBuilder.UseSqlServer(connStr);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //从当前程序集中加载 实现了IEntityTypeConfiguration接口的实体配置类
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

    }
}
