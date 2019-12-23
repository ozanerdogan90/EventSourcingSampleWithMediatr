using EventSourcingSampleWithCQRSandMediatr.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventSourcingSampleWithCQRSandMediatr.DataAccess;
using EventSourcingSampleWithCQRSandMediatr.Filters;
using EventSourcingSampleWithCQRSandMediatr.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

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
                    ApplicationName = Configuration.GetValue("Db:ApplicationName", "EventSourcingSample"),
                    ConnectionString = Configuration.GetValue("Db:ConnectionString", string.Empty),
                    UseMemoryDb = Configuration.GetValue("Db:UseMemoryDb", true),
                };
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusinessServices()
                    .AddRepositories(DbConfig)
                    .AddApplicationServices();

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
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
            app.ConfigureEF(DbConfig);
            app.ConfigureApplicationServices();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
