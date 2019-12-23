using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;

namespace EventSourcingSampleWithCQRSandMediatr.Filters
{

    public class ApiLoggingFilter : TypeFilterAttribute
    {
        public ApiLoggingFilter() : base(typeof(ApiLoggingFilterImpl))
        {

        }

        private class ApiLoggingFilterImpl : IActionFilter
        {
            private readonly ILogger logger;
            private string arguments;
            public ApiLoggingFilterImpl(ILogger<ApiLoggingFilter> logger)
            {
                this.logger = logger;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                this.arguments = JsonConvert.SerializeObject(context.ActionArguments);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                if (context.Controller.GetType().Name.Equals("AuthController", StringComparison.InvariantCultureIgnoreCase))
                    return;

                var response = context.HttpContext.Response;
                var request = context.HttpContext.Request;
                var cid = context.HttpContext.TraceIdentifier;
                var logMessage = $"[{DateTime.UtcNow.ToString("dd/MM/yy hh:mm")}] [{response.StatusCode}]  Method:[{request.Method}] Url:[{request.Path}/{request.QueryString}] Cid:[{cid}] Body:[{arguments}]";
                if (context.HttpContext.Response.StatusCode >= (int)HttpStatusCode.BadRequest)
                {
                    logger.LogError(logMessage);
                }
                else
                {
                    logger.LogWarning(logMessage);
                }

            }
        }
    }
}