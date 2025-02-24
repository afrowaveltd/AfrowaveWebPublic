namespace Id.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class IsEmailUnique(IUsersManager userService,
		 IStringLocalizer<IsEmailUnique> _t) : ControllerBase
	{
		private readonly IUsersManager _userService = userService;
		private readonly IStringLocalizer<IsEmailUnique> t = _t;

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
			public bool isUnique { get; set; } = unique;
		}
	}
}