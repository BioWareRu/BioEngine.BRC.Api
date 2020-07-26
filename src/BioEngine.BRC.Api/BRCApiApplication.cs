using System;
using BioEngine.BRC.Common;
using BioEngine.BRC.Common.Ads.Api;
using BioEngine.BRC.Common.Facebook;
using BioEngine.BRC.Common.IPB;
using BioEngine.BRC.Common.IPB.Auth;
using BioEngine.BRC.Common.Pages.Api;
using BioEngine.BRC.Common.Posts.Api;
using BioEngine.BRC.Common.Seo;
using BioEngine.BRC.Common.Twitter;
using BioEngine.BRC.Common.Web.Api;

namespace BioEngine.BRC.Api
{
    public class BRCApiApplication : BRCApplication
    {
        public BRCApiApplication(string[] args) : base(args)
        {
            AddPostgresDb()
                .AddBrcDomain()
                .AddModule<PostsApiModule>()
                .AddModule<PagesApiModule>()
                .AddElasticSearch()
                .AddModule<ApiModule, ApiModuleConfig>()
                .AddElasticStack()
                .AddS3Storage()
                .AddModule<SeoModule>()
                .AddModule<IPBApiModule, IPBApiModuleConfig>((configuration, env, moduleConfig) =>
                {
                    if (!Uri.TryCreate(configuration["BE_IPB_URL"], UriKind.Absolute, out var ipbUrl))
                    {
                        throw new ArgumentException($"Can't parse IPB url; {configuration["BE_IPB_URL"]}");
                    }

                    moduleConfig.Url = ipbUrl;


                    moduleConfig.ApiReadonlyKey = configuration["BE_IPB_API_READONLY_KEY"];
                    moduleConfig.ApiPublishKey = configuration["BE_IPB_API_PUBLISH_KEY"];
                })
                .AddIpbUsers<IPBApiUsersModule, IPBApiUsersModuleConfig, IPBApiCurrentUserProvider>()
                .AddModule<TwitterModule>()
                .AddModule<FacebookModule>()
                .AddModule<AdsApiModule>();
        }
    }
}
