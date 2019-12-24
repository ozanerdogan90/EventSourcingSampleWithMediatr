using AutoMapper;
using CorrelationId;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Prometheus;
using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddCorrelationIdServices()
                       .AddSwaggerServices()
                       .AddResponseCompression()
                       .AddMediatr()
                       .AddProblemDetailServices();

            return services;
        }

        private static IServiceCollection AddMediatr(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<ServiceFactory>(sp => t => sp.GetService(t));
            return services;
        }


        private static IServiceCollection AddCorrelationIdServices(this IServiceCollection services)
        {
            services.AddCorrelationId();
            return services;
        }

        private static IServiceCollection AddProblemDetailServices(this IServiceCollection services)
        {
            services.AddProblemDetails();
            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Event Sourcing Sample With CQRS and Mediatr",
                });
                c.DescribeAllParametersInCamelCase();
                c.DescribeAllEnumsAsStrings();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        private static IApplicationBuilder UsePrometheusServices(this IApplicationBuilder app)
        {
            return app.UseMetricServer()
                  .UseHttpMetrics();
        }

        public static IApplicationBuilder ConfigureApplicationServices(this IApplicationBuilder app)
        {

            app.UseCors(x => x
                          .AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader())
              .UseCorrelationId(new CorrelationIdOptions
              {
                  Header = "X-Correlation-ID",
                  UseGuidForCorrelationId = true,
                  UpdateTraceIdentifier = true,
                  IncludeInResponse = true
              })
              .UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Sourcing Sample With CQRS and Mediatr");
              })
              .UseResponseCompression()
              .UsePrometheusServices()
              .UseProblemDetails();

            return app;
        }


    }
}
