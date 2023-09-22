using DriverLicenseLearningSupport.Payloads.Response;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DriverLicenseLearningSupport.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }catch(Exception ex) 
            {
                // logging
                await HandleException(context, ex);
            }
        }

        static Task HandleException(HttpContext context, Exception ex) 
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = ex.Message.ToString();

            switch(ex) 
            {
                // DataBase Update Exception
                case DbUpdateException _:
                    statusCode = StatusCodes.Status500InternalServerError;
                    break;
            }
            var errorResponse = new ErrorResponse 
            { 
                StatusCode = statusCode,
                Message = message
            };
            // set content type
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        } 
    }

    public static class ExceptionMiddlewareExtension 
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app) 
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
