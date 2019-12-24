using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventSourcingSampleWithCQRSandMediatr.Filters;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Models;
using EventSourcingSampleWithCQRSandMediatr.Persistence;
using EventSourcingSampleWithCQRSandMediatr.Clients;

namespace EventSourcingSampleWithCQRSandMediatr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private DatabaseConfiguration DbConfig
        {
            get
            {
                return new DatabaseConfiguration()
                {
                    ApplicationName = Configuration.GetValue("Db:ApplicationName", "MyApp"),
                    ConnectionString = Configuration.GetValue("Db:ConnectionString", string.Empty),
                    UseMemoryDb = Configuration.GetValue("Db:UseMemoryDb", true)
                };
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices()
                    .AddPersistenceServices(DbConfig)
                    .AddCQRSServices();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ApiLoggingFilter());
                options.Filters.Add(typeof(CustomExceptionFilter));
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureApplicationServices();
            app.UseRouting();
            app.ConfigureEF(DbConfig);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
