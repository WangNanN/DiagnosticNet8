using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppDesktop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            System.Diagnostics.Trace.WriteLine("Testing trace message");
            System.Diagnostics.Trace.TraceWarning("Transient warning on the Index page at " + DateTime.Now.ToShortTimeString());
            System.Diagnostics.Trace.Flush();
            ViewBag.Message = "Your index page.";
            System.Diagnostics.Trace.WriteLine("Leaving index method");

            return View();
        }
    }
}