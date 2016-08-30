using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace NugetyCore
{
    public interface IModuleInitializer
    {
        bool ConfigureServices(IServiceCollection services, IMvcBuilder builder = null, IServiceProvider provider = null);

        bool Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);
    }
}
