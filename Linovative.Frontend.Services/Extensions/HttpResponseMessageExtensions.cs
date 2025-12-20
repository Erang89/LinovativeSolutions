using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.JsonConverters;
using Newtonsoft.Json;
using System.Xml;


namespace Linovative.Frontend.Services.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Response<T>> ToAppResponse<T>(this HttpResponseMessage message, CancellationToken token)
        {
            var isValid = message.IsSuccessStatusCode;
            var jsonString = await message.Content.ReadAsStringAsync(token);

            var result = JsonConvert
                .DeserializeObject<Response<T>>(jsonString, JsonSettingsProvider.Default)
                ?.Result(isValid)
                ?? Response<T>.Failed("An error occurred. Please contact your administrator.");

            return result!;
        }

        public static async Task<Response> ToAppBoolResponse(this HttpResponseMessage message, CancellationToken token)
        {
            var isValid = message.IsSuccessStatusCode;
            var jsonString = await message.Content.ReadAsStringAsync(token);

            var result = JsonConvert.DeserializeObject<Response>(jsonString, JsonSettingsProvider.Default)
                         ?? Response.Failed("An error occurred. Please contact your administrator.");

            return result!;
        }


    }

    public static class JsonSettingsProvider
    {
        public static readonly JsonSerializerSettings Default = Create();

        private static JsonSerializerSettings Create()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // other global settings you need
            };
            settings.Converters.Add(new Iso8601TimeSpanNewtonsoftConverter());
            return settings;
        }
    }


}
