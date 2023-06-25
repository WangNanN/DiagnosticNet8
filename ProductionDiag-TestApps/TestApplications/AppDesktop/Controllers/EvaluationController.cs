using AppCommon.Evaluation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AppDesktop.Controllers
{
    public class EvaluationController : Controller
    {
        // GET: Evaluation
        public ActionResult Evaluation()
        {
            ViewBag.Message = "Welcome to evaluation page.";

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
    }
}