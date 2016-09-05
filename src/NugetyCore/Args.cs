using System;
using System.ComponentModel;

namespace Nugety
{
    public class ModuleIntanceEventArgs : EventArgs
    {
        public ModuleIntanceEventArgs(ModuleInfo module, object value)
        {
            Module = module;
            Value = value;
        }

        public ModuleInfo Module { get; set; }

        public object Value { get; set; }
    }

    public class CancelModuleEventArgs : CancelEventArgs
    {
        public CancelModuleEventArgs(ModuleInfo module)
        {
            Module = module;
        }

        public ModuleInfo Module { get; set; }
    }
}