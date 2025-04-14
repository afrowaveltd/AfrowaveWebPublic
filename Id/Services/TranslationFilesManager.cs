using System.IO.Compression;
using System.Text;

namespace Id.Services
{
	/// <summary>
	/// This class is responsible for managing translation files.
	/// </summary>
	/// <param name="applicationsManager">The applications manager service</param>
	/// <param name="env">Environmental info</param>
	/// <param name="logger">Logger</param>
	/// <param name="t">Localizations</param>
	public class TranslationFilesManager(IApplicationsManager applicationsManager,
		IWebHostEnvironment env,
		ILogger<TranslationFilesManager> logger,
		IStringLocalizer<TranslationFilesManager> t)
		: ITranslationFilesManager
	{
		private readonly IApplicationsManager _applicationsManager = applicationsManager;
		private readonly IWebHostEnvironment _env = env;
		private readonly ILogger<TranslationFilesManager> _logger = logger;
		private readonly IStringLocalizer<TranslationFilesManager> _t = t;

		/// <summary>
		/// Path to the locales directory.
		/// </summary>
		public static string LocalesPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Locales");

		/// <summary>
		/// Path to the translations directory.
		/// </summary>
		public static string TranslationsRootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0], "Translations");

		/// <summary>
		/// Get the translation file for a specific language.
		/// </summary>
		/// <param name="applicationId">The application ID for the file - if empty or null data for Authenticator are being sent</param>
		/// <param name="lang">Two digits language code</param>
		/// <returns>ApiResponse with Data containing the dictionary of translations for selected language</returns>
		public async Task<ApiResponse<Dictionary<string, string>>> GetTranslationAsync(string? applicationId, string lang)
		{
			ApiResponse<Dictionary<string, string>> response = new();
			string basePath = await ResolveBasePath(applicationId);
			string filePath = Path.Combine(basePath, $"{lang}.json");

			if(lang.Length != 2)
			{
				response.Successful = false;
				response.Message = _t["Invalid language code"];
				return response;
			}

			if(!File.Exists(filePath))
			{
				response.Successful = false;
				response.Message = _t["Translation file not found"];
				response.Data = [];
				return response;
			}
			try
			{
				using FileStream fs = File.OpenRead(filePath);
				Dictionary<string, string> dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs)
							  ?? new();
				response.Data = dict;
				response.Successful = true;
				return response;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error reading translation file {filePath}", filePath);
				response.Successful = false;
				response.Message = _t["Error reading translation file"];
				response.Data = [];
				return response;
			}
		}

		/// <summary>
		/// Get all translations for a specific application.
		/// </summary>
		/// <param name="applicationId">The application Id - if empty dictionaries for Authenticator are sent</param>
		/// <returns>Dictionary with inner Dictionary for each translation</returns>
		public async Task<ApiResponse<Dictionary<string, Dictionary<string, string>>>> GetAllTranslationsAsync(string? applicationId)
		{
			ApiResponse<Dictionary<string, Dictionary<string, string>>> response = new();
			string basePath = await ResolveBasePath(applicationId);
			if(!Directory.Exists(basePath))
			{
				response.Successful = false;
				response.Message = _t["Translations directory not found"];
				response.Data = [];
				return response;
			}
			try
			{
				string[] files = Directory.GetFiles(basePath, "*.json");
				Dictionary<string, Dictionary<string, string>> dict = [];
				foreach(string file in files)
				{
					if(file.EndsWith("old.json", StringComparison.OrdinalIgnoreCase))
					{
						continue; // Skip the default English file
					}

					string lang = Path.GetFileNameWithoutExtension(file);
					using FileStream fs = File.OpenRead(file);
					dict[lang] = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs)
								  ?? [];
				}
				response.Data = dict;
				response.Successful = true;
				return response;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error reading translation files from {basePath}", basePath);
				response.Successful = false;
				response.Message = _t["Error reading translation files"];
				response.Data = [];
				return response;
			}
		}

		/// <summary>
		/// Export translations as a ZIP file.
		/// </summary>
		/// <param name="appId">Application Id</param>
		/// <param name="languages">Languages to be exported. If empty or null, all translations will be exported</param>
		/// <returns>Binary zip file</returns>
		public async Task<ApiResponse<byte[]>> ExportTranslationsAsZipAsync(string? appId, List<string>? languages = null)
		{
			ApiResponse<byte[]> response = new();

			try
			{
				string basePath = await ResolveBasePath(appId);

				if(!Directory.Exists(basePath))
				{
					response.Successful = false;
					response.Message = _t["Translation folder not found"];
					return response;
				}

				MemoryStream zipStream = new();

				using(ZipArchive archive = new(zipStream, ZipArchiveMode.Create, leaveOpen: true))
				{
					foreach(string file in Directory.EnumerateFiles(basePath, "*.json"))
					{
						string lang = Path.GetFileNameWithoutExtension(file);

						if(lang == "old")
						{
							continue; // Skip history file
						}

						if(languages != null && !languages.Contains(lang, StringComparer.OrdinalIgnoreCase))
						{
							continue;
						}

						string content = await File.ReadAllTextAsync(file);
						ZipArchiveEntry entry = archive.CreateEntry($"{lang}.json");

						await using StreamWriter writer = new(entry.Open(), Encoding.UTF8);
						await writer.WriteAsync(content);
					}
				}

				_ = zipStream.Seek(0, SeekOrigin.Begin);
				response.Data = zipStream.ToArray();
			}
			catch(Exception ex)
			{
				response.Successful = false;
				response.Message = _t["ZIP export failed"] + ": " + ex.Message;
				_logger.LogError(ex, "Error while creating ZIP for translations");
			}

			return response;
		}

		/// <summary>
		/// Resolve the base path for the translations.
		/// </summary>
		/// <param name="applicationId">Application Id</param>
		/// <returns>Path where translation files are located</returns>

		public async Task<string> ResolveBasePath(string? applicationId)
		{
			return (string.IsNullOrEmpty(applicationId) || applicationId == await _applicationsManager.GetAuthenticatorIdAsync())
				? LocalesPath
				: Path.Combine(TranslationsRootPath, applicationId);
		}
	}
}