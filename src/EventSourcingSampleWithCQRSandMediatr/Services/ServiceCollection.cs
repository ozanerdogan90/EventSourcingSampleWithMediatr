using EventSourcingSampleWithCQRSandMediatr.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingSampleWithCQRSandMediatr.Services
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services.AddTransient<IEncryptionService, EncryptionService>();
        }
    }
}
