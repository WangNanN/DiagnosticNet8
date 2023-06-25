using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDiagnostics.Models;
using AppSharedCore.EntityFramework;
using System.Dynamic;
using System.Threading;
using AppCommon.Evaluation;

namespace AppDiagnosticsNet.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly ILogger<EvaluationController> _logger;

        public EvaluationController(ILogger<EvaluationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Evaluation()
        {
            _logger.LogInformation("Test evaluation message");

            int a = 1;
            VerifyBreakpoint();
            VerifyEE();
            VerifyCallStack();
            VerifyRecursion(10);

            try
            {
                string s1 = String.Format("{1}", "ab12");
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

            if (a == 10)
            {
                VerifyException();
            }

            return View();
        }

        private void VerifyBreakpoint()
        {
            int iGlobal = 0;
            for (int i = 0; i < 50; i++)
            {
                iGlobal++; //set bp #1
                i++;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        private int VerifyEE()
        {
            List<string> fruits = new List<string> { "apple", "passionfruit", "banana", "mango", "orange", "blueberry", "grape", "strawberry" };
            IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);
            foreach (string fruit in query)
            {
                Console.WriteLine(fruit);
            }

            object[] pList = new object[] { 1, "one", 2, "two", 3, "three" };
            var query1 = pList.OfType<string>();
            dynamic expObj = new ExpandoObject();
            expObj.FirstName = "Daffy";
            expObj.LastName = "Duck";

            Dictionary<string, string> mygroup = new Dictionary<string, string>() { { "Hannah","Zhang"},{ "Alex","Yao"},{ "Alisa","Zhang"}
                ,{ "Nelson","Yan"},{ "Richard","Zeng"},{ "Clarie","Kang"},{ "Qian","Wang"},{ "Serena","Wang"},{ "Maggie","Zhang"},{ "Cherry","Wu"}
                ,{ "Lynn","Zhang"},{ "Grace","Dong"} };

            return 0; //set bp #2
        }

        private int VerifyCallStack()
        {
            Worker e1 = new Worker(30000);
            Worker e2 = new Worker(500, 52);
            HourlyWorker e3 = new HourlyWorker(10000);

            return 0; //set bp #3
        }

        private void VerifyException()
        {
            while (true)
            {
                Thread.Sleep(100);

                try
                {
                    throw new InvalidOperationException();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private int VerifyRecursion(int m)
        {
            if (m <= 1)
                return 1;
            return m * VerifyRecursion(m - 1);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
