using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EventCounters
{
    class Program
    {
        static void Main(string[] args)
        {
            var processId = Process.GetCurrentProcess().Id;
            Console.WriteLine($"Iniciando processo: {processId}");

            using var db = new SampleContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Console.WriteLine("Banco Criado..");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                db.Courses.Add(new Course { Description = "Test", Teacher = "Rafael" });
                db.SaveChanges();
                 
                var course = db.Courses.Find(1);

                var courses = db.Courses.AsNoTracking().Where(p => p.Id > 0).ToArray();

                Console.WriteLine($"Courses: {courses.Length}");
            }

            Console.WriteLine("Finalizado..");
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=EventCounters;Integrated Security=true");
    }
}
