using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSharedDesktop.AppDomain
{
    public sealed class AppDomainRemoteObject : MarshalByRefObject
    {
        public AppDomainModel GetModel()
        {
            AppDomainModel model = new AppDomainModel(System.AppDomain.CurrentDomain.FriendlyName);

            foreach(string assembly in System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName))
            {
                model.AssemblyNames.Add(model.Name + ":" + assembly);
            }

            return model;
        }
    }
}
