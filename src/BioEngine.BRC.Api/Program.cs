using System;
using System.Text;
using System.Threading.Tasks;
using BioEngine.BRC.Common;
using BioEngine.Core.Api;
using BioEngine.Core.Pages.Api;
using BioEngine.Core.Posts;
using BioEngine.Core.Posts.Api;
using BioEngine.Core.Seo;
using BioEngine.Extra.Ads.Api;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB;
using BioEngine.Extra.IPB.Auth;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BioEngine.BRC.Api
{
    [UsedImplicitly]
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var bioEngine = CreateBioEngine(args);
            bioEngine.GetHostBuilder().ConfigureWebHost(builder =>
            {
                builder.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024; // 1 gb
                });
            });
            await bioEngine.RunAsync<Startup>();
        }

        // need for migrations
        [UsedImplicitly]
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            CreateBioEngine(args).GetHostBuilder().ConfigureAppConfiguration(builder =>
            {
                builder.AddUserSecrets<Startup>();
                builder.AddEnvironmentVariables();
            });

        private static Core.BioEngine CreateBioEngine(string[] args)
        {
            return new Core.BioEngine(args)
                .AddPostgresDb()
                .AddBrcDomain()
                .AddModule<PostsApiModule<string>>()
                .AddModule<PostTemplatesModule<string>>()
                .AddModule<PagesApiModule>()
                .AddElasticSearch()
                .AddModule<ApiModule, ApiModuleConfig>((configuration, env) => new ApiModuleConfig())
                .AddLogging()
                .AddS3Storage()
                .AddModule<SeoModule>()
                .AddModule<IPBApiModule, IPBApiModuleConfig>((configuration, env) =>
                {
                    if (!Uri.TryCreate(configuration["BE_IPB_URL"], UriKind.Absolute, out var ipbUrl))
                    {
                        throw new ArgumentException($"Can't parse IPB url; {configuration["BE_IPB_URL"]}");
                    }

                    var config = new IPBApiModuleConfig(ipbUrl)
                    {
                        ApiReadonlyKey = configuration["BE_IPB_API_READONLY_KEY"],
                        ApiPublishKey = configuration["BE_IPB_API_PUBLISH_KEY"],
                    };

                    return config;
                })
                .AddIpbUsers<IPBApiUsersModule, IPBApiUsersModuleConfig, IPBApiCurrentUserProvider>()
                .AddModule<TwitterModule>()
                .AddModule<FacebookModule>()
                .AddModule<AdsApiModule>();
        }
    }
}
