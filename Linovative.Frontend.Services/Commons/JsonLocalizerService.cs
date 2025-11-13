using Linovative.Frontend.Services.Interfaces;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Linovative.Frontend.Services.Commons
{
    public sealed class JsonLocalizerService : IJsonLocalizer
    {
        private readonly HttpClient _http;
        private readonly ILanguageProvider _lang;
        private static Dictionary<string, string> _dict = new();
        private static List<string> _keys = new();
        private readonly IApplicationStateService _state;

        public JsonLocalizerService(HttpClient http, ILanguageProvider lang, IApplicationStateService appState)
        {
            _http = http;
            _lang = lang;
            _state = appState;
        }

        public async Task EnsureLoadedAsync(string key, string? libraryName = default)
        {
            var libraryLink = libraryName is null ? null : $"_Content/{libraryName}/";
            var culture = CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName?? "en";

            if (_keys.Contains(key.ToLower()))
                return;

            _keys.Add(key.ToLower());

            var path = $"{libraryLink}i18n/{culture}/{key}.json?id={Guid.NewGuid()}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Action<Dictionary<string, string>> registerLocalizer = (sources) =>
            {
                foreach(var l in sources)
                {
                    var dicKey = $"{key}.{l.Key}";
                    if(!_dict.ContainsKey(dicKey))
                    {
                        _dict.Add(dicKey, l.Value);
                    }
                }
            };


            try
            {
                var json = (await _http.GetFromJsonAsync<Dictionary<string, string>>(path, options)) ?? new Dictionary<string, string>();
                registerLocalizer(json);
                _state.NotifyStateChanged();
            }
            catch(Exception e)
            {
                var json = (await _http.GetFromJsonAsync<Dictionary<string, string>>($"{libraryLink}i18n/en/{key}.json?id={Guid.NewGuid()}", options)) ?? new Dictionary<string, string>();
                registerLocalizer(json);
                _state.NotifyStateChanged();
                return;
            }

           
        }

        public string this[string key]
            => _dict.TryGetValue(key, out var v) ? v : key;

        public string Format(string key, params object[] args)
        {
            var template = this[key];
            return string.Format(template, args);
        }
    }
}
