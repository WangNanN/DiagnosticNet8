using System;
using AppDiagnostics.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AppDiagnostics.Controllers
{
    public class TestEEController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            Main_orig();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static int Main_orig()
        {
            float[] values = Enumerable.Range(0, 100).Select(i => (float)i / 10).ToArray(); // Lambda Expression

            Queue<int> queueOfInts = new Queue<int>(5);
            List<double> listOfDoubles = new List<double>(5);
            for (int i = 1; i <= 5; i++)
            {
                queueOfInts.Enqueue(i * 2);
                listOfDoubles.Add(i * 2.2);
            }

            Dictionary<int, string> dictStringToInt = new Dictionary<int, string>();
            dictStringToInt.Add(1, "One");
            dictStringToInt.Add(2, "Two");
            dictStringToInt.Add(3, "Three");
            dictStringToInt.Add(4, "Four");
            dictStringToInt.Add(5, "Five");

            double doubleVal = 2.5;
            float floatVal = 0.0F;
            int intVal = 4;
            long longVal = long.MinValue;
            uint uintVal = uint.MaxValue;
            ushort ushortVal = ushort.MaxValue;
            byte byteVal = byte.MaxValue;

            intVal = (int)doubleVal;

            return 0; // Begin inspection of SnapshotEE scenario
        }
    }
}
