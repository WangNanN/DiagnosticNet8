using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDiagnostics.Models;
using AppSharedCore.EntityFramework;

namespace AppDiagnostics.Controllers
{
    public class AsyncTestController : Controller
    {
        private readonly ILogger<AsyncTestController> _logger;

        public AsyncTestController(ILogger<AsyncTestController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> AsyncTest()
        {
            ViewBag.Message = "Your async test page.";

            _logger.LogInformation("Test async message");

            var values = await DownloadWebAsync(100);
            Main_orig(values);

            return View();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        private async Task<float[]> DownloadWebAsync(int arrayLength)
        {
            var values = await WaitArrayData(arrayLength);

            await Task.Delay(2000);

            return values;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        private async Task<float[]> WaitArrayData(int arrayLength)
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                float[] values = Enumerable.Range(0, arrayLength).Select(i => (float)i / 10).ToArray();
                return values;
            });

            return await task1;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoOptimization)]
        public static int Main_orig(float[] values)
        {
            int valuesLength = values.Length;

            return valuesLength;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
