namespace Id.Middlewares
{
	public class ErrorMiddleware(IErrorResponseService errorResponseService) : IMiddleware
	{
		private readonly IErrorResponseService _errorResponseService = errorResponseService;

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);

				if(context.Response.StatusCode is >= 400 and <= 510)
				{
					int errorCode = context.Response.StatusCode;
					await _errorResponseService.HandleErrorResponse(context, errorCode);
				}
			}
			catch(Exception)
			{
				await _errorResponseService.HandleErrorResponse(context, 500);
			}
		}
	}
}