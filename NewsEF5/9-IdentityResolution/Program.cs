using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityResolution
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();
            Setup(db);

            //Query AsNoTracking
            //var test1 = db.Students.Include(p => p.Course).AsNoTracking().ToList();
            var test1 = db.Students.Include(p => p.Course).AsNoTrackingWithIdentityResolution().ToList();
        }

        private static void Setup(SampleContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //context.Add(new Course
            //{
            //    Description = "Teste 1",
            //    Teacher = "Rafael Almeida",
            //    Students = Enumerable.Range(1, 10)
            //        .Select(p => new Student { Name = $"Aluno {p}" })
            //        .ToArray()
            //});

            //context.SaveChanges();
        }
    }

    public class Course
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Teacher { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Course Course { get; set; }
    }
    
    public class SampleContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=IdentityResolution;Integrated Security=true")
                .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
