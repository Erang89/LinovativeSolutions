using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace LinoVative.Service.Backend.LocalizerServices
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly string _basePath;
        private readonly string _resourceName;
        private static readonly ConcurrentDictionary<string, IReadOnlyDictionary<string, string>> _cache = new();

        public JsonStringLocalizer(string basePath, string resourceName)
        {
            _basePath = basePath;
            _resourceName = resourceName; // e.g., "validation"
        }

        public LocalizedString this[string name]
            => Get(name, CultureInfo.CurrentUICulture, Array.Empty<object>());

        public LocalizedString this[string name, params object[] arguments]
            => Get(name, CultureInfo.CurrentUICulture, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var dict = Load(CultureInfo.CurrentUICulture);
            return dict.Select(kv => new LocalizedString(kv.Key, kv.Value, false));
        }

        private LocalizedString Get(string key, CultureInfo culture, object[] args)
        {
            var dict = Load(culture);
            if (!dict.TryGetValue(key, out var value))
            {
                // fallback to en
                dict = Load(CultureInfo.GetCultureInfo("en"));
                value = dict.TryGetValue(key, out var en) ? en : key;
                return new LocalizedString(key, Format(value, args), resourceNotFound: value == key);
            }
            return new LocalizedString(key, Format(value, args), false);
        }

        private static string Format(string template, object[] args)
            => args is { Length: > 0 } ? string.Format(CultureInfo.CurrentCulture, template, args) : template;

        private IReadOnlyDictionary<string, string> Load(CultureInfo culture)
        {
            var cacheKey = $"{_resourceName}:{culture.Name}";
            return _cache.GetOrAdd(cacheKey, _ =>
            {
                var file = Path.Combine(_basePath, "common", $"{_resourceName}.{culture.Name}.json");
                if (!File.Exists(file))
                {
                    // try neutral culture (e.g., "id" from "id-ID")
                    if (!string.IsNullOrEmpty(culture.TwoLetterISOLanguageName))
                    {
                        var neutral = Path.Combine(_basePath, "common", $"{_resourceName}.{culture.TwoLetterISOLanguageName}.json");
                        if (File.Exists(neutral)) file = neutral;
                    }
                }
                if (!File.Exists(file))
                {
                    return new Dictionary<string, string>(); // empty → key fallback
                }
                var json = File.ReadAllText(file);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                       ?? new Dictionary<string, string>();
            });
        }
    }
}
