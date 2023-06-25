using AppSharedDesktop.AppDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppDesktop.Controllers
{
    public class AppDomainController : Controller
    {
        public async Task<ActionResult> CreateDomain()
        {
            AppDomainModel model = new AppDomainModel("Aggregate");
            List<Task<AppDomainModel>> allTasks = new List<Task<AppDomainModel>>();

            for (int i = 0; i < 20; i++)
            {
                allTasks.Add(AppDomainExecutor.CreateAndExecuteDomain($"test{i}"));
            }

            await Task.WhenAll(allTasks);

            foreach(var result in allTasks)
            {
                foreach(var assembly in (await result).AssemblyNames)
                {
                    model.AssemblyNames.Add(assembly);
                }
            }
            
            return View(model);
        }
    }
}