using Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Web.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        var result = JsonSerializer.Serialize(new
                        {
                            Code = e.Code,
                            Errors = e.Errors,
                            ErrorsDetail = e.ErrorDetails
                        });
                        await response.WriteAsync(result);
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        await response.WriteAsync(JsonSerializer.Serialize(new { message = error?.Message }));
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await response.WriteAsync(JsonSerializer.Serialize(new { message = error?.Message }));
                        break;
                }


            }
        }
    }
}