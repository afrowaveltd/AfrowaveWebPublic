namespace Id.Pages
{
	public class ErrorModel(IStringLocalizer<ErrorModel> _t) : PageModel
	{
		public IStringLocalizer<ErrorModel> t = _t;

		[BindProperty(SupportsGet = true)]
		public int? ErrorCode { get; set; }

		public void OnGet(int? code)
		{
			ErrorCode = code;
		}
		public string GetErrorMessage()
		{
			return ErrorCode switch
			{
				400 => t["Bad Request"],
				401 => t["Unauthorized"],
				403 => t["Forbidden"],
				404 => t["Not Found"],
				500 => t["Internal Server Error"],
				_ => t["An unknown error occurred."]
			};
		}
	}
}