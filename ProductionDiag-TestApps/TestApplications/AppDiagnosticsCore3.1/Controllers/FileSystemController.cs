using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppSharedCore.FileSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppDiagnostics.Controllers
{
    public class FileSystemController : Controller
    {
        private readonly ILogger<FileSystemController> _logger;

        public FileSystemController(ILogger<FileSystemController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dir(string directory)
        {
            var model = FileSystemEntryModel.CreateFromFolder(directory);
            return View("FileSystem", model);
        }

        public IActionResult File(string file)
        {
            try
            {
                ViewData["Contents"] = System.IO.File.ReadAllText(file);
            }
            catch (Exception e)
            {
                ViewData["Contents"] = e.ToString();
            }

            return View("File");
        }
    }
}