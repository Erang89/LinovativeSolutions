using Linovative.Frontend.Services.FrontendServices;
using Linovative.Frontend.Services.Models;
using Newtonsoft.Json;

namespace Linovative.Frontend.Services.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Response<T>> ToAppResponse<T>(this HttpResponseMessage message, CancellationToken token)
        {
            var isValid = message.IsSuccessStatusCode;
            var jsonString = await message.Content.ReadAsStringAsync(token);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Iso8601TimeSpanConverter());


            var result = JsonConvert.DeserializeObject<Response<T>>(jsonString, settings)?.Result(isValid) ?? Response<T>.Failed("An error accourred. Please contact your adminstrator.");
            return result!;
        }


        public static async Task<Response> ToAppBoolResponse(this HttpResponseMessage message, CancellationToken token)
        {
            var isValid = message.IsSuccessStatusCode;
            var jsonString = await message.Content.ReadAsStringAsync(token);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Iso8601TimeSpanConverter());

            var result = JsonConvert.DeserializeObject<Response> (jsonString, settings);

            if (result is null || !result)
                result = Response.Failed("An error accourred. Please contact your adminstrator.");

            return result!;
        }
    }
}
