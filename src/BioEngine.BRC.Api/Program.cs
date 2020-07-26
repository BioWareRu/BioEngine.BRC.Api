using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BioEngine.BRC.Api
{
    [UsedImplicitly]
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var application = CreateApplication(args);
            application.GetHostBuilder().ConfigureWebHost(builder =>
            {
                builder.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024; // 1 gb
                });
            });
            await application.RunAsync<Startup>();
        }

        // need for migrations
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            CreateApplication(args).CreateBasicHostBuilder<Startup>();

        public static BRCApiApplication CreateApplication(string[] args) => new BRCApiApplication(args);
    }
}
