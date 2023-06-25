using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSharedDesktop.AppDomain
{
    [Serializable]
    public class AppDomainModel
    {
        public string Name { get; }

        public AppDomainModel(string friendlyName)
        {
            Name = friendlyName;
        }

        public IList<string> AssemblyNames { get; } = new List<string>();
    }
}
