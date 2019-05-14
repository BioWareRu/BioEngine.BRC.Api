using System;
using System.Text;
using BioEngine.BRC.Domain;
using BioEngine.Core.API;
using BioEngine.Core.Logging.Loki;
using BioEngine.Core.Seo;
using BioEngine.Extra.Ads;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BioEngine.BRC.Api
{
    [UsedImplicitly]
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new Core.BioEngine(args)
                .AddPostgresDb()
                .AddBrcCommon()
                .AddElasticSearch()
                .AddModule<ApiModule>()
                .AddModule<LokiLoggingModule, LokiLoggingConfig>((configuration, environment) =>
                    new LokiLoggingConfig(configuration["BRC_LOKI_URL"]))
                .AddS3Storage()
                .AddModule<SeoModule>()
                .AddModule<IPBApiModule, IPBModuleConfig>((configuration, env) =>
                {
                    bool.TryParse(configuration["BE_IPB_API_DEV_MODE"] ?? "", out var devMode);
                    int.TryParse(configuration["BE_IPB_API_ADMIN_GROUP_ID"], out var adminGroupId);
                    int.TryParse(configuration["BE_IPB_API_PUBLISHER_GROUP_ID"], out var publisherGroupId);
                    int.TryParse(configuration["BE_IPB_API_EDITOR_GROUP_ID"], out var editorGroupId);
                    if (!Uri.TryCreate(configuration["BE_IPB_URL"], UriKind.Absolute, out var ipbUrl))
                    {
                        throw new ArgumentException($"Can't parse IPB url; {configuration["BE_IPB_URL"]}");
                    }

                    return new IPBModuleConfig(ipbUrl)
                    {
                        DevMode = devMode,
                        AdminGroupId = adminGroupId,
                        PublisherGroupId = publisherGroupId,
                        EditorGroupId = editorGroupId,
                        ApiClientId = configuration["BE_IPB_API_CLIENT_ID"],
                        ApiReadonlyKey = configuration["BE_IPB_API_READONLY_KEY"],
                        IntegrationKey = configuration["BE_IPB_INTEGRATION_KEY"]
                    };
                })
                .AddModule<IPBAuthModule>()
                .AddModule<TwitterModule, TwitterModuleConfig>((configuration, env) =>
                    new TwitterModuleConfig(configuration["BE_TWITTER_CONSUMER_KEY"],
                        configuration["BE_TWITTER_CONSUMER_SECRET"], configuration["BE_TWITTER_ACCESS_TOKEN"],
                        configuration["BE_TWITTER_ACCESS_TOKEN_SECRET"]))
                .AddModule<FacebookModule, FacebookModuleConfig>((configuration, env) =>
                {
                    var parsed = Uri.TryCreate(configuration["BE_FACEBOOK_API_URL"], UriKind.Absolute,
                        out var url);
                    if (!parsed)
                    {
                        throw new ArgumentException(
                            $"Facebook api url is incorrect: {configuration["BE_FACEBOOK_API_URL"]}");
                    }

                    return new FacebookModuleConfig(url, configuration["BE_FACEBOOK_PAGE_ID"],
                        configuration["BE_FACEBOOK_ACCESS_TOKEN"]);
                })
                .AddModule<AdsModule>()
                .GetHostBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
