using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSharedDesktop.AppDomain
{
    public static class AppDomainExecutor
    {
        public static async Task<AppDomainModel> CreateAndExecuteDomain(string name)
        {
            return await Task.Run(() =>
            {
                System.AppDomain domain = null;
                try
                {
                    AppDomainSetup setup = new AppDomainSetup();

                    setup.ApplicationBase = Path.GetDirectoryName(new Uri(typeof(AppDomainExecutor).Assembly.CodeBase).LocalPath);
                    domain = System.AppDomain.CreateDomain(name, null, setup);

                    AppDomainRemoteObject remote = (AppDomainRemoteObject)domain.CreateInstanceAndUnwrap(typeof(AppDomainRemoteObject).Assembly.FullName, typeof(AppDomainRemoteObject).FullName);
                    return remote.GetModel();
                }
                finally
                {
                    if (domain != null)
                    {
                        System.AppDomain.Unload(domain);
                    }
                }
            });
        }
    }
}
