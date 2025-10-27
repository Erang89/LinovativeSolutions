using Linovative.Frontend.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Linovative.Frontend.Services.Commons
{
    public sealed class JsonLocalizerService : IJsonLocalizer
    {
        private readonly HttpClient _http;
        private readonly ILanguageProvider _lang;
        private Dictionary<string, string> _dict = new();
        public string CurrentCulture { get; private set; } = "";

        public JsonLocalizerService(HttpClient http, ILanguageProvider lang)
        {
            _http = http;
            _lang = lang;
        }

        public async Task EnsureLoadedAsync(string culture)
        {
            if (!string.IsNullOrWhiteSpace(CurrentCulture) && string.Equals(CurrentCulture, culture, StringComparison.OrdinalIgnoreCase))
                return;

            // contoh path: /i18n/id.json
            var path = $"i18n/{culture}.json";

            // options agar case-insensitive
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                var json = await _http.GetFromJsonAsync<Dictionary<string, string>>(path, options);
                _dict = json ?? new Dictionary<string, string>();
                CurrentCulture = culture;
            }
            catch
            {
                // fallback ke default culture dari provider
                var fallback = _lang.DefaultCulture;
                if (!string.Equals(culture, fallback, StringComparison.OrdinalIgnoreCase))
                {
                    var json = await _http.GetFromJsonAsync<Dictionary<string, string>>($"i18n/{fallback}.json", options);
                    _dict = json ?? new Dictionary<string, string>();
                    CurrentCulture = fallback;
                    return;
                }

                _dict = new Dictionary<string, string>();
                CurrentCulture = culture;
            }
        }

        public string this[string key]
            => _dict.TryGetValue(key, out var v) ? v : key; // jika tidak ada, kembalikan key

        public string Format(string key, params object[] args)
        {
            var template = this[key];
            return string.Format(template, args);
        }
    }
}
