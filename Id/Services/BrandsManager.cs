using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;

namespace Id.Services
{
	public class BrandsManager(
		ApplicationDbContext context,
		IImageService imageService,
		ILogger<BrandsManager> logger,
		IStringLocalizer<BrandsManager> t)
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly IImageService _imageService = imageService;
		private readonly ILogger<BrandsManager> _logger = logger;
		private readonly IStringLocalizer<BrandsManager> _t = t;

		// Private variables
		private readonly string[] permittedExtensions = { ".jpeg", ".jpg", ".gif", ".png" };

		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			.Substring(0, AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")), "wwwroot", "brands");

		private string noLogo = "/img/no-icon.png";

		// Public functions
		public async Task<bool> IsNameUnique(string name)
		{
			return (!await _context.Brands
				.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim())
				.AnyAsync());
		}

		public async Task<RegisterBrandResult> RegisterBrandAsync(RegisterBrandInput input)
		{
			RegisterBrandResult result = new();
			if(input == null)
			{
				_logger.LogError("RegisterBrandAsync: input is null");
				result.ErrorMessage = _t["Model is null"];
				result.BrandCreated = false;
				result.LogoUploaded = false;
				return result;
			}
			CheckInputResult checkInput = await CheckBrandInputAsync(input);
			if(!checkInput.Success)
			{
				_logger.LogError("RegisterBrandAsync: input is invalid");
				result.ErrorMessage = string.Join(", ", checkInput.Errors);
				result.BrandCreated = false;
				result.LogoUploaded = false;
				return result;
			}
			else
			{
				Brand brand = new()
				{
					Name = input.Name,
					Description = input.Description,
					Website = input.Website,
					Email = input.Email,
					OwnerId = input.OwnerId
				};
				await _context.Brands.AddAsync(brand);
				await _context.SaveChangesAsync();
				result.BrandId = brand.Id;
				if(input.Icon != null)
				{
					List<ApiResponse<string>> iconResponse = await _imageService.CreateBrandIcons(input.Icon, brand.Id);
					if(iconResponse.Any(r => r.Successful == false))
					{
						_logger.LogError("RegisterBrandAsync: icon upload failed");
						result.LogoUploaded = false;
						result.ErrorMessage = _t["Logo upload failed"];
						return result;
					}
					else
					{
						result.LogoUploaded = true;
					}
				}
			}
			return result;
		}

		public string GetLogoPath(int brandId, LogoSize size)
		{
			string logoPath = size switch
			{
				LogoSize.png16px =>
					File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icon-16x16.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icon-16x16.png")
					: "/img/no-icon_16.png",
				LogoSize.png32px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icon-32x32.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icon-32x32.png")
					: "/img/no-icon_32.png",
				LogoSize.png76px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icon-76x76.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icon-76x76.png")
					: "/img/no-icon_76.png",
				LogoSize.png120px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icon-120x120.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icon-120x120.png")
					: "/img/no-icon_120.png",
				LogoSize.png152px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icon-152x152.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icon-152x152.png")
					: "/img/no-icon_152.png",
				_ =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "original-icon*.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "original-icon*.png")
					: "/img/no-icon.png"
			};
			return logoPath;
		}

		public string GetFullsizeLogoPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.pngOriginal);
		}

		public string GetIconPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.png32px);
		}

		// private functions

		private async Task<CheckInputResult> CheckBrandInputAsync(RegisterBrandInput input)
		{
			CheckInputResult result = new();
			if(input == null)
			{
				_logger.LogError("CheckRegistration: input is null");
				result.Success = false;
				result.Errors.Add(_t["Missing application data"]);
				return result;
			}
			if(string.IsNullOrEmpty(input.Name))
			{
				_logger.LogError("CheckRegistration: Name is null");
				result.Success = false;
				result.Errors.Add(_t["Brand name is required"]);
				return result;
			}
			if(input.Name.Length > 50)
			{
				_logger.LogError("CheckRegistration: Name is too long");
				result.Errors.Add(_t["Brand name should have between 2 and 50 characters."]);
				return result;
			}
			if(input.Name.Length < 2)
			{
				_logger.LogError("CheckRegistration: Name is too short");
				result.Errors.Add(_t["Brand name should have between 2 and 50 characters."]);
				return result;
			}

			if(string.IsNullOrEmpty(input.OwnerId))
			{
				_logger.LogError("CheckRegistration: OwnerId is null");
				result.Success = false;
				result.Errors.Add(_t["Owner not found"]);
				return result;
			}
			if(await IsNameUnique(input.Name) == false)
			{
				_logger.LogError("CheckRegistration: Name is not unique");
				result.Success = false;
				result.Errors.Add(_t["Brand name is already registered"]);
				return result;
			}
			if(!string.IsNullOrWhiteSpace(input.Email))
			{
				if(!Tools.IsEmailValid(input.Email))
				{
					_logger.LogError("CheckRegistration: Email is not valid");
					result.Success = false;
					result.Errors.Add(_t["Invalid email address"]);
					return result;
				}
			}

			if(!string.IsNullOrWhiteSpace(input.Website))
			{
				if(!Tools.IsValidUrl(input.Website))
				{
					_logger.LogError("CheckRegistration: Website is not valid");
					result.Success = false;
					result.Errors.Add(_t["Website is not valid"]);
					return result;
				}
			}

			if(input.Icon != null)
			{
				if(!Tools.IsImage(input.Icon))
				{
					_logger.LogError("CheckRegistration: Icon is not valid");
					result.Success = false;
					result.Errors.Add(_t["Logo is not valid"]);
					return result;
				}
			}

			return result;
		}
	}
}