using System;
using System.Text;
using BioEngine.BRC.Domain;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Logging.Loki;
using BioEngine.Core.Search.ElasticSearch;
using BioEngine.Core.Seo;
using BioEngine.Core.Storage;
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
                .AddModule<PostgresDatabaseModule, PostgresDatabaseModuleConfig>(
                    (config, configuration, env) =>
                    {
                        config.Host = configuration["BE_POSTGRES_HOST"];
                        config.Port = int.Parse(configuration["BE_POSTGRES_PORT"]);
                        config.Username = configuration["BE_POSTGRES_USERNAME"];
                        config.Password = configuration["BE_POSTGRES_PASSWORD"];
                        config.Database = configuration["BE_POSTGRES_DATABASE"];
                        config.EnablePooling = env.IsDevelopment();
                        config.MigrationsAssembly = typeof(Developer).Assembly;
                    })
                .AddModule<BrcDomainModule>()
                .AddModule<ElasticSearchModule, ElasticSearchModuleConfig>((config, configuration, env) =>
                {
                    config.Url = configuration["BE_ELASTICSEARCH_URI"];
                    config.Login = configuration["BE_ELASTICSEARCH_LOGIN"];
                    config.Password = configuration["BE_ELASTICSEARCH_PASSWORD"];
                })
                .AddModule<ApiModule>()
                .AddModule<LokiLoggingModule, LokiLoggingConfig>((config, configuration, env) =>
                {
                    config.Url = configuration["BRC_LOKI_URL"];
                })
                .AddModule<S3StorageModule, S3StorageModuleConfig>((config, configuration, env) =>
                {
                    var uri = configuration["BE_STORAGE_PUBLIC_URI"];
                    var success = Uri.TryCreate(uri, UriKind.Absolute, out var publicUri);
                    if (!success)
                    {
                        throw new ArgumentException($"URI {uri} is not proper URI");
                    }

                    var serverUriStr = configuration["BE_STORAGE_S3_SERVER_URI"];
                    success = Uri.TryCreate(serverUriStr, UriKind.Absolute, out var serverUri);
                    if (!success)
                    {
                        throw new ArgumentException($"S3 server URI {uri} is not proper URI");
                    }

                    config.PublicUri = publicUri;
                    config.ServerUri = serverUri;
                    config.Bucket = configuration["BE_STORAGE_S3_BUCKET"];
                    config.AccessKey = configuration["BE_STORAGE_S3_ACCESS_KEY"];
                    config.SecretKey = configuration["BE_STORAGE_S3_SECRET_KEY"];
                })
                .AddModule<SeoModule>()
                .AddModule<IPBApiModule, IPBModuleConfig>((config, configuration, env) =>
                {
                    bool.TryParse(configuration["BE_IPB_API_DEV_MODE"] ?? "", out var devMode);
                    int.TryParse(configuration["BE_IPB_API_ADMIN_GROUP_ID"], out var adminGroupId);
                    int.TryParse(configuration["BE_IPB_API_PUBLISHER_GROUP_ID"], out var publisherGroupId);
                    int.TryParse(configuration["BE_IPB_API_EDITOR_GROUP_ID"], out var editorGroupId);
                    if (!Uri.TryCreate(configuration["BE_IPB_URL"], UriKind.Absolute, out var ipbUrl))
                    {
                        throw new ArgumentException($"Can't parse IPB url; {configuration["BE_IPB_URL"]}");
                    }

                    config.DevMode = devMode;
                    config.AdminGroupId = adminGroupId;
                    config.PublisherGroupId = publisherGroupId;
                    config.EditorGroupId = editorGroupId;
                    config.Url = ipbUrl;
                    config.ApiClientId = configuration["BE_IPB_API_CLIENT_ID"];
                    config.ApiReadonlyKey = configuration["BE_IPB_API_READONLY_KEY"];
                    config.IntegrationKey = configuration["BE_IPB_INTEGRATION_KEY"];
                })
                .AddModule<IPBAuthModule>()
                .AddModule<TwitterModule, TwitterModuleConfig>((config, configuration, env) =>
                {
                    config.ConsumerKey = configuration["BE_TWITTER_CONSUMER_KEY"];
                    config.ConsumerSecret = configuration["BE_TWITTER_CONSUMER_SECRET"];
                    config.AccessToken = configuration["BE_TWITTER_ACCESS_TOKEN"];
                    config.AccessTokenSecret = configuration["BE_TWITTER_ACCESS_TOKEN_SECRET"];
                })
                .AddModule<FacebookModule, FacebookModuleConfig>((config, configuration, env) =>
                {
                    var parsed = Uri.TryCreate(configuration["BE_FACEBOOK_API_URL"], UriKind.Absolute,
                        out var url);
                    if (!parsed)
                    {
                        throw new ArgumentException(
                            $"Facebook api url is incorrect: {configuration["BE_FACEBOOK_API_URL"]}");
                    }

                    config.Url = url;
                    config.PageId = configuration["BE_FACEBOOK_PAGE_ID"];
                    config.AccessToken = configuration["BE_FACEBOOK_ACCESS_TOKEN"];
                })
                .AddModule<AdsModule>()
                .GetHostBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
