using LinoVative.Service.Backend.Interfaces;
using Newtonsoft.Json;

namespace LinoVative.Service.Backend.LocalizerServices
{
    public class LangueageService : ILangueageService
    {
        private Dictionary<string, string> _dics { get; set; }  = new();
        private List<string> _loadedKey { get; set; } = new();


        public string this[string key] => !_dics.ContainsKey(key)? key : _dics[key];

        public async Task EnsureLoad(string key)
        {
            if(_loadedKey.Contains(key.ToLower()))
            {
                return;    
            }

            _loadedKey.Add(key.ToLower());

            var lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var defaultDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "en");
            var folderDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", lang);
            
            var filePath = Path.Combine(folderDir!, $"{key}.json");
            var defaultPath = Path.Combine(defaultDir!, $"{key}.json");

            string pathToLoad = File.Exists(filePath) ? filePath : defaultPath;

            if (File.Exists(pathToLoad))
            {
                var jsonContent = File.ReadAllText(pathToLoad);
                var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent) ?? new Dictionary<string, string>();
                foreach(var m in message)
                {
                    _dics.Add($"{key}.{m.Key}", m.Value);
                }
            }
            else
            {
                throw new FileNotFoundException($"Message file not found: {pathToLoad}");
            }

            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public string Format(string key, object value)
        {
            if (!_dics.ContainsKey(key))
                return key;

            return string.Format(_dics["key"], value);
        }
    }
}
