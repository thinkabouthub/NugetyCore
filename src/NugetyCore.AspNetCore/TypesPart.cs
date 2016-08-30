using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace NugetyCore
{
    public class TypesPart : ApplicationPart, IApplicationPartTypeProvider
    {
        public TypesPart(params Type[] types)
        {
            Types = types.Select(t => t.GetTypeInfo());
        }

        public override string Name => string.Join(", ", Types.Select(t => t.FullName));
        public IEnumerable<TypeInfo> Types { get; }
    }
}