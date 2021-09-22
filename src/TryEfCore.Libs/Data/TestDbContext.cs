using Microsoft.EntityFrameworkCore;
using TryEfCore.Libs.Core.Demos;

namespace TryEfCore.Libs.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
        
        public  DbSet<Org> Orgs { get; set; }
        public  DbSet<User> Users { get; set; }
        public  DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            var org = builder.Entity<Org>();
            org.ToTable("Test_Simple_Org");
            var user = builder.Entity<User>();
            user.ToTable("Test_Simple_User");
            var course = builder.Entity<Course>();
            course.ToTable("Test_Simple_Course");
        }
    }
}
