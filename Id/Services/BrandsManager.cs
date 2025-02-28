/*
 *  The BrandsManager class is responsible for managing the brands in the application.
 *  It provides methods to get information about a brand, get the applications of a brand,
 *  Register a new brand, update a brand, check if a brand name is unique, get the path to the logo of a brand,
 *  Get the path to the icon of a brand, get the path to the full size logo of a brand, and check if a user is the owner of a brand.
 */

using Id.Models.DataViews;
using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;

namespace Id.Services
{
	/// <summary>
	/// Interface for managing brands.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="imageService"></param>
	/// <param name="logger"></param>
	/// <param name="t"></param>
	public class BrandsManager(
		ApplicationDbContext context,
		IImageService imageService,
		ILogger<BrandsManager> logger,
		IStringLocalizer<BrandsManager> t) : IBrandsManager
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly IImageService _imageService = imageService;
		private readonly ILogger<BrandsManager> _logger = logger;
		private readonly IStringLocalizer<BrandsManager> _t = t;

		// Private variables
		private readonly string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
[..AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")], "wwwroot", "brands");

		private readonly string webImgDirectory = "/brands";

		// Public functions

		/// <summary> Gets the applications of a brand </summary>
		/// <param name="brandId">brandId</param>
		/// <remarks>Gets the applications of a brand</remarks>
		/// <returns>List of ApplicationView</returns>
		/// <example>
		/// <!-- This will return the applications of the brand with the given id. -->
		/// var applications = await GetBrandApplications(1);
		/// <!-- Example returns: -->
		/// [
		///		{
		///			"ApplicationId": 1,
		///			"ApplicationName": "name",
		///			"ApplicationDescription": "description",
		///			"ApplicationWebsite": "https://example.com",
		///			"ApplicationEmail": "email@example.com",
		///			"BrandName": "brandName"
		///		},
		///		{
		///			"ApplicationId": 2,
		///			"ApplicationName": "name",
		///			"ApplicationDescription": "description",
		///			"ApplicationWebsite": "https://example.com",
		///			"ApplicationEmail": "other@example.com",
		///			"BrandName": "brandName"
		///		}
		///	 ]
		///	</example>
		public async Task<List<ApplicationView>> GetBrandApplicationsAsync(int brandId)
		{
			List<ApplicationView> applications = await _context.Applications
				.Include(a => a.Brand)
				.Include(a => a.Owner)
				.Where(a => a.BrandId == brandId)
				.Select(a => new ApplicationView
				{
					ApplicationId = a.Id,
					ApplicationName = a.Name,
					ApplicationDescription = a.Description,
					ApplicationWebsite = a.ApplicationWebsite,
					ApplicationEmail = a.ApplicationEmail,
					BrandName = a.Brand.Name ?? string.Empty
				})
				.ToListAsync();
			return applications ?? new();
		}

		/// <summary>
		/// Gets the information of a brand
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns>BrandView class instance if brand is found or null</returns>
		/// <example>
		/// <!-- This is an example of how to use the GetBrandInfoAsync method. -->
		/// await GetBrandInfoAsync(1);
		/// <!-- This will return the information of the brand with the given id. -->
		/// new BrandView {
		///		BranId = 1,
		///		Name = "name",
		///		Description = "description",
		///		LogoPath = "/brands/brandId/icons/icon-32x32.png",
		///		Website = "https://example.com",
		///		Email = "some@example.com",
		///		OwnerName = "ownerName"
		///	 };
		///	 </example>
		public async Task<BrandView?> GetBrandInfoAsync(int brandId)
		{
			BrandView? brandView = await _context.Brands
				.Include(b => b.Owner)
				.Where(b => b.Id == brandId)
				.Select(b => new BrandView
				{
					Id = b.Id,
					Name = b.Name,
					Description = b.Description,
					LogoPath = GetIconPath(b.Id),
					Website = b.Website,
					Email = b.Email,
					OwnerName = b.Owner.DisplayName
				})
				.FirstOrDefaultAsync();
			return brandView;
		}

		/// <summary>
		/// Gets the owner id of a brand
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns>string representing UserId of the brand owner or null</returns>
		/// <example>
		/// <!-- Example of how to use the GetBrandOwnerId method. -->
		/// await GetBrandOwnerId(1);
		/// <!-- This will return the owner id of the brand with the given id. -->
		/// Returns: "ownerId"
		/// </example>
		public async Task<string?> GetBrandOwnerId(int brandId)
		{
			return await _context.Brands
				.Where(b => b.Id == brandId)
				.Select(b => b.OwnerId)
				.FirstOrDefaultAsync();
		}

		/// <summary>
		///  Gets the path to the fullsize logo of a brand
		/// </summary>
		/// <param name="brandId">brandId</param>
		/// <returns> url for a logo or placeholder if logo is not available</returns>
		/// <example>
		/// <!-- This is an example of how to use the GetFullsizeLogoPath method. -->
		/// await GetFullsizeLogoPath("brandId");
		/// <!-- This will return the url for the logo of the brand with the given id. -->
		/// Returns: "/brands/brandId/icons/original-icon*.png"
		/// </example>
		public string GetFullsizeLogoPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.pngOriginal);
		}

		/// <summary>
		/// Gets the path to the icon of a brand
		/// </summary>
		/// <param name="brandId">brandId</param>
		/// <returns>The url for a logo or placeholder if logo is not available</returns>
		/// <example>
		/// <!-- This is an example of how to use the GetIconPath method. -->
		/// await GetIconPath("brandId");
		/// <!-- This will return the url for the icon of the brand with the given id. -->
		/// Returns: "/brands/brandId/icons/icon-32x32.png"
		/// </example>
		public string GetIconPath(int brandId)
		{
			return GetLogoPath(brandId, LogoSize.png32px);
		}

		/// <summary> Gets the path to the logo of a brand </summary>
		/// <param name="brandId">brandId</param>
		/// <param name="size">size</param>
		/// <returns>The url for a logo or placeholder if logo is not available</returns>
		/// <example>
		/// <!-- This is an example of how to use the GetLogoPath method. -->
		/// await GetLogoPath("brandId", LogoSize.png16px);
		/// Returns: "/brands/brandId/icons/icon-16x16.png"
		/// </example>
		public string GetLogoPath(int brandId, LogoSize size)
		{
			string logoPath = size switch
			{
				LogoSize.png16px =>
					File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-16x16.png"))
					? $"{webImgDirectory}/{brandId}/icons/icon-16x16.png"
					: "/img/no-icon_16.png",
				LogoSize.png32px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-32x32.png"))
					? $"{webImgDirectory}/{brandId}/icons/icon-32x32.png"
					: "/img/no-icon_32.png",
				LogoSize.png76px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-76x76.png"))
					? $"{webImgDirectory}/{brandId}/icons/icon-76x76.png"
					: "/img/no-icon_76.png",
				LogoSize.png120px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-120x120.png"))
					? $"{webImgDirectory}/{brandId}/icons/icon-120x120.png"
					: "/img/no-icon_120.png",
				LogoSize.png152px =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "icon-152x152.png"))
					? $"{webImgDirectory}/{brandId}/icons/icon-152x152.png"
					: "/img/no-icon_152.png",
				_ =>
				File.Exists(Path.Combine(appImgDirectory, brandId.ToString(), "icons", "original-icon*.png"))
					? $"{webImgDirectory}/{brandId}/icons/original-icon*.png"
					: "/img/no-icon.png"
			};
			return logoPath;
		}

		/// <summary> Checks if a brand name is unique </summary>
		/// <param name="name">name</param>
		/// <returns>True if the name is unique, false otherwise</returns>
		/// <example>
		/// <!-- This is an example of how to use the IsNameUnique method. -->
		/// await IsNameUnique("name");
		/// Returns: true
		/// </example>
		public async Task<bool> IsNameUnique(string name)
		{
			return (!await _context.Brands
				.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim())
				.AnyAsync());
		}

		/// <summary> Registers a new brand </summary>
		/// <param name="input">input class</param>
		/// <returns>RegisterBrandResult</returns>
		/// <example>
		/// <!-- This is an example of how to use the RegisterBrandAsync method. -->
		/// await RegisterBrandAsync(new RegisterBrandInput(){
		///		Name = "name",
		///		Description = "description",
		///		Icon = new IFormFile(),
		///		Website = "https://example.com",
		///		Email = "some@example.com",
		///		OnwerId = "ownerId"
		/// });
		/// <!-- Example returns: -->
		/// {
		///		BrandCreated = true,
		///		BrandId = 1,
		///		LogoUploaded = true,
		///		ErrorMessages = ""
		///	 }
		/// </example>
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

		/// <summary> Updates a brand </summary>
		/// <param name="input">input class</param>
		/// <returns>UpdateResult</returns>
		/// <example>
		/// <!-- This is an example of how to use the UpdateBrandAsync method. -->
		/// RegisterBrandResult result = await UpdateBrandAsync(new UpdateBrandInput(){
		///   BrandId = 1,
		///   Name = "New name",
		///   Description = "New description",
		///   Icon = new IFormFile(),
		///   Website = "https://example.com",
		///   Email = "some@example.com",
		///   OwnerId = "ownerId"
		/// });
		/// <!-- Example returns: -->
		/// {
		///		success = true,
		///		UpdateValues = {
		///		"Name": "New name",
		///		"Description": "New description",
		///		},
		///		Errors = []
		///	 }
		///	 </example>
		public async Task<UpdateResult> UpdateBrandAsync(UpdateBrandInput input)
		{
			UpdateResult result = new();
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

		/// <summary>
		/// Checks if a user is the owner of a brand
		/// </summary>
		/// <param name="brandId"></param>
		/// <param name="ownerId"></param>
		/// <returns>A boolean if the user is owner of the brand</returns>
		/// <example>
		/// <!-- This is an example of how to use the ValidBrandAndOwner method. -->
		/// await ValidBrandAndOwner(1, "ownerId");
		/// returns: true
		/// </example>
		public async Task<bool> ValidBrandAndOwner(int brandId, string ownerId)
		{
			return await _context.Brands
				.Where(b => b.Id == brandId && b.OwnerId == ownerId)
				.AnyAsync();
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