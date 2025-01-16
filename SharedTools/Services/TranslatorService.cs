﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedTools.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SharedTools.Services
{
	public class TranslatorService : ITranslatorService
	{
		private readonly HttpClient _client;
		private readonly ILogger<TranslatorService> _logger;
		private readonly string languagesEndpoint;
		private readonly string translateEndpoint;

		private JsonSerializerOptions options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
			ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
		};

		public TranslatorService(IConfiguration config, ILogger<TranslatorService> logger)
		{
			Translator translator = config.GetSection("Translator").Get<Translator>() ?? new Translator();
			languagesEndpoint = $"{translator.Host}/languages";
			translateEndpoint = $"{translator.Host}/translate";
			_client = new HttpClient();
			_logger = logger;
		}

		public async Task<string[]> GetSupportedLanguagesAsync()
		{
			try
			{
				HttpResponseMessage response = await _client.GetAsync(languagesEndpoint);
				_ = response.EnsureSuccessStatusCode();
				List<LibretranslateLanguage> supportedLanguages = await _client.GetFromJsonAsync<List<LibretranslateLanguage>>(languagesEndpoint, options) ?? new();
				if(supportedLanguages.Count == 0)
				{
					_logger.LogWarning("No supported languages found");
					return Array.Empty<string>();
				}
				return supportedLanguages.Select(language => language.Code).ToArray();
			}
			catch(HttpRequestException e)
			{
				_logger.LogError(e, "Error getting supported languages");
				return Array.Empty<string>();
			}
		}

		public async Task<ApiResponse<string>> TranslateAsync(string text, string sourceLanguage, string targetLanguage)
		{
			ApiResponse<string> returnValue = new ApiResponse<string>();
			int tries = 0;
			int maxTries = 20;
			bool success = false;
			while(tries < maxTries && success == false)
			{
				try
				{
					HttpRequestMessage request = new(HttpMethod.Post, translateEndpoint);
					request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
					 {
						  { "q", text },
						  { "source", sourceLanguage },
						  { "target", targetLanguage }
					 });
					HttpResponseMessage response = await _client.PostAsync(translateEndpoint, request.Content);
					_ = response.EnsureSuccessStatusCode();
					TranslateResponse translation = await response.Content.ReadFromJsonAsync<TranslateResponse>(options) ?? new();
					if(translation.TranslatedText == string.Empty)
					{
						_logger.LogWarning("No translation found");
						returnValue.Successful = false;
						returnValue.Message = "No translation found";
						returnValue.Data = text;
						return returnValue;
					}
					success = true;
					returnValue.Data = translation.TranslatedText;
					return returnValue;
				}
				catch(HttpRequestException)
				{
					_logger.LogWarning("Error translating text, retrying");
					await Task.Delay(5000);
					tries++;
				}
			}
			_logger.LogError("Error translating text");
			returnValue.Successful = false;
			returnValue.Message = "Error translating text: ";
			returnValue.Data = text;
			return returnValue;
		}

		public async Task<ApiResponse<TranslateResponse>> AutodetectSourceLanguageAndTranslateAsync(string text, string targetLanguage)
		{
			ApiResponse<TranslateResponse> returnValue = new ApiResponse<TranslateResponse>();
			try
			{
				HttpRequestMessage request = new(HttpMethod.Post, translateEndpoint);
				request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
					 {
						  { "q", text },
						  { "source", "auto"},
						  { "target", targetLanguage }
					 });
				HttpResponseMessage response = await _client.PostAsync(translateEndpoint, request.Content);
				_ = response.EnsureSuccessStatusCode();
				TranslateResponse translation = await response.Content.ReadFromJsonAsync<TranslateResponse>(options) ?? new();
				return new ApiResponse<TranslateResponse>
				{
					Successful = true,
					Data = translation
				};
			}
			catch(HttpRequestException e)
			{
				_logger.LogError(e, "Error translating text");
				returnValue.Successful = false;
				returnValue.Message = "Error translating text: " + e.Message;
				returnValue.Data = new TranslateResponse
				{
					TranslatedText = text
				};
				return returnValue;
			}
		}
	}
}