using System;
using System.Text;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core;
using BioEngine.Core.API;
using BioEngine.Core.Entities.Blocks;
using BioEngine.Core.Infra;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BioEngine.BRC.Api
{
    [UsedImplicitly]
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddBioEngineModule<CoreModule, CoreModuleConfig>(config =>
                {
                    config.Assemblies.Add(typeof(Developer).Assembly);
                    config.EnableValidation = true;
                    config.MigrationsAssembly = typeof(Developer).Assembly;
                })
                .AddBioEngineModule<ApiModule, ApiModuleConfig>(config =>
                {
                    config.Assemblies.Add(typeof(Startup).Assembly);
                })
                .AddBioEngineModule<InfraModule>()
                .AddBioEngineModule<IPBApiModule>()
                .AddBioEngineModule<IPBAuthModule>()
                .AddBioEngineModule<TwitterModule>()
                .AddBioEngineModule<FacebookModule>()
                .UseStartup<Startup>();
    }
}