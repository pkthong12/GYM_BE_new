using API;
using GYM_BE.Main;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;

namespace GYM_BE.All.System.Common.Middleware
{
    public class RequestResponseLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggerMiddleware> _logger;

        public RequestResponseLoggerMiddleware
            (
            RequestDelegate next,
             ILogger<RequestResponseLoggerMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext,
               IOptions<AppSettings> options
            )
        {

            string requestBodyContent = await ReadBodyFromRequest(httpContext.Request);

            if (requestBodyContent.IndexOf("sysMutationLogBeforeAfterRequest") < 0)
            {
                await _next(httpContext);
                return;
            }


            // Middleware is enabled only when the 
            // EnableRequestResponseLogging config value is set.

            AppSettings _appSettings = options.Value;
            HttpRequest request = httpContext.Request;
            string sid = request.Sid(_appSettings);


            if (sid == "" || _appSettings.RequestResponseLogger == null || !_appSettings.RequestResponseLogger.IsEnabled)
            {
                await _next(httpContext);
                return;
            }

            /*log*/
            //log.LogId = Guid.NewGuid().ToString(); //Asigned with constructor
            var ip = request.HttpContext.Connection.RemoteIpAddress;

            // Temporarily replace the HttpResponseStream, 
            // which is a write-only stream, with a MemoryStream to capture 
            // its value in-flight.

            HttpResponse response = httpContext.Response;
            var originalResponseBody = response.Body;
            using var newResponseBody = new MemoryStream();
            response.Body = newResponseBody;

            // Call the next middleware in the pipeline
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                /*exception: but was not managed at app.UseExceptionHandler() 
                  or by any middleware*/
            }

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText =
                await new StreamReader(response.Body).ReadToEndAsync();

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);

            /*response*/


            /*exception: but was managed at app.UseExceptionHandler() 
              or by any middleware*/
            var contextFeature =
                httpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (contextFeature != null && contextFeature.Error != null)
            {
                Exception exception = contextFeature.Error;
            }

            //var jsonString = logCreator.LogString(); /*log json*/
            //_logger.Log(logCreator);
        }


        private static Dictionary<string, string> FormatHeaders(IHeaderDictionary headers)
        {
            Dictionary<string, string> pairs = new();
            foreach (var header in headers)
            {
                pairs.Add(header.Key, header.Value!);
            }
            return pairs;
        }

        private List<KeyValuePair<string, string>> FormatQueries(string queryString)
        {
            List<KeyValuePair<string, string>> pairs = new();
            string key, value;
            foreach (var query in queryString.TrimStart('?').Split("&"))
            {
                var items = query.Split("=");
                key = items.Count() >= 1 ? items[0] : string.Empty;
                value = items.Count() >= 2 ? items[1] : string.Empty;
                if (!String.IsNullOrEmpty(key))
                {
                    pairs.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return pairs;
        }

        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times 
            // (for the next middlewares in the pipeline).
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            // Reset the request's body stream position for 
            // next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
