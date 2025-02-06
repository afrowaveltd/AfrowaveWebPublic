using System.Xml.Serialization;

public class CustomErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IWebHostEnvironment _env;
	private readonly ILogger<CustomErrorHandlingMiddleware> _logger;

	public CustomErrorHandlingMiddleware(
		 RequestDelegate next,
		 IWebHostEnvironment env,
		 ILogger<CustomErrorHandlingMiddleware> logger)
	{
		_next = next;
		_env = env;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		Stream originalBodyStream = context.Response.Body;

		try
		{
			using MemoryStream responseBody = new MemoryStream();
			context.Response.Body = responseBody;

			await _next(context);

			_ = responseBody.Seek(0, SeekOrigin.Begin);
			if(IsErrorStatusCode(context.Response.StatusCode))
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

	private async Task HandleErrorResponse(
		 HttpContext context,
		 MemoryStream? responseBody = null,
		 Stream originalBodyStream = null,
		 Exception exception = null)
	{
		try
		{
			int statusCode = context.Response.StatusCode;
			ResponseFormat formatter = GetResponseFormatter(context.Request);
			ErrorDetails errorDetails = CreateErrorDetails(statusCode, exception);

			if(statusCode == 500 && !_env.IsDevelopment())
			{
				LogProductionError(exception);
			}

			string responseContent = formatter switch
			{
				ResponseFormat.Json => FormatJson(errorDetails),
				ResponseFormat.Xml => FormatXml(errorDetails),
				ResponseFormat.Html => await FormatHtml(context, errorDetails),
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

	private ErrorDetails CreateErrorDetails(int statusCode, Exception exception)
	{
		return new ErrorDetails
		{
			StatusCode = statusCode,
			Title = $"Error {statusCode}",
			Message = exception?.Message ?? "An error occurred.",
			Details = _env.IsDevelopment() ? exception?.ToString() : null,
			Timestamp = DateTime.UtcNow
		};
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

	private async Task<string> FormatHtml(HttpContext context, ErrorDetails details)
	{
		return $"<html><body><h1>{details.StatusCode} - {details.Title}</h1><p>{details.Message}</p></body></html>";
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

public enum ResponseFormat
{ Json, Xml, Html, Text }

public static class ResponseFormatExtensions
{
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

public class ErrorDetails
{
	public int StatusCode { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
	public string Details { get; set; }
	public DateTime Timestamp { get; set; }
}