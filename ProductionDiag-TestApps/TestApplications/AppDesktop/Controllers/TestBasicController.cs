using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace AppDesktop.Controllers
{
    public class TestBasicController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page."; // #bp1
            string aboutMessage = ViewBag.Message;
            int firstValue = 12;
            int secondValue = 13;
            for (int i = 0; i < 100; i++)
            {
                aboutMessage += i.ToString();// #bp3
            }
            int result = GetResult(firstValue, secondValue); // #bp4
            string headText = string.Join("#", result.ToString(), "Your application description page.");
            ViewBag.Message = headText; // #bp8

            return View();
        }

        private int GetResult(int firstValue, int secondValue)
        {
            int lastValue = 100;
            firstValue += secondValue;
            secondValue = firstValue * 2; // #bp5
            lastValue *= 2;
            return firstValue + secondValue + lastValue;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page."; // #bp2
            string contactMessage = ViewBag.Message;
            int a = 10;
            int b = 20;
            int c = 0;
            for (int i = 0; i < 100; i++)
            {
                c = i;
                contactMessage += i.ToString(); // #bp6
                c += 2;
            }
            string message = string.Join("#", c.ToString(), "Your contact page.");
            ViewBag.Message = message; // #bp7

            try
            {
                var d = a / 0;
            }
            catch (Exception ex)
            {
                string info = ex.ToString();
            }

            return View();
        }
    }
}
