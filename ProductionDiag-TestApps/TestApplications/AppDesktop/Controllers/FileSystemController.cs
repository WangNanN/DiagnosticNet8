using System;
using System.Web.Mvc;
using AppSharedCore.FileSystem;

namespace AppDesktop.Controllers
{
    public class FileSystemController : Controller
    {
        public ActionResult Dir(string directory)
        {
            var model = FileSystemEntryModel.CreateFromFolder(directory);
            return View("FileSystem", model);
        }

        public ActionResult File(string file)
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