using System;
using System.ComponentModel;
using System.Reflection;

namespace Nugety
{
    public class ModuleIntanceEventArgs : EventArgs
    {
        public ModuleIntanceEventArgs(ModuleInfo module, object value)
        {
            this.Module = module;
            this.Value = value;
        }

        public ModuleInfo Module { get; private set; }

        public object Value { get; set; }
    }

    public class ModuleCancelEventArgs : CancelEventArgs
    {
        public ModuleCancelEventArgs(ModuleInfo module)
        {
            this.Module = module;
        }

        public ModuleInfo Module { get; private set; }
    }

    public class AssemblyResolveCancelEventArgs : CancelEventArgs
    {
        public AssemblyResolveCancelEventArgs(AssemblyName name)
        {
            this.Name = name;
        }

        public AssemblyName Name { get; private set; }

        public Assembly Assembly { get; set; }
    }

    public class AssemblyResolvedEventArgs : EventArgs
    {
        public AssemblyResolvedEventArgs(AssemblyName name, ModuleInfo module, AssemblyInfo info)
        {
            this.Name = name;
            this.Module = module;
            this.Info = info;
        }

        public AssemblyName Name { get; private set; }

        public ModuleInfo Module { get; private set; }

        public AssemblyInfo Info { get; private set; }
    }
}