using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BioEngine.BRC.Common;
using BioEngine.Core.Api;
using BioEngine.Core.Api.Auth;
using BioEngine.Core.Pages.Api;
using BioEngine.Core.Posts.Api;
using BioEngine.Core.Seo;
using BioEngine.Extra.Ads.Api;
using BioEngine.Extra.ContentTemplates;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
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
                .AddModule<PostsApiModule>()
                .AddModule<PagesApiModule>()
                .AddElasticSearch()
                .AddModule<ApiModule>()
                .AddLogging()
                .AddS3Storage()
                .AddModule<SeoModule>()
                .AddModule<ContentItemTemplatesModule>()
                .AddModule<IPBApiModule, IPBApiModuleConfig>((configuration, env) =>
                {
                    bool.TryParse(configuration["BE_IPB_API_DEV_MODE"] ?? "", out var devMode);
                    int.TryParse(configuration["BE_IPB_API_ADMIN_GROUP_ID"], out var adminGroupId);
                    int.TryParse(configuration["BE_IPB_API_SITE_TEAM_GROUP_ID"], out var siteTeamGroupId);
                    var additionalGroupIds = new List<int> {siteTeamGroupId};
                    if (!string.IsNullOrEmpty(configuration["BE_IPB_API_ADDITIONAL_GROUP_IDS"]))
                    {
                        var ids = configuration["BE_IPB_API_ADDITIONAL_GROUP_IDS"].Split(',');
                        foreach (var id in ids)
                        {
                            if (int.TryParse(id, out var parsedId))
                            {
                                additionalGroupIds.Add(parsedId);
                            }
                        }
                    }

                    if (!Uri.TryCreate(configuration["BE_IPB_URL"], UriKind.Absolute, out var ipbUrl))
                    {
                        throw new ArgumentException($"Can't parse IPB url; {configuration["BE_IPB_URL"]}");
                    }

                    var config = new IPBApiModuleConfig(ipbUrl)
                    {
                        DevMode = devMode,
                        AdminGroupId = adminGroupId,
                        AdditionalGroupIds = additionalGroupIds.Distinct().ToArray(),
                        ApiClientId = configuration["BE_IPB_API_CLIENT_ID"],
                        ApiReadonlyKey = configuration["BE_IPB_API_READONLY_KEY"],
                        ApiPublishKey = configuration["BE_IPB_API_PUBLISH_KEY"],
                    };

                    var adminPolicy = new AuthorizationPolicyBuilder().RequireClaim(ClaimTypes.Role, "admin").Build();
                    var siteTeamPolicy = new AuthorizationPolicyBuilder()
                        .RequireClaim(ClaimTypes.GroupSid, siteTeamGroupId.ToString(), adminGroupId.ToString())
                        .Build();
                    var policies = new Dictionary<string, AuthorizationPolicy>
                    {
                        // sections
                        {BioPolicies.Sections, siteTeamPolicy},
                        {BioPolicies.SectionsAdd, adminPolicy},
                        {BioPolicies.SectionsEdit, adminPolicy},
                        {BioPolicies.SectionsPublish, adminPolicy},
                        {BioPolicies.SectionsDelete, adminPolicy},
                        // posts
                        {PostsPolicies.Posts, siteTeamPolicy},
                        {PostsPolicies.PostsAdd, siteTeamPolicy},
                        {PostsPolicies.PostsEdit, siteTeamPolicy},
                        {PostsPolicies.PostsDelete, siteTeamPolicy},
                        {PostsPolicies.PostsPublish, siteTeamPolicy},
                        // pages
                        {PagesPolicies.Pages, adminPolicy},
                        {PagesPolicies.PagesAdd, adminPolicy},
                        {PagesPolicies.PagesEdit, adminPolicy},
                        {PagesPolicies.PagesDelete, adminPolicy},
                        {PagesPolicies.PagesPublish, adminPolicy},
                        // ads
                        {AdsPolicies.Ads, adminPolicy},
                        {AdsPolicies.AdsAdd, adminPolicy},
                        {AdsPolicies.AdsEdit, adminPolicy},
                        {AdsPolicies.AdsDelete, adminPolicy},
                        {AdsPolicies.AdsPublish, adminPolicy}
                    };

                    config.EnableAuth(policies);

                    return config;
                })
                .AddModule<TwitterModule>()
                .AddModule<FacebookModule>()
                .AddModule<AdsApiModule>();
        }
    }
}
