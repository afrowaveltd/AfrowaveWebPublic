namespace Id.Api
{
	[Route("api/[controller]")]
	[ApiController]
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
			public bool IsUnique { get; set; } = unique;
		}
	}
}