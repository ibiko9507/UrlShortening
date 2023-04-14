using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using UrlShortening.Shared.Models;

namespace VotingAPI.WebAPI.Filters
{

    public class GlobalHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            var jsonSerializerSettings = new JsonSerializerSettings { 
                NullValueHandling = NullValueHandling.Ignore, 
                ContractResolver = new CamelCasePropertyNamesContractResolver()};
            try
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;
                await _next(context);
                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseData = await new StreamReader(responseBody).ReadToEndAsync();
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var response = new ApiResponseWrapper
                    {
                        Data = JsonConvert.DeserializeObject(responseData), //todo data null ise boş yere null gelmesin
                    };
                    context.Response.ContentType = "application/json";
                    var json = JsonConvert.SerializeObject(response, jsonSerializerSettings);
                    await context.Response.WriteAsync(json);
                }
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var error = new ApiResponseWrapper { Error = ex.Message, HasError = true };
                var json = JsonConvert.SerializeObject(error, jsonSerializerSettings);
                using var errorResponseBody = new MemoryStream();
                await errorResponseBody.WriteAsync(Encoding.UTF8.GetBytes(json));
                errorResponseBody.Seek(0, SeekOrigin.Begin);
                await errorResponseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}