using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace AppDesktop.Controllers
{
    public class TestLogpointController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static void Func()
        {   // first line of Func().
            int dummy = 0;
            int iLocal = 0;	// init iLocal in Func().
            iLocal++;

            string fourtype = "㐀㒣㕴㕵㙉㙊䵯䵰䶴䶵"; //#bp3
            string s1 = "hello"; //#bp4
            string s2 = "World"; //#bp6
        }

        public static int recursive(int m)
        {
            if (m <= 1)
                return 1;
            return m * recursive(m - 1);
        }

        public static void Linqexpression()
        {
            int[] array = { 0, 5, 2, 10, 8 };

            var results = from x in array
                          where x % 2 == 1
                          orderby x descending
                          select x * x;
            foreach (var result in results)
            {
                int i = 1;//#bp7
                i++;//#bp8
            }
        }

        public ActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            for (int i = 1; i < 100; i++)
            {
                int j = i; //#bp1
                j++;
            }
            List<string> list = new List<string>() { "wang", "zhao", "sun", "tian" };
            foreach (string s2 in list)
            {
                Console.WriteLine(s2);//#bp2
            }
            Func();//#bp5
            recursive(4);
            Linqexpression();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }
    }
}
