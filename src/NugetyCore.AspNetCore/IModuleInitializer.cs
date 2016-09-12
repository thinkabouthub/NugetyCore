using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nugety
{
    public interface IModuleInitializer
    {
        bool ConfigureServices(IServiceCollection services, IMvcBuilder builder = null, IServiceProvider provider = null);

        bool Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);
    }
}