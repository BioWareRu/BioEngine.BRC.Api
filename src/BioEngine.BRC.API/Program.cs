using System;
using System.Text;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core;
using BioEngine.Extra.IPB;
using BioEngine.Extra.IPB.Auth;
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
                .AddIPB()
                .AddIPBTokenAuth()
                .AddBioEngineValidation(assemblies: typeof(Post).Assembly)
                .AddBioEngineDB(domainAssembly: typeof(Post).Assembly)
                .AddBioEngineS3Storage()
                .AddBioEngineSeo()
                .UseStartup<Startup>();
    }
}