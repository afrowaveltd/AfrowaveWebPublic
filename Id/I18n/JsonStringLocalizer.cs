using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Globalization;

namespace Id.I18n
{
	/// <summary>
	/// Provides localization using JSON files stored in a distributed cache.
	/// </summary>
	public class JsonStringLocalizer : IStringLocalizer
	{
		private readonly IDistributedCache _cache;
		private readonly Newtonsoft.Json.JsonSerializer _serializer = new();
		private readonly string _localesPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonStringLocalizer"/> class.
		/// </summary>
		/// <param name="cache">Distributed cache instance.</param>
		public JsonStringLocalizer(IDistributedCache cache)
		{
			_cache = cache;
			string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0];
			_localesPath = Path.Combine(projectPath, "Locales");
		}

		/// <inheritdoc/>
		public LocalizedString this[string name] => new(name, GetString(name) ?? name, GetString(name) == null);

		/// <inheritdoc/>
		public LocalizedString this[string name, params object[] arguments]
		{
			get
			{
				LocalizedString localized = this[name];
				return localized.ResourceNotFound
					 ? localized
					 : new LocalizedString(name, string.Format(localized.Value, arguments), false);
			}
		}

		/// <inheritdoc/>
		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
			string filePath = GetLocaleFilePath();
			if(!File.Exists(filePath))
			{
				throw new FileNotFoundException("Localization file not found.");
			}

			using FileStream stream = File.OpenRead(filePath);
			using StreamReader reader = new StreamReader(stream);
			using JsonTextReader jsonReader = new JsonTextReader(reader);

			while(jsonReader.Read())
			{
				if(jsonReader.TokenType == JsonToken.PropertyName)
				{
					string key = jsonReader.Value.ToString();
					_ = jsonReader.Read();
					yield return new LocalizedString(key, _serializer.Deserialize<string>(jsonReader), false);
				}
			}
		}

		private string GetString(string key)
		{
			string filePath = GetLocaleFilePath();
			string cacheKey = $"locale_{CultureInfo.CurrentUICulture.Name}_{key}";
			string cachedValue = _cache.GetString(cacheKey);

			if(!string.IsNullOrEmpty(cachedValue))
			{
				return cachedValue;
			}

			string value = GetValueFromJson(key, filePath);
			if(!string.IsNullOrEmpty(value))
			{
				_cache.SetString(cacheKey, value);
			}

			return value;
		}

		private string GetLocaleFilePath()
		{
			string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
			string filePath = Path.Combine(_localesPath, $"{culture}.json");
			return File.Exists(filePath) ? filePath : Path.Combine(_localesPath, "en.json");
		}

		private string GetValueFromJson(string key, string filePath)
		{
			if(string.IsNullOrEmpty(key) || string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				return default;
			}

			try
			{
				using FileStream stream = File.OpenRead(filePath);
				using StreamReader reader = new StreamReader(stream);
				using JsonTextReader jsonReader = new JsonTextReader(reader);

				while(jsonReader.Read())
				{
					if(jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.ToString() == key)
					{
						_ = jsonReader.Read();
						return _serializer.Deserialize<string>(jsonReader);
					}
				}
			}
			catch
			{
				return default;
			}
			return default;
		}
	}
}