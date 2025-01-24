
namespace Id.Services
{
	public interface IErrorResponseService
	{
		Task HandleErrorResponse(HttpContext context, int errorCode);
	}
}