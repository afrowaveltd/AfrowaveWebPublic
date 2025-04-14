namespace Id.Api
{
	/// <summary>
	/// Check if the application name is unique
	/// </summary>
	/// <param name="applicationService">Service for application manager</param>
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json", "application/xml")]
	public class IsApplicationNameUnique(IApplicationsManager applicationService) : ControllerBase
	{
		private readonly IApplicationsManager _applicationService = applicationService;

		/// <summary>
		/// Check if the application name is unique
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Is Unique Response class</returns>
		/// <example>
		/// Example usage:
		/// GET /api/IsApplicationNameUnique/{name}
		/// Example response:
		/// {
		///   "isUnique": true
		///  }
		///  </example>
		[HttpGet]
		[Route("{name}")]
		public async Task<IActionResult> OnGetAsync(string name)
		{
			return Ok(new IsUniqueResponse(await _applicationService.IsNameUnique(name)));
		}

		private class IsUniqueResponse(bool unique)
		{
			/// <summary>
			/// Gets or sets a value indicating whether the name is unique
			/// </summary>
			public bool IsUnique { get; set; } = unique;
		}
	}
}