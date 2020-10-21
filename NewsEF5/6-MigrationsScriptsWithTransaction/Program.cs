using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsScriptsWithTransaction
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new SampleContext();

            var migrator = db.Database.GetService<IMigrator>();
            var script = migrator.GenerateScript(options: MigrationsSqlGenerationOptions.Script);
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
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=ExcludeTablesFromMigrations;Integrated Security=true");
    }
}
