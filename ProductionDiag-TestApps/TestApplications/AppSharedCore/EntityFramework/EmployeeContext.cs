using AppCommon.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AppSharedCore.EntityFramework
{

    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO Probably want to use Sql here once we have central server

            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
            builder.Mode = SqliteOpenMode.ReadWriteCreate;
            builder.DataSource = Path.GetTempFileName();
            optionsBuilder.UseSqlite(builder.ToString());

            base.OnConfiguring(optionsBuilder);
        }

        public void InitializeData()
        {
            Database.EnsureCreated();
            foreach(var e in AppCommon.EntityFramework.Employee.GenerateNew(count: 20, seed: DateTime.UtcNow.Millisecond))
            {
                Add(e);
            }
            SaveChanges();
        }
    }

}
