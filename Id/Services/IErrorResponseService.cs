namespace Id.Services
{
	/// <summary>
	/// Service to handle error responses.
	/// </summary>
	public interface IErrorResponseService
	{
		/// <summary>
		/// Handle an error response.
		/// </summary>
		/// <param name="context">HTTP Context</param>
		/// <param name="errorCode">Error status code</param>
		/// <returns></returns>
		Task HandleErrorResponse(HttpContext context, int errorCode);
	}
}