using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SaveChangesInterceptionAndEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();
            db.Database.EnsureCreated();


            db.SavingChanges += (sender, args) =>
            {
                var contextId = ((DbContext)sender).ContextId;
                Console.WriteLine($"ContextId: {contextId}");
            };

            db.SavedChanges += (sender, args) =>
            {
                var totalSaved = args.EntitiesSavedCount;
                Console.WriteLine($"Total Saved: {totalSaved}");
            };
           

            db.Courses.Add(new Course { Description = "Test", Teacher = "Rafael" });
            db.SaveChanges();
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
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SaveChangesInterceptionAndEvents;Integrated Security=true")
                .AddInterceptors(new CustomSaveChangesInterceptor());
    }


    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var contextId = eventData.Context.ContextId;
            Console.WriteLine($"ContextId: {contextId}");

            return result;
        }
    }
}
