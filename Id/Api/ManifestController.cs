namespace Id.Api
{
	/// <summary>
	/// Handles requests for the web app manifest, returning a JSON object with app details and icon links. Ensures valid
	/// URLs and defaults if necessary.
	/// </summary>
	[Route("manifest.json")]
	[ApiController]
	public class ManifestController : ControllerBase
	{
		private readonly IApplicationsManager _applicationsManager;

		/// <summary>
		/// Initializes a new instance of the ManifestController class.
		/// </summary>
		/// <param name="applicationsManager">Provides access to application management functionalities.</param>
		public ManifestController(IApplicationsManager applicationsManager)
		{
			_applicationsManager = applicationsManager;
		}

		/// <summary>
		/// Retrieves a manifest object containing application details and icon links. It constructs the manifest based on
		/// available icon URLs.
		/// </summary>
		/// <returns>Returns a JsonResult containing the manifest object.</returns>
		[HttpGet]
		[ProducesResponseType(typeof(Manifest), 200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetManifestAsync()
		{
			ImageLinksResult icons = await _applicationsManager.GetAuthenticatorImagesLinksAsync();

			// Ensure icons are not null
			icons ??= new ImageLinksResult();

			// Ensure Url property is not null
			if(Url == null)
			{
				return StatusCode(500, "Url helper is not available.");
			}

			// Construct the manifest object
			Manifest manifest = new Manifest
			{
				Name = "Afrowave",
				ShortName = "Afrowave",
				Icons = new[]
				{
					new ManifestIcon { Src = icons.Png16 != null ? Url.Content(icons.Png16) : "", Sizes = "16x16", Type = "image/png" },
					new ManifestIcon { Src = icons.Png32 != null ? Url.Content(icons.Png32) : "", Sizes = "32x32", Type = "image/png" },
					new ManifestIcon { Src = icons.Png76 != null ? Url.Content(icons.Png76) : "", Sizes = "76x76", Type = "image/png" },
					new ManifestIcon { Src = icons.Png120 != null ? Url.Content(icons.Png120) : "", Sizes = "120x120", Type = "image/png" },
					new ManifestIcon { Src = icons.Png152 != null ? Url.Content(icons.Png152) : "", Sizes = "152x152", Type = "image/png" },
					new ManifestIcon { Src = icons.Png180 != null ? Url.Content(icons.Png180) : "", Sizes = "180x180", Type = "image/png" },
					new ManifestIcon { Src = icons.Png192 != null ? Url.Content(icons.Png192) : "", Sizes = "192x192", Type = "image/png" },
					new ManifestIcon { Src = icons.Png512 != null ? Url.Content(icons.Png512) : "", Sizes = "512x512", Type = "image/png" }
				}.Where(icon => !string.IsNullOrEmpty(icon.Src)).ToArray(),  // Ensure empty URLs are removed
				ThemeColor = "#000000",
				BackgroundColor = "#ffffff",
				Display = "standalone"
			};

			return new JsonResult(manifest);
		}

		/// <summary>
		/// Represents a manifest with properties for name, short name, icons, theme color, background color, and display
		/// type.
		/// </summary>
		public class Manifest
		{
			/// <summary>
			/// Represents the name of an entity. It can be both retrieved and modified.
			/// </summary>
			public string Name { get; set; } = string.Empty;

			/// <summary>
			/// Represents a short name as a string. It can be both retrieved and modified.
			/// </summary>
			public string ShortName { get; set; } = string.Empty;

			/// <summary>
			/// Represents an array of icons associated with a manifest. Each icon provides visual representation for the
			/// application.
			/// </summary>
			public ManifestIcon[] Icons { get; set; } = Array.Empty<ManifestIcon>();

			/// <summary>
			/// Represents the color theme of a user interface element. It is a string property that can be set or retrieved.
			/// </summary>
			public string ThemeColor { get; set; } = string.Empty;

			/// <summary>
			/// Gets or sets the background color as a string. It defines the color used for the background.
			/// </summary>
			public string BackgroundColor { get; set; } = string.Empty;

			/// <summary>
			/// Represents the display name of an object. It can be accessed and modified as a string property.
			/// </summary>
			public string Display { get; set; } = string.Empty;
		}

		// Define the ManifestIcon class
		/// <summary>
		/// Represents an icon for a web application manifest. Contains properties for the icon's source, size, and type.
		/// </summary>
		public class ManifestIcon
		{
			/// <summary>
			/// Represents the source as a string. It can be used to get or set the value of the source.
			/// </summary>
			public string Src { get; set; } = string.Empty;

			/// <summary>
			/// Represents a collection of sizes as a string. It can be used to store and retrieve size information.
			/// </summary>
			public string Sizes { get; set; } = string.Empty;

			/// <summary>
			/// Represents a type of the icon
			/// </summary>
			public string Type { get; set; } = string.Empty;
		}
	}
}