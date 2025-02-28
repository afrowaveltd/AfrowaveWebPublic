namespace Id.Pages.Helps
{
	/// <summary>
	/// The brand description is a short text that describes your company.
	/// </summary>
	/// <param name="_t"></param>
	public class Install_BrandDescriptionModel(IStringLocalizer<Install_BrandDescriptionModel> _t) : PageModel
	{
		private readonly IStringLocalizer<Install_BrandDescriptionModel> t = _t;

		/// <summary>
		/// The brand description is a short text that describes your company.
		/// </summary>
		public string Title => t["Brand description"];

		/// <summary>
		/// The brand description is a short text that describes your company.
		/// </summary>
		public List<string> Lines = [];

		/// <summary>
		/// The brand description is a short text that describes your company.
		/// </summary>
		public void OnGet()
		{
			Lines.Add(t["The Brand description is a short text that describes your company."]);
			Lines.Add(t["The description is not required, but it is recommended to fill it in."]);
		}
	}
}