using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FlexibleQueryUpdateMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();
            Setup(db);

            var course = db.Courses.FirstOrDefault(p => p.Id == 1);

            db.Courses.Add(new Course { Description = "Test", Teacher = "Rafael" });
            db.SaveChanges();

        }

        private static void Setup(SampleContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlRaw(
    @"CREATE or ALTER VIEW vw_Courses  
AS  
SELECT *  
FROM Courses");
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>()
                .ToTable("Courses")
                .ToView("vw_Courses");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=FlexibleQueryUpdateMapping;Integrated Security=true")
                .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
