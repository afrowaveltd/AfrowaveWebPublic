namespace Id.Services
{
	public class ErrorResponseService(IStringLocalizer<ErrorResponseService> _t) : IErrorResponseService
	{
		private readonly IStringLocalizer<ErrorResponseService> t = _t;

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
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync($"Error {errorCode}: {errorMessage}");
			}
		}
	}
}