using Id.Models.ResultModels;

namespace Id.Api
{
	/// <summary>
	/// Controller responsible for generating the dynamic web app manifest.
	/// </summary>
	[Route("manifest.json")]
	[ApiController]
	public class ManifestController : ControllerBase
	{
		private readonly IApplicationsManager _applicationsManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ManifestController"/> class.
		/// </summary>
		/// <param name="applicationsManager">The application manager service providing icon links and other application metadata.</param>
		public ManifestController(IApplicationsManager applicationsManager)
		{
			_applicationsManager = applicationsManager;
		}

		/// <summary>
		/// Generates and returns a dynamic web app manifest in JSON format.
		/// This manifest defines app metadata such as name, icons, and display preferences.
		/// </summary>
		/// <returns>
		/// A JSON object representing the web app manifest.
		/// </returns>
		/// <response code="200">Returns the dynamically generated manifest.</response>
		/// <response code="500">If there was an error retrieving icon data.</response>
		[HttpGet]
		[ProducesResponseType(typeof(object), 200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetManifestAsync()
		{
			ImageLinksResult icons = await _applicationsManager.GetAuthenticatorImagesLinksAsync();

			var manifest = new
			{
				name = "Afrowave",
				short_name = "Afrowave",
				icons = new[]
				 {
						  new { src = Url.Content(icons.Png16), sizes = "16x16", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png32), sizes = "32x32", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png76), sizes = "76x76", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png120), sizes = "120x120", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png152), sizes = "152x152", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png180), sizes = "180x180", type = "image/png", purpose = "any" },
						  new { src = Url.Content(icons.Png192), sizes = "192x192", type = "image/png", purpose = "maskable" },
						  new { src = Url.Content(icons.Png512), sizes = "512x512", type = "image/png", purpose = "maskable" }
					 },
				theme_color = "#000000",
				background_color = "#ffffff",
				display = "standalone"
			};

			return new JsonResult(manifest);
		}
	}
}