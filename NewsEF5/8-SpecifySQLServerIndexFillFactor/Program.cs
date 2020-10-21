using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SpecifySQLServerIndexFillFactor
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>(p =>
                {
                    p.ToTable("Courses");

                    p.HasIndex(p => p.Description).HasFillFactor(80);
                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SpecifySQLServerIndexFillFactor;Integrated Security=true")
                .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
