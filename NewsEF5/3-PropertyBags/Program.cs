using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PropertyBags
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var courses = new Dictionary<string, object>
            {
                ["Description"] = "EF Core 3",
                ["Description"] = "EF Core 5"
            };

            db.Courses.Add(courses);

            db.SaveChanges();


           var coursesList = db.Courses.Where(p => p["Description"] == "Teste").ToArray();
        }
    }

    public class SampleContext : DbContext
    {
        public DbSet<Dictionary<string, object>> Courses => Set<Dictionary<string, object>>("Course");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Course", b =>
            {
                b.IndexerProperty<int>("Id");
                b.IndexerProperty<string>("Description");              
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=PropertyBags;Integrated Security=true");
    }
}
