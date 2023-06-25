using AppCommon.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSharedDesktop.EntityFramework
{
    public class DataInitializer : CreateDatabaseIfNotExists<EmployeeContext>
    {
        protected override void Seed(EmployeeContext context)
        {
            context.Employees.AddRange(Employee.GenerateNew(5, 5));
            base.Seed(context);
        }
    }
}
