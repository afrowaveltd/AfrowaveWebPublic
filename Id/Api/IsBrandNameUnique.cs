namespace Id.Api
{
	/// <summary>
	/// Check if the brand name is unique
	/// </summary>
	/// <param name="brandService"></param>
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json", "application/xml")]
	public class IsBrandNameUnique(IBrandsManager brandService) : ControllerBase
	{
		private readonly IBrandsManager _brandService = brandService;

		/// <summary>
		/// Check if the brand name is unique
		/// </summary>
		/// <param name="name">The brand name for a test</param>
		/// <returns>IsUniqueResponse class with boolean IsUnique as the result</returns>
		/// <example>
		/// GET /api/IsBrandNameUnique/{name}
		/// Response:
		/// {
		///	   "isUnique": true
		///	 }
		///	 </example>
		[HttpGet]
		[Route("{name}")]
		public async Task<IActionResult> OnGetAsync(string name)
		{
			return Ok(new IsUniqueResponse(await _brandService.IsNameUnique(name)));
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