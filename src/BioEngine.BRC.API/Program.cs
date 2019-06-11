using System;
using System.Text;
using System.Threading.Tasks;
using BioEngine.BRC.Common;
using BioEngine.Core.API;
using BioEngine.Core.Logging.Loki;
using BioEngine.Core.Seo;
using BioEngine.Extra.Ads;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;

namespace BioEngine.BRC.Api
{
    [UsedImplicitly]
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var bioEngine = CreateBioEngine(args);

            await bioEngine.RunAsync<Startup>();
        }

        private static Core.BioEngine CreateBioEngine(string[] args)
        {
            return new Core.BioEngine(args)
                .AddPostgresDb()
                .AddBrcDomain()
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
                .AddModule<TwitterModule>()
                .AddModule<FacebookModule>()
                .AddModule<AdsModule>();
        }
    }
}
