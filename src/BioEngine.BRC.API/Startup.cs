using System.Collections.Generic;
using System.Globalization;
using BioEngine.BRC.Api.Components;
using BioEngine.Core.API;
using BioEngine.Core.DB;
using BioEngine.Core.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BioEngine.BRC.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver
                        = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                }).AddApplicationPart(typeof(ResponseRestController<,>).Assembly)
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
                        corsBuilder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowCredentials();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "BRC API", Version = "v1"});
                var security = new Dictionary<string, IEnumerable<string>> {{"Bearer", new string[] { }},};

                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        Description = "IPB Auth token", Name = "Authorization", In = "header", Type = "apiKey"
                    });
                c.AddSecurityRequirement(security);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BioContext dbContext)
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

                app.UseHsts();
            }


            app.UseAuthentication();

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
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BRC API V1"); });

            app.UseMvc();
        }
    }
}
