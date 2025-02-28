/// <summary>
/// The error page model
/// </summary>
/// <param name="_t"></param>
public class ErrorModel(IStringLocalizer<ErrorModel> _t) : PageModel
{
	private readonly IStringLocalizer<ErrorModel> t = _t;

	/// <summary>
	/// The status code
	/// </summary>
	[BindProperty(SupportsGet = true)]
	public new int StatusCode { get; set; } = 500;

	/// <summary>
	/// The title of the error
	/// </summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// The message of the error
	/// </summary>
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// Get request for the page
	/// </summary>
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