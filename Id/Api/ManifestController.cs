using Id.Models.ResultModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Id.Api
{
	[Route("manifest.json")]
	[ApiController]
	public class ManifestController : ControllerBase
	{
		private readonly IApplicationsManager _applicationsManager;

		public ManifestController(IApplicationsManager applicationsManager)
		{
			_applicationsManager = applicationsManager;
		}

		[HttpGet]
		[ProducesResponseType(typeof(Manifest), 200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> GetManifestAsync()
		{
			ImageLinksResult icons = await _applicationsManager.GetAuthenticatorImagesLinksAsync();

			// Ensure icons are not null
			icons ??= new ImageLinksResult();

			// Ensure Url property is not null
			if (Url == null)
			{
				return StatusCode(500, "Url helper is not available.");
			}

			// Construct the manifest object
			var manifest = new Manifest
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

		// Define the Manifest class
		public class Manifest
		{
			public string Name { get; set; }
			public string ShortName { get; set; }
			public ManifestIcon[] Icons { get; set; }
			public string ThemeColor { get; set; }
			public string BackgroundColor { get; set; }
			public string Display { get; set; }
		}

		// Define the ManifestIcon class
		public class ManifestIcon
		{
			public string Src { get; set; }
			public string Sizes { get; set; }
			public string Type { get; set; }
		}
	}
}