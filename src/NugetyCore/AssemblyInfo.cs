using System.Reflection;
using System.Runtime.Loader;

namespace NugetyCore
{
    public class AssemblyInfo
    {
        public AssemblyInfo(Assembly assembly)
        {
            this.Assembly = assembly;
            this.Location = this.Assembly.Location;
        }

        public Assembly Assembly { get; set; }
        public string Location { get; set; }

        public AssemblyLoadContext Context
        {
            get
            {
                return AssemblyLoadContext.GetLoadContext(this.Assembly);
            }
        }
    }
}
