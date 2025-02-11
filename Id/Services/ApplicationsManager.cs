using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;

namespace Id.Services
{
	public class ApplicationsManager(ApplicationDbContext context,
		IImageService imageService,
		ISettingsService settings,
		IStringLocalizer<ApplicationsManager> t,
		ILogger<ApplicationsManager> logger,
		IEncryptionService encryptionService)
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly IImageService _imageService = imageService;
		private readonly ILogger<ApplicationsManager> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IStringLocalizer<ApplicationsManager> _t = t;
		private readonly IEncryptionService _encryptionService = encryptionService;

		// Private variables
		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			.Substring(0, AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")), "wwwroot", "applications");

		// Public functions
		public async Task<bool> ApplicationExistsAsync(string applicationId)
		{
			return await _context.Applications.AnyAsync(s => s.Id == applicationId);
		}

		public async Task<ApplicationView?> GetInfoAsync(string applicationId)
		{
			Application? application = await _context.Applications
				.Include(s => s.Brand)
				.Where(s => s.Id == applicationId)
				.FirstOrDefaultAsync();
			if(application == null)
			{
				return null;
			}
			ApplicationView result = new();
			result.ApplicationId = application.Id;
			result.ApplicationName = application.Name;
			result.ApplicationDescription = application.Description ?? string.Empty;
			result.ApplicationWebsite = application.ApplicationWebsite ?? string.Empty;
			result.BrandName = application.Brand?.Name ?? string.Empty;
			return result;
		}

		public string GetFullsizeLogoPath(string applicationId)
		{
			return GetLogoPath(applicationId, LogoSize.pngOriginal);
		}

		public string GetIconPath(string applicationId)
		{
			return GetLogoPath(applicationId, LogoSize.png32px);
		}

		public string GetLogoPath(string applicationId, LogoSize size)
		{
			string logoPath = size switch
			{
				LogoSize.png16px =>
					File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-16x16.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "icon-16x16.png")
					: "/img/no-icon_16.png",
				LogoSize.png32px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-32x32.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "icon-32x32.png")
					: "/img/no-icon_32.png",
				LogoSize.png76px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-76x76.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "icon-76x76.png")
					: "/img/no-icon_76.png",
				LogoSize.png120px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-120x120.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "icon-120x120.png")
					: "/img/no-icon_120.png",
				LogoSize.png152px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-152x152.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "icon-152x152.png")
					: "/img/no-icon_152.png",
				_ =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "original-icon*.png"))
					? Path.Combine(appImgDirectory, applicationId, "icons", "original-icon*.png")
					: "/img/no-icon.png"
			};
			return logoPath;
		}

		public async Task<bool> IsNameUnique(string name)
		{
			return (!await _context.Applications
				.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim())
				.AnyAsync());
		}

		// Private functions
		private CheckInputResult CheckApplicationInput<T>(T input) where T : IApplicationInput
		{
			CheckInputResult result = new();
			if(input == null)
			{
				_logger.LogError("CheckRegistration: input is null");
				result.Success = false;
				result.Errors.Add(_t["Missing application data"]);
				return result;
			}
			if(string.IsNullOrEmpty(input.OwnerId))
			{
				_logger.LogError("CheckRegistration: OwnerId is null");
				result.Success = false;
				result.Errors.Add(_t["Missing owner"]);
			}
			return result;
		}
	}
}