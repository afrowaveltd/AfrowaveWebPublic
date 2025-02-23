using System.Xml.Serialization;

public class CustomErrorHandlingMiddleware(
		 RequestDelegate next,
		 IWebHostEnvironment env,
		 ILogger<CustomErrorHandlingMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly IWebHostEnvironment _env = env;
	private readonly ILogger<CustomErrorHandlingMiddleware> _logger = logger;

	public async Task InvokeAsync(HttpContext context)
	{
		Stream originalBodyStream = context.Response.Body;

		try
		{
			using MemoryStream responseBody = new MemoryStream();
			context.Response.Body = responseBody;

			await _next(context);

			responseBody.Seek(0, SeekOrigin.Begin);
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
			ErrorDetails errorDetails = CreateErrorDetails(statusCode, exception, context);

			if(formatter == ResponseFormat.Html)
			{
				context.Response.Redirect($"/Error?statusCode={statusCode}");
				return;
			}

			if(statusCode == 500 && !_env.IsDevelopment())
			{
				LogProductionError(exception);
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
		var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<CustomErrorHandlingMiddleware>>();
		return new ErrorDetails
		{
			StatusCode = statusCode,
			Title = GetLocalizedTitle(statusCode, localizer),
			Message = exception?.Message ?? GetLocalizedMessage(statusCode, localizer),
			Details = _env.IsDevelopment() ? exception?.ToString() : null,
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