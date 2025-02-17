﻿using Id.Models.CommunicationModels;
using Id.Models.SettingsModels;
using SharedTools.Services;

namespace Id.Services
{
	public class ApplicationService(ApplicationDbContext context,
	  IImageService imageService,
	  IEncryptionService encryptionService,
		ISettingsService settings,
	  ILogger<ApplicationService> logger) : IApplicationService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IImageService _imageService = imageService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly ILogger<ApplicationService> _logger = logger;
		private readonly ISettingsService _settings = settings;

		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			 .Substring(0, AppDomain.CurrentDomain.BaseDirectory
			 .IndexOf("bin")), "wwwroot");

		// public functions
		public async Task<bool> IsApplicationNameUnique(string name)
		{
			return (!await _context.Applications.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim()).AnyAsync());
		}

		public async Task<ApplicationPublicInfo?> GetPublicInfoAsync(string applicationId)
		{
			Application? application = await _context.Applications.Include(s => s.Brand).Where(s => s.Id == applicationId).FirstOrDefaultAsync();
			if(application == null)
			{
				return null;
			}
			ApplicationPublicInfo result = new();
			result.ApplicationId = application.Id;
			result.ApplicationName = application.Name;
			result.ApplicationDescription = application.Description;
			result.ApplicationWebsite = application.ApplicationWebsite ?? string.Empty;
			result.ApplicationLogoUrl = GetApplicationIconPath(application.Id, LogoSize.png32px);
			result.BrandName = application.Brand?.Name ?? string.Empty;

			return result;
		}

		public string GetApplicationIconPath(string applicationId, LogoSize size)
		{
			string noLogo = "/img/no-icon.png";
			if(string.IsNullOrEmpty(applicationId))
			{ return noLogo; }
			string filePath = size switch
			{
				LogoSize.png16px => Path.Combine("applications", applicationId, "icons", "icon-16x16.png"),
				LogoSize.png32px => Path.Combine("applications", applicationId, "icons", "icon-32x32.png"),
				LogoSize.png76px => Path.Combine("applications", applicationId, "icons", "icon-76x76.png"),
				LogoSize.png120px => Path.Combine("applications", applicationId, "icons", "icon-120x120.png"),
				LogoSize.png152px => Path.Combine("applications", applicationId, "icons", "icon-152x152.png"),
				_ => Path.Combine("applications", applicationId, "icons", "original-icon*.png"),
			};
			if(File.Exists(Path.Combine(appImgDirectory, filePath)))
			{
				if(size == LogoSize.pngOriginal)
				{
					return $"/applications/{applicationId}/icons/original-icon-*.png";
				}
				else
				{
					string fileName = size switch
					{
						LogoSize.png16px => "icon-16x16.png",
						LogoSize.png32px => "icon-32x32.png",
						LogoSize.png76px => "icon-76x76.png",
						LogoSize.png120px => "icon-120x120.png",
						LogoSize.png152px => "icon-152x152.png",
						_ => "icon-*.png"
					};
					return $"/applications/{applicationId}/icons/{fileName}";
				}
			}
			else
			{
				return noLogo;
			}
		}

		public string GetApplicationIconPath(string applicationId)
		{
			return GetApplicationIconPath(applicationId, LogoSize.png32px);
		}

		public string GetApplicationImagePath(string applicationId)
		{
			return GetApplicationIconPath(applicationId, LogoSize.pngOriginal);
		}

		public async Task<string> CheckApplicationId(string applicationId)
		{
			if(Guid.TryParse(applicationId, out Guid id))
			{
				if(await _context.Applications.FindAsync(id) != null)
				{
					return applicationId;
				}
			}
			return string.Empty;
		}

		public async Task<string> GetDefaultApplicationId()
		{
			IdentificatorSettings settings = await _settings.GetSettingsAsync();
			return settings.ApplicationId;
		}

		public async Task<RegisterApplicationResult> RegisterApplicationAsync(RegisterApplicationModel input)
		{
			RegisterApplicationResult result = new();
			if(await IsApplicationNameUnique(input.Name))
			{
				Application application = new()
				{
					Name = input.Name,
					Description = input.Description ?? string.Empty,
					ApplicationEmail = input.Email,
					ApplicationWebsite = input.Website,
					OwnerId = input.OwnerId,
					BrandId = input.BrandId,
					IsEnabled = true,
					RequireTerms = input.RequireTerms,
					RequireCookiePolicy = input.RequireCookiePolicy,
					RequirePrivacyPolicy = input.RequirePrivacyPolicy,
					Logo = _imageService.IsImage(input.Icon)
				};
				application.ClientSecret = _encryptionService.GenerateApplicationSecret();
				application.Owner = await _context.Users.FindAsync(input.OwnerId);
				application.Brand = await _context.Brands.FindAsync(input.BrandId);
				if(application.Owner == null || application.Brand == null)
				{
					result.Success = false;
					result.Error = new List<string> { "Owner or Brand not found" };
					return result;
				}

				try
				{
					_ = _context.Applications.Add(application);
					_ = await _context.SaveChangesAsync();
				}
				catch(Exception ex)
				{
					_logger.LogError(ex, "Error registering application");
					result.Error = new List<string> { "Error registering application" };
					result.Success = false;
					return result;
				}

				// application is registered, now we have to deal with the icon
				string applicationId = application.Id.ToString();

				result.ApplicationId = application.Id.ToString();
				result.OwnerId = application.OwnerId;
				result.BrandId = application.BrandId;
				result.Enabled = application.IsEnabled;
				result.Success = true;
				if(input.Icon != null)
				{
					List<ApiResponse<string>> icons = await _imageService.CreateApplicationIcons(input.Icon, applicationId);
					if(icons.Where(s => s.Successful == false).Any())
					{
						result.Error = icons.Where(s => s.Successful == false).Select(s => s.Message).ToList();
						result.LogoCreated = false;
					}
					else
					{
						result.LogoCreated = true;
					}
				}
			}
			else
			{
				result.Success = false;
				result.Error = new List<string> { "Application name is not unique" };
			}
			return result;
		}

		// private functions

		// private classes
	}
}