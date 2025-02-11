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
		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			.Substring(0, AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")), "wwwroot", "brands");

		// Public functions
		public string GetFullsizeLogoPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.pngOriginal);
		}

		public string GetIconPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.png32px);
		}

		public string GetLogoPath(int brandId, LogoSize size)
		{
			string logoPath = size switch
			{
				LogoSize.png16px =>
					File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-16x16.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-16x16.png")
					: "/img/no-icon_16.png",
				LogoSize.png32px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-32x32.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-32x32.png")
					: "/img/no-icon_32.png",
				LogoSize.png76px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-76x76.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-76x76.png")
					: "/img/no-icon_76.png",
				LogoSize.png120px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-120x120.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-120x120.png")
					: "/img/no-icon_120.png",
				LogoSize.png152px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-152x152.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-152x152.png")
					: "/img/no-icon_152.png",
				_ =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "original-icon*.png"))
					? Path.Combine(appImgDirectory, brandId.ToString(), "icons", "original-icon*.png")
					: "/img/no-icon.png"
			};
			return logoPath;
		}

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
			if(await IsNameUnique(input.Name) == false)
			{
				_logger.LogError("CheckRegistration: Name is not unique");
				result.BrandCreated = false;
				result.LogoUploaded = false;
				result.ErrorMessage = _t["Brand name is already registered"];
				return result;
			}
			CheckInputResult checkInput = CheckBrandInput(input);
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
				_ = await _context.Brands.AddAsync(brand);
				_ = await _context.SaveChangesAsync();
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
						brand.Logo = true;
						_ = await _context.SaveChangesAsync();
						result.LogoUploaded = true;
					}
				}
			}
			return result;
		}

		public async Task<UpdateBrandResult> UpdateBrandAsync(UpdateBrandInput input)
		{
			UpdateBrandResult result = new();
			if(input == null)
			{
				_logger.LogError("UpdateBrandAsync: input is null");
				result.Errors.Add(_t["Model is null"]);
				result.Success = false;
				return result;
			}
			CheckInputResult checkInput = CheckBrandInput(input);
			if(!checkInput.Success)
			{
				_logger.LogError("UpdateBrandAsync: input is invalid");
				result.Errors.AddRange(checkInput.Errors);
				result.Success = false;
				return result;
			}
			else
			{
				Brand? brand = await _context.Brands
					.Include(b => b.Owner)
					.FirstOrDefaultAsync(b => b.Id == input.BrandId);
				if(brand == null)
				{
					_logger.LogError("UpdateBrandAsync: brand not found");
					result.Errors.Add(_t["Brand not found"]);
					result.Success = false;
					return result;
				}
				if(brand.OwnerId != input.OwnerId)
				{
					_logger.LogError("UpdateBrandAsync: user is not the owner");
					result.Errors.Add(_t["User is not the owner"]);
					result.Success = false;
					return result;
				}
				if(brand.Name != input.Name)
				{
					if(await IsNameUnique(input.Name) == false)
					{
						_logger.LogError("UpdateBrandAsync: Name is not unique");
						result.Errors.Add(_t["Brand name is already registered"]);
						result.Success = false;
						return result;
					}
					brand.Name = input.Name;
					result.UpdatedValues.Add("Name", input.Name);
				}
				if(brand.Description != input.Description)
				{
					brand.Description = input.Description;
					result.UpdatedValues.Add("Description", input.Description ?? string.Empty);
				}
				if(brand.Website != input.Website)
				{
					brand.Website = input.Website;
					result.UpdatedValues.Add("Website", input.Website ?? string.Empty);
				}
				if(brand.Email != input.Email)
				{
					brand.Email = input.Email;
					result.UpdatedValues.Add("Email", input.Email ?? string.Empty);
				}

				if(input.Icon != null)
				{
					List<ApiResponse<string>> iconResponse = await _imageService.CreateBrandIcons(input.Icon, brand.Id);
					if(iconResponse.Any(r => r.Successful == false))
					{
						_logger.LogError("UpdateBrandAsync: icon upload failed");
						result.Success = false;
						result.Errors.Add(_t["Logo upload failed"]);
						return result;
					}
					else
					{
						brand.Logo = true;
					}
				}
				_ = await _context.SaveChangesAsync();
				return result;
			}
		}

		// private functions
		private CheckInputResult CheckBrandInput<T>(T input) where T : IBrandInput
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
			}
			if(input.Name.Length > 50)
			{
				_logger.LogError("CheckRegistration: Name is too long");
				result.Success = false;
				result.Errors.Add(_t["Brand name should have between 2 and 50 characters."]);
			}
			if(input.Name.Length < 2)
			{
				_logger.LogError("CheckRegistration: Name is too short");
				result.Success = false;
				result.Errors.Add(_t["Brand name should have between 2 and 50 characters."]);
			}
			if(string.IsNullOrEmpty(input.OwnerId))
			{
				_logger.LogError("CheckRegistration: OwnerId is null");
				result.Success = false;
				result.Errors.Add(_t["Owner not found"]);
				return result;
			}

			if(!string.IsNullOrWhiteSpace(input.Email))
			{
				if(!Tools.IsEmailValid(input.Email))
				{
					_logger.LogError("CheckRegistration: Email is not valid");
					result.Success = false;
					result.Errors.Add(_t["Invalid email address"]);
				}
			}

			if(!string.IsNullOrWhiteSpace(input.Website))
			{
				if(!Tools.IsValidUrl(input.Website))
				{
					_logger.LogError("CheckRegistration: Website is not valid");
					result.Success = false;
					result.Errors.Add(_t["Website is not valid"]);
				}
			}

			if(input.Icon != null)
			{
				if(!Tools.IsImage(input.Icon))
				{
					_logger.LogError("CheckRegistration: Icon is not valid");
					result.Success = false;
					result.Errors.Add(_t["Logo is not valid"]);
				}
			}

			return result;
		}
	}
}