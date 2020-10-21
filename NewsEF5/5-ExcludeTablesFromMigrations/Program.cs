using Microsoft.EntityFrameworkCore;

namespace ExcludeTablesFromMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContextExcludeMigrations();
            var script = db.Database.GenerateCreateScript();
        }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; }
    }

    public class SampleContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
    }

    public class SampleContextExcludeMigrations : DbContext
    {
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Courses", exclude => exclude.ExcludeFromMigrations());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=ExcludeTablesFromMigrations;Integrated Security=true");
    }
}
