using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BioEngine.BRC.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddApplicationPart(typeof(ResponseRestController<,>).Assembly)
                .AddApplicationPart(typeof(ForumsController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddHttpContextAccessor();
            services.AddScoped<IViewRenderService, ViewRenderService>();
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BRC API", Version = "v1"});
                //var security = new Dictionary<string, IEnumerable<string>> {,};

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Description = "IPB Auth token",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        },
                        new string[] { }
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {Type = SecuritySchemeType.ApiKey});
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, BioContext dbContext,
            IEnumerable<ISearchProvider> searchProviders = default)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            if (searchProviders != null)
                foreach (var searchProvider in searchProviders)
                {
                    searchProvider.InitAsync().GetAwaiter().GetResult();
                }


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var supportedCultures = new[] {new CultureInfo("ru-RU"), new CultureInfo("ru")};

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseCors("allorigins");

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BRC API V1"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
