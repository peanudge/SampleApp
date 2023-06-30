using System.Net;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class HttpCustomExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpCustomExceptionFilter> _logger;

        public HttpCustomExceptionFilter(IWebHostEnvironment env, ILogger<HttpCustomExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var eventId = new EventId(context.Exception.HResult);

            _logger.LogError(eventId, context.Exception, context.Exception.Message);

            var json = new JsonErrorPayload { EventId = eventId.Id };

            if (_env.IsDevelopment())
            {
                json.DetailedMessage = new
                {
                    context.Exception.Message,
                    context.Exception.StackTrace
                };
            }

            var exceptionObject = new ObjectResult(json)
            {
                StatusCode = 500,
            };

            context.Result = exceptionObject;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
