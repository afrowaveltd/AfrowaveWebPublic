namespace Id.Api
{
	/// <summary>
	/// Check if the email is unique
	/// </summary>
	/// <param name="userService">User manager service</param>
	/// <param name="_t">Localization service</param>
	[Route("api/[controller]")]
	[ApiController]
	public class IsEmailUnique(IUsersManager userService,
		 IStringLocalizer<IsEmailUnique> _t) : ControllerBase
	{
		private readonly IUsersManager _userService = userService;
		private readonly IStringLocalizer<IsEmailUnique> t = _t;

		/// <summary>
		/// Check if the email is unique
		/// </summary>
		/// <param name="email"></param>
		/// <returns>IsUniqueResponse</returns>
		[HttpGet]
		[Route("{email}")]
		public async Task<IActionResult> OnGetAsync(string email)
		{
			if(!Tools.IsEmailValid(email))
			{
				return BadRequest(t["Invalid email address"].Value);
			}

			return Ok(new IsUniqueResponse(await _userService.IsEmailFreeAsync(email)));
		}

		private class IsUniqueResponse(bool unique)
		{
			/// <summary>
			/// Gets or sets a value indicating whether the email is unique
			/// </summary>
			public bool isUnique { get; set; } = unique;
		}
	}
}