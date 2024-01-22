using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Talabat.Apis2.ErrorsResponseHandling;

namespace Talabat.Apis2.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
               await  next.Invoke(context);
            }
            catch (Exception ex)
            {
                //the Hader of Response

                logger.LogError(ex, ex.Message); // we here log the Exception in console in case we at Development 
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = /*500*/ (int) HttpStatusCode.InternalServerError;

                // the body of Response

                var response = env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                   : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);



            }

        }


    }
}
