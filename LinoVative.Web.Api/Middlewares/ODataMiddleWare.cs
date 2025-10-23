using System.Text;

namespace LinoVative.Web.Api.Middlewares
{
    public class ODataMiddleware
    {
        private readonly RequestDelegate _next;

        public ODataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.ContentType?.Contains("application/json") == false ||
                !context.Request.Path.ToString().Contains("/odata/", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var originalResponseStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);
            context.Response.Body = originalResponseStream;

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memoryStream);
            var responseBody = await reader.ReadToEndAsync();

            responseBody = responseBody.Replace("\"@odata.count\":", "\"count\":");
            responseBody = responseBody.Replace("\"value\":", "\"data\":");
            var responseBytes = Encoding.UTF8.GetBytes(responseBody);
            context.Response.ContentLength = responseBytes.Length;
            await context.Response.WriteAsync(responseBody);
        }
    }
}
