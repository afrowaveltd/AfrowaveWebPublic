public class ErrorModel(IStringLocalizer<ErrorModel> _t) : PageModel
{
	private readonly IStringLocalizer<ErrorModel> t = _t;

	[BindProperty(SupportsGet = true)]
	public int StatusCode { get; set; }

	public string Title { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public void OnGet()
	{
		Message = StatusCode switch
		{
			400 => t["Error_400_Message"],
			401 => t["Error_401_Message"],
			403 => t["Error_403_Message"],
			404 => t["Error_404_Message"],
			500 => t["Error_500_Message"],
			_ => t["Error_Generic_Message"]
		};
		Title = StatusCode switch
		{
			400 => t["Error_400_Title"],
			401 => t["Error_401_Title"],
			403 => t["Error_403_Title"],
			404 => t["Error_404_Title"],
			500 => t["Error_500_Title"],
			_ => t["Error_Generic_Title"]
		};
	}
}