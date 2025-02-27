using System.Xml.Serialization;

/// <summary>
/// Initializes a new instance of the <see cref="CustomErrorHandlingMiddleware"/> class.
/// </summary>
/// <param name="next">The next middleware delegate.</param>
/// <param name="env">The hosting environment.</param>
/// <param name="logger">The logger instance.</param>
public class CustomErrorHandlingMiddleware(
		 RequestDelegate next,
		 IWebHostEnvironment env,
		 ILogger<CustomErrorHandlingMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly IWebHostEnvironment _env = env;
	private readonly ILogger<CustomErrorHandlingMiddleware> _logger = logger;

	/// <summary>
	/// Middleware execution logic.
	/// </summary>
	/// <param name="context">The HTTP context.</param>
	public async Task InvokeAsync(HttpContext context)
	{
		Stream originalBodyStream = context.Response.Body;

		try
		{
			using MemoryStream responseBody = new MemoryStream();
			context.Response.Body = responseBody;

			await _next(context);

			_ = responseBody.Seek(0, SeekOrigin.Begin);
			if(IsErrorStatusCode(context.Response.StatusCode) || context.Response.StatusCode == 404)
			{
				await HandleErrorResponse(context, responseBody, originalBodyStream);
			}
			else if(responseBody.Length > 0)
			{
				await responseBody.CopyToAsync(originalBodyStream);
			}
		}
		catch(Exception ex)
		{
			await HandleExceptionAsync(context, originalBodyStream, ex);
		}
	}

	private bool IsErrorStatusCode(int statusCode)
		 => statusCode >= 400 && statusCode < 600;

	private async Task HandleExceptionAsync(HttpContext context, Stream originalBodyStream, Exception exception)
	{
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		await HandleErrorResponse(context, null, originalBodyStream, exception);
	}

	/// <summary>
	/// Handles error responses based on the request format.
	/// </summary>
	private async Task HandleErrorResponse(
		 HttpContext context,
		 MemoryStream? responseBody = null,
		 Stream? originalBodyStream = null,
		 Exception? exception = null)
	{
		try
		{
			int statusCode = context.Response.StatusCode;
			ResponseFormat formatter = GetResponseFormatter(context.Request);
			ErrorDetails errorDetails = CreateErrorDetails(statusCode, exception ?? new(), context);

			if(formatter == ResponseFormat.Html)
			{
				context.Response.Redirect($"/Error?statusCode={statusCode}");
				return;
			}

			if(statusCode == 500 && !_env.IsDevelopment())
			{
				LogProductionError(exception ?? new());
			}

			string responseContent = formatter switch
			{
				ResponseFormat.Json => FormatJson(errorDetails),
				ResponseFormat.Xml => FormatXml(errorDetails),
				_ => FormatText(errorDetails)
			};

			context.Response.Clear();
			context.Response.ContentType = formatter.GetContentType();
			await context.Response.WriteAsync(responseContent);
			context.Response.Body = originalBodyStream;
		}
		catch(Exception ex)
		{
			_logger.LogError(ex, "Error handling failed");
		}
	}

	private string FormatJson(ErrorDetails details) =>
		 JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });

	private string FormatXml(ErrorDetails details)
	{
		using StringWriter writer = new StringWriter();
		XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
		serializer.Serialize(writer, details);
		return writer.ToString();
	}

	private string FormatText(ErrorDetails details) =>
		 $"{details.Title} ({details.StatusCode})\n{details.Message}\n{details.Details}";

	private ErrorDetails CreateErrorDetails(int statusCode, Exception exception, HttpContext context)
	{
		IStringLocalizer<CustomErrorHandlingMiddleware> localizer = context.RequestServices.GetRequiredService<IStringLocalizer<CustomErrorHandlingMiddleware>>();
		return new ErrorDetails
		{
			StatusCode = statusCode,
			Title = GetLocalizedTitle(statusCode, localizer),
			Message = exception?.Message ?? GetLocalizedMessage(statusCode, localizer),
			Details = exception?.ToString() ?? string.Empty,
			Timestamp = DateTime.UtcNow
		};
	}

	private string GetLocalizedTitle(int statusCode, IStringLocalizer<CustomErrorHandlingMiddleware> localizer)
	{
		return localizer[$"Error_{statusCode}_Title"] ?? localizer["Error_Generic_Title"];
	}

	private string GetLocalizedMessage(int statusCode, IStringLocalizer<CustomErrorHandlingMiddleware> localizer)
	{
		return localizer[$"Error_{statusCode}_Message"] ?? localizer["Error_Generic_Message"];
	}

	private ResponseFormat GetResponseFormatter(HttpRequest request)
	{
		string acceptHeader = request.Headers.Accept.ToString();

		return acceptHeader switch
		{
			string h when h.Contains("text/html") => ResponseFormat.Html,
			string h when h.Contains("application/xml") => ResponseFormat.Xml,
			string h when h.Contains("application/json") => ResponseFormat.Json,
			_ => ResponseFormat.Text
		};
	}

	private void LogProductionError(Exception exception)
	{
		try
		{
			string logDir = Path.Combine(_env.ContentRootPath, "logs");
			_ = Directory.CreateDirectory(logDir);

			string logFile = Path.Combine(logDir, $"{Guid.NewGuid()}.log");
			File.WriteAllText(logFile, $"[{DateTime.UtcNow:u}] {exception}");
		}
		catch(Exception ex)
		{
			_logger.LogError(ex, "Failed to log error");
		}
	}
}

/// <summary>
/// Enumeration of supported response formats.
/// </summary>
public enum ResponseFormat
{
	/// <summary>
	/// JSON format.
	/// </summary>
	Json,

	/// <summary>
	/// XML format.
	/// </summary>
	Xml,

	/// <summary>
	/// HTML format.
	/// </summary>
	Html,

	/// <summary>
	/// Text format.
	/// </summary>
	Text
}

/// <summary>
/// Extension methods for <see cref="ResponseFormat"/>.
/// </summary>
public static class ResponseFormatExtensions
{
	/// <summary>
	/// Gets the content type based on the response format.
	/// </summary>
	/// <param name="format">The response format.</param>
	/// <returns>The corresponding content type.</returns>
	public static string GetContentType(this ResponseFormat format)
	{
		return format switch
		{
			ResponseFormat.Json => "application/json",
			ResponseFormat.Xml => "application/xml",
			ResponseFormat.Html => "text/html",
			_ => "text/plain"
		};
	}
}

/// <summary>
/// Represents the details of an error response.
/// </summary>
public class ErrorDetails
{
	/// <summary>
	/// The status code of the response.
	/// </summary>
	public int StatusCode { get; set; }

	/// <summary>
	/// The title of the error.
	/// </summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// The message of the error.
	/// </summary>
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// The details of the error.
	/// </summary>

	public string Details { get; set; } = string.Empty;

	/// <summary>
	/// The timestamp of the error.
	/// </summary>
	public DateTime Timestamp { get; set; }
}