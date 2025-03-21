using System.Net.Http.Json;
using System.Text.Json;

namespace SharedTools.Services
{
	/// <summary>
	/// Default implementation of <see cref="IHttpService"/> using <see cref="HttpClient"/>.
	/// Provides methods for sending HTTP requests and handling JSON or form-encoded content.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of <see cref="HttpService"/>.
	/// </remarks>
	/// <param name="client">The injected <see cref="HttpClient"/> instance.</param>
	public class HttpService(HttpClient client) : IHttpService
	{
		private readonly HttpClient _client = client;

		/// <inheritdoc />
		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
			=> _client.SendAsync(request, cancellationToken);

		/// <inheritdoc />
		public Task<T?> ReadJsonAsync<T>(HttpContent content, JsonSerializerOptions? options = null)
			=> content.ReadFromJsonAsync<T>(options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web));

		/// <inheritdoc />
		public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
			AddHeaders(request, headers);
			return await _client.SendAsync(request);
		}

		/// <inheritdoc />
		public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T payload, Dictionary<string, string>? headers = null)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = JsonContent.Create(payload)
			};
			AddHeaders(request, headers);
			return await _client.SendAsync(request);
		}

		/// <inheritdoc />
		public async Task<HttpResponseMessage> PostFormAsync(string url, Dictionary<string, string> formFields, Dictionary<string, string>? headers = null)
		{
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new FormUrlEncodedContent(formFields)
			};
			AddHeaders(request, headers);
			return await _client.SendAsync(request);
		}

		/// <inheritdoc />
		public async Task<HttpResponseMessage> PostMultipartAsync(string url,
	Dictionary<string, string> formFields,
	IEnumerable<(string FieldName, string FileName, Stream Content, string ContentType)> files,
	Dictionary<string, string>? headers = null)
		{
			using MultipartFormDataContent content = new MultipartFormDataContent();

			// Add form fields
			foreach(KeyValuePair<string, string> field in formFields)
			{
				content.Add(new StringContent(field.Value), field.Key);
			}

			// Add files
			foreach((string FieldName, string FileName, Stream Content, string ContentType) file in files)
			{
				StreamContent streamContent = new StreamContent(file.Content);
				streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
				content.Add(streamContent, file.FieldName, file.FileName);
			}

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = content
			};

			AddHeaders(request, headers);
			return await _client.SendAsync(request);
		}

		/// <summary>
		/// Adds headers to the HTTP request if any are provided.
		/// </summary>
		/// <param name="request">The <see cref="HttpRequestMessage"/> to modify.</param>
		/// <param name="headers">Dictionary of header key-value pairs.</param>
		private void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
		{
			if(headers == null)
			{
				return;
			}

			foreach((string key, string value) in headers)
			{
				_ = request.Headers.TryAddWithoutValidation(key, value);
			}
		}
	}
}