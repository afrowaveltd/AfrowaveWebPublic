namespace Id.Pages.Error
{
	[Microsoft.AspNetCore.Mvc.Route("Error/{StatusCode}")]
	public class IndexModel(IStringLocalizer<IndexModel> _t) : PageModel
	{
		public readonly IStringLocalizer<IndexModel> t = _t;

		[BindProperty(SupportsGet = true)]
		public int StatusCode { get; set; }

		public IActionResult OnGet()
		{
			// Determine response type (JSON or HTML)
			var acceptHeader = Request.Headers["Accept"].ToString();
			if(acceptHeader.Contains("application/json"))
			{
				return new JsonResult(new
				{
					error = true,
					statusCode = StatusCode,
					message = GetErrorMessage(StatusCode)
				});
			}
			else
			{
				return Page();
			}
		}

		public string GetErrorMessage(int statusCode)
		{
			return statusCode switch
			{
				400 => t["Bad request"],
				401 => t["Unauthorized"],
				403 => t["Forbidden"],
				404 => t["Not found"],
				500 => t["Internal server error"],
				501 => t["Not implemented"],
				502 => t["Bad gateway"],
				503 => t["Service unavailable"],
				504 => t["Gateway timeout"],
				_ => t["Unknown error"]
			};
		}
	}
}