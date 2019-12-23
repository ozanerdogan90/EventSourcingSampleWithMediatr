using EventSourcingSampleWithCQRSandMediatr.DataAccess.Models;
using EventSourcingSampleWithCQRSandMediatr.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingSampleWithCQRSandMediatr.DataAccess
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, DatabaseConfiguration config)
        {
            return services.AddRepositories()
                           .AddCustomerContext(config);
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IAuditRepository, AuditRepository>();
        }
        private static IServiceCollection AddCustomerContext(this IServiceCollection services, DatabaseConfiguration config)
        {
            if (config.UseMemoryDb)
            {
                services.AddDbContext<Context>(options =>
                options.UseInMemoryDatabase(config.ApplicationName));
            }
            else
            {
                services.AddDbContext<Context>(options =>
                        options.UseNpgsql(config.ConnectionString));
            }

            return services;
        }

        public static void ConfigureEF(this IApplicationBuilder app, DatabaseConfiguration dbConfig)
        {
            if (dbConfig.UseMemoryDb)
            {
                using (var scope =
          app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                using (var context = scope.ServiceProvider.GetService<Context>())
                {
                    context.ChangeTracker.LazyLoadingEnabled = false;
                }
                return;
            };

            using (var scope =
      app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<Context>())
                context.Database.Migrate();
        }

    }
}
