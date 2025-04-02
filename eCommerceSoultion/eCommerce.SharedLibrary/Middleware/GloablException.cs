using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.Middleware
{
    public class GloablException(RequestDelegate next)
    {
        string message = "Sorry, Internal server error occured!!";
        int statusCode=(int)HttpStatusCode.InternalServerError;
        string title = "Error";
        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await next(context);

                // Too many request
                if (context.Response.StatusCode==StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request!!";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                }

                // Unathorized Request
                if (context.Response.StatusCode==StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                }
                // Forbidden
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed to access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                }
                await ModifyHeader(context, title, message, statusCode);
            }
            catch (Exception ex){
                LogException.LogExceptions(ex);

                // Timeout Excpetion
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time!!";
                    message = "Ruquest timeout..";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                await ModifyHeader(context, title, message, statusCode);
            }
        }

        public static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails() { 
                Detail=message,
                Status=statusCode,
                Title=title
            }),CancellationToken.None);
            return;
        }
    }
}
