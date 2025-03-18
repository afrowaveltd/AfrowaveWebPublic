/// <summary>
/// Middleware for handling custom error responses.
/// </summary>
public class CustomErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IWebHostEnvironment _env;
	private readonly ILogger<CustomErrorHandlingMiddleware> _logger;

	/// <summary>
	/// Initializes a new instance of the <see cref="CustomErrorHandlingMiddleware"/> class.
	/// </summary>
	/// <param name="next">Middleware delegate</param>
	/// <param name="env">Environment</param>
	/// <param name="logger">Logging function</param>
	public CustomErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<CustomErrorHandlingMiddleware> logger)
	{
		_next = next;
		_env = env;
		_logger = logger;
	}

	/// <summary>
	/// Middleware execution logic to handle custom error responses.
	/// </summary>
	/// <param name="context">Http context</param>
	/// <returns></returns>
	public async Task InvokeAsync(HttpContext context)
	{
		Stream originalBodyStream = context.Response.Body;
		try
		{
			await _next(context);

			if(context.Response.StatusCode == 404)
			{
				await HandleErrorResponse(context, 404);
			}
		}
		catch(Exception ex)
		{
			_logger.LogError(ex, "Error handling failed");
			await HandleErrorResponse(context, 500);
		}
		finally
		{
			context.Response.Body = originalBodyStream; // Restore the original response body
		}
	}

	private async Task HandleErrorResponse(HttpContext context, int statusCode)
	{
		if(!context.Response.HasStarted)
		{
			context.Response.Clear();
		}

		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/json";

		ErrorDetails errorDetails = CreateErrorDetails(statusCode, new Exception(), context);

		if(statusCode == 500 && _env.EnvironmentName != "Development")
		{
			LogErrorToFile(errorDetails);
		}

		if(context.Request.Headers["Accept"].ToString().Contains("text/html"))
		{
			context.Response.Redirect($"/Error?statusCode={statusCode}");
			return;
		}

		await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
	}

	/// <summary>
	/// Creates an error details object for the given status code and exception.
	/// </summary>
	/// <param name="statusCode">Error status code</param>
	/// <param name="exception">Exception message</param>
	/// <param name="context">HttpContext</param>
	/// <returns></returns>
	public ErrorDetails CreateErrorDetails(int statusCode, Exception exception, HttpContext context)
	{
		IStringLocalizer<CustomErrorHandlingMiddleware> localizer = context.RequestServices.GetRequiredService<IStringLocalizer<CustomErrorHandlingMiddleware>>();
		return new ErrorDetails
		{
			StatusCode = statusCode,
			Title = localizer != null ? GetLocalizedTitle(statusCode, localizer) : $"Error {statusCode}",
			Message = exception?.Message ?? (localizer != null ? GetLocalizedMessage(statusCode, localizer) : "An error occurred"),
			Details = exception?.ToString() ?? string.Empty,
			Timestamp = DateTime.UtcNow
		};
	}

	/// <summary>
	/// Gets the localized title for the given status code.
	/// </summary>
	/// <param name="statusCode">Status code</param>
	/// <param name="localizer">Localization service</param>
	/// <returns></returns>
	public string GetLocalizedTitle(int statusCode, IStringLocalizer<CustomErrorHandlingMiddleware> localizer)
	{
		return localizer[$"Error_{statusCode}_Title"] ?? localizer["Error_Generic_Title"];
	}

	/// <summary>
	/// Gets the localized message for the given status code.
	/// </summary>
	/// <param name="statusCode">Status code</param>
	/// <param name="localizer">Localization service</param>
	/// <returns></returns>
	public string GetLocalizedMessage(int statusCode, IStringLocalizer<CustomErrorHandlingMiddleware> localizer)
	{
		return localizer[$"Error_{statusCode}_Message"] ?? localizer["Error_Generic_Message"];
	}

	private void LogErrorToFile(ErrorDetails errorDetails)
	{
		try
		{
			string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
			if(!Directory.Exists(logDir))
			{
				_ = Directory.CreateDirectory(logDir);
			}
			string logFile = Path.Combine(logDir, "errors.log");
			File.AppendAllText(logFile, $"{DateTime.UtcNow}: {JsonSerializer.Serialize(errorDetails)}{Environment.NewLine}");
		}
		catch(Exception ex)
		{
			_logger.LogError(ex, "Failed to log error to file.");
		}
	}

	/// <summary>
	/// Checks if the client is disconnected.
	/// </summary>
	/// <param name="context">Http context</param>
	/// <returns>true if client is not connected</returns>
	public async Task<bool> IsClientDisconnectedAsync(HttpContext context)
	{
		try
		{
			return await Task.FromResult(context.RequestAborted.IsCancellationRequested);
		}
		catch(Exception ex)
		{
			_logger.LogError(ex, "Failed to check client connection status.");
			return await Task.FromResult(true);
		}
	}
}

/// <summary>
/// Error details object for custom error responses.
/// </summary>
public class ErrorDetails
{
	/// <summary>
	/// Error status code.
	/// </summary>
	public int StatusCode { get; set; }

	/// <summary>
	/// Error title.
	/// </summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Error message.
	/// </summary>
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// Error details.
	/// </summary>
	public string Details { get; set; } = string.Empty;

	/// <summary>
	/// Timestamp of the error.
	/// </summary>
	public DateTime Timestamp { get; set; }
}