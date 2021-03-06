﻿using System.Reflection;
using System.Runtime.Loader;

namespace Nugety
{
    public class AssemblyInfo
    {
        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
            Location = Assembly.Location;
        }

        public Assembly Assembly { get; set; }
        public string Location { get; set; }

        public AssemblyLoadContext Context
        {
            get { return AssemblyLoadContext.GetLoadContext(Assembly); }
        }
    }
}