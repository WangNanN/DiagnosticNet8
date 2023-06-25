using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTDTest_AspNetCore.Models;

namespace TTDTest_AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] cheesecakeNames = new string[]
            { 
                "Original",
                "Fresh Strawberry Cheesecake",
                "Pineapple Upside-Down Cheesecake",
                "OREO® Dream Extreme Cheesecake",
                "Very Cherry Ghirardelli® Chocolate Cheesecake",
                "Cinnabon® Cinnamon Swirl Cheesecake",
                "Reese's® Peanut Butter Chocolate Cake Cheesecake",
                "Celebration Cheesecake",
                "Chocolate Hazelnut Crunch Cheesecake",
                "Salted Caramel Cheesecake",
                "Toasted Marshmallow S'mores Galore™",
                "Lemon Meringue Cheesecake",
                "Adam's Peanut Butter Cup Fudge Ripple Cheesecake",
                "Godiva® Chocolate Cheesecake",
                "Ultimate Red Velvet Cake Cheesecake™",
                "Dulce de Leche Caramel Cheesecake",
                "White Chocolate Raspberry Truffle®",
                "Mango Key Lime Cheesecake",
                "Fresh Banana Cream Cheesecake",
                "White Chocolate Caramel Macadamia Nut Cheesecake",
                "Lemon Raspberry Cream Cheesecake",
                "Chocolate Mousse Cheesecake",
                "Chocolate Tuxedo Cream® Cheesecake",
                "Hershey's® Chocolate Bar Cheesecake",
                "30th Anniversary Chocolate Cake Cheesecake",
                "Vanilla Bean Cheesecake",
                "Tiramisu Cheesecake",
                "Key Lime Cheesecake",
                "Low Carb Cheesecake",
                "Low Carb Cheesecake with Strawberries",
                "Caramel Pecan Turtle Cheesecake",
                "Pumpkin Cheesecake",
                "Pumpkin Pecan Cheesecake",
                "Peppermint Bark Cheesecake",
            };

            IndexViewModel vm = new IndexViewModel { CheesecakeNames = cheesecakeNames };

            return View(vm);
        }

        public IActionResult HashBenchmark()
        {
            int[] byteCounts = new int[] { 256, 1024, 4 * 1024, 32 * 1024, 256 * 1024, 1024 * 1024, 16 * 1024 * 1024 };

            HashBenchmarkViewModel vm = new HashBenchmarkViewModel(byteCounts);

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
