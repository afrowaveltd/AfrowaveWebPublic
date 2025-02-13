using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;

namespace Id.Services
{
	public class ApplicationsManager(ApplicationDbContext context,
		IImageService imageService,
		IBrandsManager brandsManager,
		ISettingsService settings,
		IStringLocalizer<ApplicationsManager> t,
		ILogger<ApplicationsManager> logger,
		IEncryptionService encryptionService) : IApplicationsManager
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly IBrandsManager _brandsManager = brandsManager;
		private readonly IImageService _imageService = imageService;
		private readonly ILogger<ApplicationsManager> _logger = logger;
		private readonly ISettingsService _settings = settings;
		private readonly IStringLocalizer<ApplicationsManager> _t = t;
		private readonly IEncryptionService _encryptionService = encryptionService;

		// Private variables
		private readonly string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			.Substring(0, AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")), "wwwroot", "applications");

		private readonly string webImgDirectory = "/applications";

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
					? $"{webImgDirectory}/{applicationId}/icons/icon-16x16.png"
					: "/img/no-icon_16.png",
				LogoSize.png32px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-32x32.png"))
					? $"{webImgDirectory}/{applicationId}/icons/icon-32x32.png"
					: "/img/no-icon_32.png",
				LogoSize.png76px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-76x76.png"))
					? $"{webImgDirectory}/{applicationId}/icons/icon-76x76.png"
					: "/img/no-icon_76.png",
				LogoSize.png120px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-120x120.png"))
					? $"{webImgDirectory}/{applicationId}/icons/icon-120x120.png"
					: "/img/no-icon_120.png",
				LogoSize.png152px =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "icon-152x152.png"))
					? $"{webImgDirectory}/{applicationId}/icons/icon-152x152.png"
					: "/img/no-icon_152.png",
				_ =>
				File.Exists(Path.Combine(appImgDirectory, applicationId, "icons", "original-icon*.png"))
					? $"{webImgDirectory}/{applicationId}/icons/original-icon*.png"
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

		public async Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationInput input)
		{
			RegisterApplicationResult result = new();
			CheckInputResult formCheck = CheckApplicationInput(input);
			if(!formCheck.Success)
			{
				result.ApplicationCreated = false;
				result.LogoUploaded = false;
				result.ErrorMessage = string.Join(", ", formCheck.Errors);
				return result;
			}
			if(!await IsNameUnique(input.Name))
			{
				result.ApplicationCreated = false;
				result.LogoUploaded = false;
				result.ErrorMessage = _t["Application name is already in use"];
				return result;
			}
			if(!await _brandsManager.ValidBrandAndOwner(input.BrandId, input.OwnerId))
			{
				result.ApplicationCreated = false;
				result.LogoUploaded = false;
				result.ErrorMessage = _t["Invalid brand"];
				return result;
			}
			Application application = new();
			application.Id = Guid.NewGuid().ToString();
			application.Name = input.Name;
			application.Description = input.Description ?? string.Empty;
			application.ApplicationEmail = input.Email;
			application.ApplicationWebsite = input.Website;
			application.OwnerId = input.OwnerId;
			application.BrandId = input.BrandId;
			application.ApplicationPrivacyPolicy = input.PrivacyUrl ?? string.Empty;
			application.ApplicationTermsAndConditions = input.TermsUrl ?? string.Empty;
			application.ApplicationCookiesPolicy = input.CookiesUrl ?? string.Empty;
			application.RequireTerms = input.RequireTerms;
			application.RequirePrivacyPolicy = input.RequirePrivacyPolicy;
			application.RequireCookiePolicy = input.RequireCookiePolicy;
			application.Published = DateTime.UtcNow;
			application.IsEnabled = true;
			application.IsDeleted = false;
			application.Suspended = false;
			application.SuspendedById = string.Empty;
			application.ReasonForSuspension = string.Empty;

			_ = await _context.Applications.AddAsync(application);
			_ = await _context.SaveChangesAsync();
			result.ApplicationId = application.Id;
			result.ApplicationCreated = true;

			if(input.Icon != null)
			{
				List<ApiResponse<string>> resultUpload = await _imageService.CreateApplicationIcons(input.Icon, application.Id);
				if(resultUpload.Where(s => s.Successful == false).Any())
				{
					result.LogoUploaded = false;
					result.ErrorMessage = _t["Logo upload failed"];
				}
				else
				{
					result.LogoUploaded = true;
				}
			}
			return result;
		}

		public async Task<UpdateResult> UpdateApplicationAsync(UpdateApplicationInput input)
		{
			UpdateResult result = new();
			if(input == null)
			{
				_logger.LogError("UpdateApplicationAsync: input is null");
				result.Success = false;
				result.Errors.Add(_t["Missing application data"]);
				return result;
			}
			CheckInputResult checkInput = CheckApplicationInput(input);
			if(!checkInput.Success)
			{
				_logger.LogError("UpdateApplicationAsync: input is invalid");
				result.Errors.AddRange(checkInput.Errors);
				result.Success = false;
				return result;
			}
			if(!await ApplicationExistsAsync(input.ApplicationId))
			{
				_logger.LogError("UpdateApplicationAsync: application does not exist");
				result.Success = false;
				result.Errors.Add(_t["Application does not exist"]);
				return result;
			}

			if(!await _brandsManager.ValidBrandAndOwner(input.BrandId, input.OwnerId))
			{
				_logger.LogError("UpdateApplicationAsync: invalid brand");
				result.Success = false;
				result.Errors.Add(_t["Invalid brand"]);
				return result;
			}

			Application? application = await _context.Applications
				.Where(s => s.Id == input.ApplicationId)
				.FirstOrDefaultAsync();

			if(application == null)
			{
				_logger.LogError("UpdateApplicationAsync: application not found");
				result.Success = false;
				result.Errors.Add(_t["Application not found"]);
				return result;
			}

			if(application.OwnerId != input.OwnerId)
			{
				_logger.LogError("UpdateApplicationAsync: owner does not match");
				result.Success = false;
				result.Errors.Add(_t["Owner does not match"]);
				return result;
			}

			if(application.Name != input.Name)
			{
				if(!await IsNameUnique(input.Name))
				{
					_logger.LogError("UpdateApplicationAsync: name is not unique");
					result.Success = false;
					result.Errors.Add(_t["Application name is already in use"]);
					return result;
				}
				application.Name = input.Name;
				result.UpdatedValues.Add("Name", input.Name);
			}

			if(application.Description != input.Description)
			{
				application.Description = input.Description ?? string.Empty;
				result.UpdatedValues.Add("Description", input.Description ?? string.Empty);
			}
			if(application.ApplicationEmail != input.Email)
			{
				application.ApplicationEmail = input.Email;
				result.UpdatedValues.Add("Email", input.Email ?? string.Empty);
			}
			if(application.ApplicationWebsite != input.Website)
			{
				application.ApplicationWebsite = input.Website;
				result.UpdatedValues.Add("Website", input.Website ?? string.Empty);
			}
			if(application.ApplicationPrivacyPolicy != input.PrivacyUrl)
			{
				application.ApplicationPrivacyPolicy = input.PrivacyUrl ?? string.Empty;
				result.UpdatedValues.Add("PrivacyUrl", input.PrivacyUrl ?? string.Empty);
			}
			if(application.ApplicationTermsAndConditions != input.TermsUrl)
			{
				application.ApplicationTermsAndConditions = input.TermsUrl ?? string.Empty;
				result.UpdatedValues.Add("TermsUrl", input.TermsUrl ?? string.Empty);
			}
			if(application.ApplicationCookiesPolicy != input.CookiesUrl)
			{
				application.ApplicationCookiesPolicy = input.CookiesUrl ?? string.Empty;
				result.UpdatedValues.Add("CookiesUrl", input.CookiesUrl ?? string.Empty);
			}
			if(application.RequireTerms != input.RequireTerms)
			{
				application.RequireTerms = input.RequireTerms;
				result.UpdatedValues.Add("RequireTerms", input.RequireTerms.ToString());
			}
			if(application.RequirePrivacyPolicy != input.RequirePrivacyPolicy)
			{
				application.RequirePrivacyPolicy = input.RequirePrivacyPolicy;
				result.UpdatedValues.Add("RequirePrivacyPolicy", input.RequirePrivacyPolicy.ToString());
			}
			if(application.RequireCookiePolicy != input.RequireCookiePolicy)
			{
				application.RequireCookiePolicy = input.RequireCookiePolicy;
				result.UpdatedValues.Add("RequireCookiePolicy", input.RequireCookiePolicy.ToString());
			}
			try
			{
				_ = await _context.SaveChangesAsync();
				result.Success = true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "UpdateApplicationAsync: error saving changes");
				result.Success = false;
				result.Errors.Add(_t["Error saving changes"]);
			}
			return result;
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

			if(string.IsNullOrEmpty(input.Name))
			{
				_logger.LogError("CheckRegistration: Name is null");
				result.Success = false;
				result.Errors.Add(_t["Missing application name"]);
			}

			if(input.BrandId == 0)
			{
				_logger.LogError("CheckRegistration: BrandId is null");
				result.Success = false;
				result.Errors.Add(_t["Missing brand"]);
			}

			if(input.Name.Length < 2)
			{
				_logger.LogError("CheckRegistration: Name is too short");
				result.Success = false;
				result.Errors.Add(_t["Application name is too short"]);
			}

			if(input.Name.Length > 100)
			{
				_logger.LogError("CheckRegistration: Name is too long");
				result.Success = false;
				result.Errors.Add(_t["Application name is too long"]);
			}

			if(input.Description != null && input.Description.Length > 500)
			{
				_logger.LogError("CheckRegistration: Description is too long");
				result.Success = false;
				result.Errors.Add(_t["Description is too long"]);
			}

			if(input.Email != null && !Tools.IsEmailValid(input.Email))
			{
				_logger.LogError("CheckRegistration: Email is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid email address"]);
			}

			if(input.Website != null && !Tools.IsValidUrl(input.Website))
			{
				_logger.LogError("CheckRegistration: Website is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid website"]);
			}

			if(input.Icon != null && !Tools.IsImage(input.Icon))
			{
				_logger.LogError("CheckRegistration: Icon is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid icon"]);
			}

			if(input.PrivacyUrl != null && !Tools.IsValidUrl(input.PrivacyUrl))
			{
				_logger.LogError("CheckRegistration: PrivacyUrl is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid privacy policy URL"]);
			}

			if(input.TermsUrl != null && !Tools.IsValidUrl(input.TermsUrl))
			{
				_logger.LogError("CheckRegistration: TermsUrl is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid terms URL"]);
			}

			if(input.CookiesUrl != null && !Tools.IsValidUrl(input.CookiesUrl))
			{
				_logger.LogError("CheckRegistration: CookiesUrl is not valid");
				result.Success = false;
				result.Errors.Add(_t["Invalid cookies URL"]);
			}

			result.Success = true;
			return result;
		}
	}
}