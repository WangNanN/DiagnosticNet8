using AppSharedCore.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDiagnosticsNet.Controllers
{
    public class DatabaseController : Controller
    {
        private ILogger<DatabaseController> _logger;

        public DatabaseController(ILogger<DatabaseController> logger)
        {
            _logger = logger;
        }

        public IActionResult List()
        {
            using (var context = new EmployeeContext())
            {
                context.InitializeData();
                return View("Database", context.Employees.OrderBy(e => e.LastName).ToList());
            }
        }

    }
}
