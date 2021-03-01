using System;
using System.Globalization;
using BioEngine.BRC.Api.Components;
using BioEngine.BRC.Common;
using BioEngine.Core.Api;
using BioEngine.Core.Web;
using BioEngine.Extra.IPB.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BioEngine.BRC.Api
{
    public class Startup : BioEngineApiStartup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment) : base(configuration,
            environment)
        {
        }

        protected override IMvcBuilder ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            return base.ConfigureMvc(mvcBuilder).AddApplicationPart(typeof(ForumsController).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.RegisterApiEntities(GetType().Assembly);
            services.AddScoped<BRCPostsPublisher>();

            services.AddScoped<IContentRender, ContentRender>();

            services.AddCors(options =>
            {
                // Define one or more CORS policies
                options.AddPolicy("allorigins",
                    corsBuilder =>
                    {
                        corsBuilder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(s => true);
                    });
            });

            services.Configure<BrcApiOptions>(options =>
            {
                options.DefaultMainSiteId = Guid.Parse(Configuration["BE_DEFAULT_MAIN_SITE_ID"]);
            });
        }

        protected override void ConfigureBeforeRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            base.ConfigureBeforeRouting(app, env);
            var supportedCultures = new[] {new CultureInfo("ru-RU"), new CultureInfo("ru")};

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
        }

        protected override void ConfigureAfterRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            base.ConfigureAfterRouting(app, env);
            app.UseCors("allorigins");
        }

        protected override void ConfigureEndpoints(IApplicationBuilder app, IHostEnvironment env, IEndpointRouteBuilder endpoints)
        {
            endpoints.AddBrcRoutes();
            base.ConfigureEndpoints(app, env, endpoints);
        }
    }

    public class BrcApiOptions
    {
        public Guid DefaultMainSiteId { get; set; }
    }
}
