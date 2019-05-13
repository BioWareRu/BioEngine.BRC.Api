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
                .AddModule<IPBApiModule>()
                .AddModule<IPBAuthModule>()
                .AddModule<TwitterModule>()
                .AddModule<FacebookModule>()
                .AddModule<AdsModule>()
                .GetHostBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
