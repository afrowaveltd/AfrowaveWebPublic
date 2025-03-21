using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedTools.Models;
using SharedTools.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Implementation of <see cref="ITranslatorService"/> using an HTTP translation API (e.g., LibreTranslate).
/// Handles both direct and auto-detected translations, and retrieves supported languages.
/// </summary>
public class TranslatorService : ITranslatorService
{
	private readonly IHttpService _httpService;
	private readonly ILogger<TranslatorService> _logger;
	private readonly string _languagesEndpoint;
	private readonly string _translateEndpoint;

	private readonly JsonSerializerOptions _options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never,
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};

	/// <summary>
	/// Initializes a new instance of <see cref="TranslatorService"/>.
	/// </summary>
	/// <param name="httpService">Injected HTTP service for making requests.</param>
	/// <param name="config">Configuration object providing translation API host.</param>
	/// <param name="logger">Logger instance for logging errors and warnings.</param>
	public TranslatorService(IHttpService httpService, IConfiguration config, ILogger<TranslatorService> logger)
	{
		_httpService = httpService;
		_logger = logger;

		Translator translator = config.GetSection("Translator").Get<Translator>() ?? new();
		_languagesEndpoint = $"{translator.Host}/languages";
		_translateEndpoint = $"{translator.Host}/translate";
	}

	/// <summary>
	/// Retrieves a list of supported languages from the translation API.
	/// </summary>
	/// <returns>array of strings or empty array</returns>
	public async Task<string[]> GetSupportedLanguagesAsync()
	{
		try
		{
			HttpResponseMessage response = await _httpService.GetAsync(_languagesEndpoint);
			_ = response.EnsureSuccessStatusCode();

			List<LibretranslateLanguage> languages = await _httpService.ReadJsonAsync<List<LibretranslateLanguage>>(response.Content, _options) ?? new();
			if(languages.Count == 0)
			{
				_logger.LogWarning("No supported languages found");
				return Array.Empty<string>();
			}

			return languages.Select(l => l.Code).ToArray();
		}
		catch(HttpRequestException e)
		{
			_logger.LogError(e, "Error getting supported languages");
			return Array.Empty<string>();
		}
	}

	/// <summary>
	/// Translates text from a source language to a target language.
	/// </summary>
	/// <param name="text">Text to translate</param>
	/// <param name="sourceLanguage">Source language code</param>
	/// <param name="targetLanguage">Target language code</param>
	/// <returns>Api response with string data representating the translation</returns>
	public async Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage)
	{
		_ = new ApiResponse<string>();
		int tries = 0;
		const int maxTries = 20;
		bool success = false;

		while(tries < maxTries && !success)
		{
			try
			{
				Dictionary<string, string> content = new Dictionary<string, string>
				{
					{ "q", text },
					{ "source", sourceLanguage },
					{ "target", targetLanguage }
				};

				HttpResponseMessage response = await _httpService.PostFormAsync(_translateEndpoint, content);
				_ = response.EnsureSuccessStatusCode();

				TranslateResponse translation = await _httpService.ReadJsonAsync<TranslateResponse>(response.Content, _options) ?? new();

				if(string.IsNullOrEmpty(translation.TranslatedText))
				{
					_logger.LogWarning("No translation found");
					return new ApiResponse<string> { Successful = false, Message = "No translation found", Data = text };
				}

				success = true;
				return new ApiResponse<string> { Data = translation.TranslatedText };
			}
			catch(HttpRequestException)
			{
				_logger.LogWarning("Error translating text, retrying");
				await Task.Delay(5000);
				tries++;
			}
		}

		_logger.LogError("Translation failed after retries");
		return new ApiResponse<string> { Successful = false, Message = "Translation failed", Data = text };
	}

	/// <summary>
	/// Translates text by auto-detecting the source language.
	/// </summary>
	/// <param name="text">Text to translate</param>
	/// <param name="targetLanguage">Target language code</param>
	/// <returns>ApiResponse with TranslateResponse containing the translation</returns>
	public async Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage)
	{
		try
		{
			Dictionary<string, string> content = new Dictionary<string, string>
			{
				{ "q", text },
				{ "source", "auto" },
				{ "target", targetLanguage }
			};

			HttpResponseMessage response = await _httpService.PostFormAsync(_translateEndpoint, content);
			_ = response.EnsureSuccessStatusCode();

			TranslateResponse translation = await _httpService.ReadJsonAsync<TranslateResponse>(response.Content, _options) ?? new();

			return new ApiResponse<TranslateResponse> { Successful = true, Data = translation };
		}
		catch(HttpRequestException e)
		{
			_logger.LogError(e, "Error translating text");
			return new ApiResponse<TranslateResponse>
			{
				Successful = false,
				Message = $"Error translating text: {e.Message}",
				Data = new TranslateResponse { TranslatedText = text }
			};
		}
	}
}