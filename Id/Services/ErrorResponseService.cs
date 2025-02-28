namespace Id.Services
{
	/// <summary>
	/// Service for handling error responses
	/// </summary>
	/// <param name="_t">Localizer</param>
	/// <param name="logger">Logger service</param>
	public class ErrorResponseService(IStringLocalizer<ErrorResponseService> _t, ILogger<ErrorResponseService> logger) : IErrorResponseService
	{
		private readonly IStringLocalizer<ErrorResponseService> t = _t;
		private readonly ILogger<ErrorResponseService> _logger = logger;

		/// <summary>
		/// Handle an error response
		/// </summary>
		/// <param name="context">Http context</param>
		/// <param name="errorCode">Error code</param>
		/// <returns>Proper response based on the type of an error</returns>
		public async Task HandleErrorResponse(HttpContext context, int errorCode)
		{
			string? acceptHeader = context.Request.Headers["Accept"].FirstOrDefault();

			LocalizedString errorMessage = errorCode switch
			{
				400 => t["Bad Request"],
				401 => t["Unauthorized"],
				403 => t["Forbidden"],
				404 => t["Not Found"],
				500 => t["Internal Server Error"],
				_ => t["An unknown error occurred."]
			};

			if(acceptHeader?.Contains("application/json") == true)
			{
				context.Response.ContentType = "application/json";
				var response = new
				{
					StatusCode = errorCode,
					Error = errorMessage
				};
				await context.Response.WriteAsJsonAsync(response);
			}
			else if(acceptHeader?.Contains("text/html") == true)
			{
				context.Response.Redirect($"/Error/{errorCode}");
			}
			else if(acceptHeader?.Contains("text/xml") == true || acceptHeader?.Contains("application/xml") == true)
			{
				context.Response.ContentType = "application/xml";
				string xmlResponse = $@"
                    <ErrorResponse>
                        <StatusCode>{errorCode}</StatusCode>
                        <Error>{errorMessage}</Error>
                    </ErrorResponse>";
				await context.Response.WriteAsync(xmlResponse);
			}
			else
			{
				try
				{
					context.Response.ContentType = "text/plain";
					await context.Response.WriteAsync($"Error {errorCode}: {errorMessage}");
				}
				catch(Exception ex)
				{
					_logger.LogWarning("Navigation error {error}", ex.Message);
				}
			}
		}
	}
}