using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace EventSourcingSampleWithCQRSandMediatr.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger<CustomExceptionFilter> logger;

        public CustomExceptionFilter(
                IWebHostEnvironment hostingEnvironment,
                ILogger<CustomExceptionFilter> logger)
        {
            this.hostEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var isDev = this.hostEnvironment.IsDevelopment();
            var correlationId = context.HttpContext.TraceIdentifier;

            var errorDic = new Dictionary<string, string>()
            {
                { "CorrelationId", correlationId},
                { "ErrorMessage",  isDev? exception.Demystify().ToString() : exception.Message},
                { "ErrorTrace",  isDev ? exception.StackTrace : string.Empty },
            };
            logger.LogError(exception, exception.Message, correlationId);

            context.Result = new ObjectResult(errorDic)
            { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
