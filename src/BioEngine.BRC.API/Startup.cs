using System.Globalization;
using System.Linq;
using BioEngine.BRC.Api.Components;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Search;
using BioEngine.Core.Web;
using BioEngine.Extra.IPB.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BioEngine.BRC.Api
{
    public class Startup : BioEngineApiStartup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment) : base(configuration,
            hostEnvironment)
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
            services.AddScoped<BRCContentPublisher>();

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

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (env.IsProduction())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BioContext>();
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }


                var searchProviders = scope.ServiceProvider.GetServices<ISearchProvider>();
                if (searchProviders != null)
                {
                    foreach (var searchProvider in searchProviders)
                    {
                        searchProvider.InitAsync().GetAwaiter().GetResult();
                    }
                }
            }
        }

        protected override void ConfigureAfterRouting(IApplicationBuilder app, IHostEnvironment env)
        {
            base.ConfigureAfterRouting(app, env);
            app.UseCors("allorigins");
        }
    }
}
