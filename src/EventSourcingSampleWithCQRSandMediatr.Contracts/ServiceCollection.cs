using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts
{
    public static class ServiceCollection
    {
        public static IMvcBuilder AddContractValidators(this IMvcBuilder builder)
        {
            return builder.AddFluentValidation(opt =>
              {
                  opt.ImplicitlyValidateChildProperties = true;
                  opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
              });
        }
    }
}
