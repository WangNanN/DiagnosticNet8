using AppCommon.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSharedDesktop.EntityFramework
{
    public sealed class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext() : base()
        {
            Database.SetInitializer(new DataInitializer());
        }
    }
}
