namespace Id.Pages.Test
{
	public class HttpErrorModel : PageModel
	{
		public IActionResult OnGet(int? id)
		{
			if(id == null)
			{
				return BadRequest("Status code is required. Use /ErrorTests/{statusCode}.");
			}

			// Generate a response with the given status code
			return id switch
			{
				400 => BadRequest("This is a 400 Bad Request error."),
				401 => Unauthorized(),
				403 => Forbid("This is a 403 Forbidden error."),
				404 => NotFound("This is a 404 Not Found error."),
				500 => StatusCode(500, "This is a 500 Internal Server Error."),
				_ => StatusCode(id.Value, $"This is a custom {id.Value} error."),
			};
		}
	}
}