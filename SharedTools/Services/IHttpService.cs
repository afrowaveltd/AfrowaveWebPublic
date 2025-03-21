using System.Text.Json;

namespace SharedTools.Services;

/// <summary>
/// Interface for making HTTP requests in a flexible and testable way.
/// </summary>
public interface IHttpService
{
	/// <summary>
	/// Sends a raw HTTP request.
	/// </summary>
	/// <param name="request">The HTTP request message.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>The HTTP response.</returns>
	Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

	/// <summary>
	/// Reads and deserializes JSON content into a specific type.
	/// </summary>
	/// <typeparam name="T">Target deserialization type.</typeparam>
	/// <param name="content">HTTP content to read from.</param>
	/// <param name="options">Optional JSON serializer options.</param>
	/// <returns>Deserialized object or null.</returns>
	Task<T?> ReadJsonAsync<T>(HttpContent content, JsonSerializerOptions? options = null);

	/// <summary>
	/// Sends a GET request to the specified URL.
	/// </summary>
	/// <param name="url">The target URL.</param>
	/// <param name="headers">Optional request headers.</param>
	/// <returns>The HTTP response.</returns>
	Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Sends a POST request with JSON payload to the specified URL.
	/// </summary>
	/// <typeparam name="T">Type of the payload object.</typeparam>
	/// <param name="url">The target URL.</param>
	/// <param name="payload">The data object to send as JSON.</param>
	/// <param name="headers">Optional request headers.</param>
	/// <returns>The HTTP response.</returns>
	Task<HttpResponseMessage> PostJsonAsync<T>(string url, T payload, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Sends a POST request with form URL encoded content to the specified URL.
	/// </summary>
	/// <param name="url">The target URL.</param>
	/// <param name="formFields">Dictionary of form fields and values.</param>
	/// <param name="headers">Optional request headers.</param>
	/// <returns>The HTTP response.</returns>
	Task<HttpResponseMessage> PostFormAsync(string url, Dictionary<string, string> formFields, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Sends a POST request with multipart/form-data including files and form fields.
	/// </summary>
	/// <param name="url">The target URL.</param>
	/// <param name="formFields">Text form fields to include.</param>
	/// <param name="files">Collection of files to upload.</param>
	/// <param name="headers">Optional headers.</param>
	/// <returns>The HTTP response.</returns>
	Task<HttpResponseMessage> PostMultipartAsync(string url,
		Dictionary<string, string> formFields,
		IEnumerable<(string FieldName, string FileName, Stream Content, string ContentType)> files,
		Dictionary<string, string>? headers = null);
}