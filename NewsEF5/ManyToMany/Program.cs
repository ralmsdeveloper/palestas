using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ManyToMany
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SampleManyToManyContext();
            var script = db.Database.GenerateCreateScript();
             
            Console.WriteLine("Hello World!");
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public IList<CourseStudent> CourseStudents { get; } = new List<CourseStudent>();
       public IList<Course> Courses { get; } = new List<Course>();
    }

    public class Course
    {
        public int Id { get; set; }
        public string Description { get; set; }

        //public IList<CourseStudent> CourseStudents { get; } = new List<CourseStudent>();
        public IList<Student> Students { get; } = new List<Student>();
    }

    //public class CourseStudent
    //{
    //    public int CourseId { get; set; }
    //    public Course Course { get; set; }
    //    public int StudentId { get; set; }
    //    public Student Student { get; set; }
    //}

    public class SampleManyToManyContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CourseStudent>()
            //    .HasKey(p => new { p.CourseId, p.StudentId });

            //modelBuilder.Entity<CourseStudent>()
            //    .HasOne(p => p.Student)
            //    .WithMany(p => p.CourseStudents)
            //    .HasForeignKey(p => p.StudentId);

            //modelBuilder.Entity<CourseStudent>()
            //    .HasOne(p => p.Course)
            //    .WithMany(p => p.CourseStudents)
            //    .HasForeignKey(p => p.CourseId);

            //// Opcao 1
            //modelBuilder.Entity<Course>()
            //   .HasMany(p => p.Students)
            //   .WithMany(p => p.Courses);

            // Opcao 2
            //modelBuilder.Entity<Course>()
            //   .HasMany(p => p.Students)
            //   .WithMany(p => p.Courses)
            //   .UsingEntity<CourseStudent>(
            //       "CourseStudents",
            //       e => e.HasOne<Student>().WithMany(),
            //       e => e.HasOne<Course>().WithMany()
            //   ).HasKey(p => new { p.CourseId, p.StudentId });

            //// Opcao 3
            //modelBuilder.Entity<Course>()
            //   .HasMany(p => p.Students)
            //   .WithMany(p => p.Courses)
            //   .UsingEntity<Dictionary<string, object>>(
            //       "CourseStudents",
            //       e => e.HasOne<Student>().WithMany(),
            //       e => e.HasOne<Course>().WithMany()
            //   );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=M2MEF5;Integrated Security=true");
    }
}
