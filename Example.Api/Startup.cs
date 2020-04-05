using System;
using System.IO;
using System.Reflection;
using Example.Api.Middlewares;
using Example.Api.Security;
using Example.Api.Utils;
using Example.Data.Data;
using Example.Services.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Example.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddServices();
            var connectionString = Configuration.GetConnectionString("dbConnection");
            services.AddDbContext<RepositoryContext>(options => options.UseNpgsql(connectionString));

            services.AddSwaggerGen(
                c =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                            {Title = "Example", Version = "v1", Description = "Example project"}
                    );
                    c.OrderActionsBy(SwaggerDocumentFilter.SortSwagger);
                }
            );

            services.AddJwtAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example"); }
                );
            }

            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
