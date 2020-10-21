using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MapEntityTypesToQueries
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();
            var query = db.Courses.Where(p => p.Description == "EF Core");
            
            var sql = query.ToQueryString();
            /*
            SELECT[c].[Id], [c].[Description], [c].[Teacher]
            FROM(
                SELECT Id, Description, Teacher FROM Courses
                UNION ALL
                SELECT Id, Description, "Undefined" from Courses_Old
            ) AS[c]
            WHERE[c].[Description] = N'EF Core'
            */

            foreach (var item in query)
            {
                Console.WriteLine($"{item.Id} - {item.Description} - {item.Teacher}");
            }
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
            modelBuilder.Entity<Course>()
                .ToSqlQuery(
                    @"SELECT Id, Description, Teacher FROM Courses
                      UNION ALL
                      SELECT Id, Description, ""Undefined"" from Courses_Old");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=MapEntityTypesToQueries;Integrated Security=true");
    }
}
