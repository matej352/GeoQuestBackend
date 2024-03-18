﻿using Newtonsoft.Json;
using System.Net;

namespace GeoQuest.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception and customize the error response
                // Log the exception details if needed
                // Send a custom error response to the client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new ErrorResponse { message = ex.Message };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }

    internal class ErrorResponse
    {
        public string message { get; set; }
    }
}
