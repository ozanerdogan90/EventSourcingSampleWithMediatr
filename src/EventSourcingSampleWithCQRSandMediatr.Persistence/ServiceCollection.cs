using EventSourcingSampleWithCQRSandMediatr.Persistence.DataAccess;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Models;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, DatabaseConfiguration config)
        {
            return services.AddScoped<IGameRepository, GameRepository>()
                           .AddContext(config);

        }

        private static IServiceCollection AddContext(this IServiceCollection services, DatabaseConfiguration config)
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
