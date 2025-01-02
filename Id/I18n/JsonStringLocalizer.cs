using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Id.I18n
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IDistributedCache _cache;
        private readonly Newtonsoft.Json.JsonSerializer _serializer = new Newtonsoft.Json.JsonSerializer();
        private readonly string localesPath;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
            string projectPath = AppDomain.CurrentDomain.BaseDirectory
                    .Substring(0, AppDomain.CurrentDomain.BaseDirectory
                    .IndexOf("bin"));
            localesPath = Path.Combine(projectPath, "Locales");
        }

        public LocalizedString this[string name]
        {
            get
            {
                string value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                LocalizedString actualValue = this[name];
                return !actualValue.ResourceNotFound
                     ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                     : actualValue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            string filePath = Path.Combine(localesPath, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() + ".json");

            if(!File.Exists(filePath))
            {
                filePath = Path.Combine(localesPath, "en.json");
                if(!File.Exists(filePath))
                {
                    throw new FileNotFoundException("en.json file not found.");
                }
            }
            using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader sReader = new StreamReader(stream);
            using JsonTextReader reader = new JsonTextReader(sReader);
            while(reader.Read())
            {
                if(reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                string key = (string)reader.Value;
                _ = reader.Read();
                string value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value, false);
            }
        }

        private string GetString(string key)
        {
            string filePath = Path.Combine(localesPath, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() + ".json");
            if(!File.Exists(filePath))
            {
                filePath = Path.Combine(localesPath, "en.json");
                if(!File.Exists(filePath))
                {
                    throw new FileNotFoundException("en.json file not found.");
                }
            }

            string cacheKey = $"locale_{Thread.CurrentThread.CurrentUICulture.Name}_{key}";
            string cacheValue = _cache.GetString(cacheKey);
            if(!string.IsNullOrEmpty(cacheValue))
            {
                return cacheValue;
            }

            string result = GetValueFromJSON(key, filePath);
            if(!string.IsNullOrEmpty(result))
            {
                _cache.SetString(cacheKey, result);
            }

            return result;
        }

        private string GetValueFromJSON(string propertyName, string filePath)
        {
            try
            {
                filePath = Path.Combine(localesPath, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() + ".json");
                if(!File.Exists(filePath))
                {
                    filePath = Path.Combine(localesPath, "en.json");
                    if(!File.Exists(filePath))
                    {
                        return default;
                    }
                }
                if(propertyName == null)
                {
                    return default;
                }

                if(filePath == null)
                {
                    return default;
                }

                using FileStream str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using StreamReader sReader = new StreamReader(str);
                using JsonTextReader reader = new JsonTextReader(sReader);
                while(reader.Read())
                {
                    if(reader.TokenType == JsonToken.PropertyName && (string)reader.Value == propertyName)
                    {
                        _ = reader.Read();
                        return _serializer.Deserialize<string>(reader);
                    }
                }

                return default;
            }
            catch
            {
                return default;
            }
        }
    }
}