using BioEngine.BRC.Domain.Entities;
using BioEngine.Core;
using BioEngine.Extra.IPB.Api;
using BioEngine.Extra.IPB.Auth;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BioEngine.BRC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .AddIPB()
                .AddIPBTokenAuth()
                .AddBioEngineValidation(assemblies: typeof(Post).Assembly)
                .AddBioEngineDB(domainAssembly: typeof(Post).Assembly)
                .UseStartup<Startup>();
    }
}