using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace Tchat.Api.Exceptions.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

		public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception, "Exception occurred: {Message}", exception.Message);

                var casted = exception as IException;
                var problemDetails = new ProblemDetails();
                if (casted is null || casted.StatusCode == 500)
                {
                    problemDetails.Status = exception is ArgumentException ? 400 : StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = "Server internal Error";
                    problemDetails.Title = "Server error";
                }
                else
                {
                    problemDetails.Status = casted.StatusCode;
                    problemDetails.Detail = casted.EMessage;
                    problemDetails.Title = "Server error";
                }

                context.Response.StatusCode = problemDetails.Status.Value;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(problemDetails, options);

                await context.Response.WriteAsync(json);
            }
        }
	}
}
