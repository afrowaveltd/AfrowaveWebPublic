using Id.Models.CommunicationModels;
using SharedTools.Services;

namespace Id.Services
{
	public class BrandService(ApplicationDbContext context,
	  IImageService imageService,
	  ILogger<BrandService> logger,
	  IStringLocalizer<BrandService> _t) : IBrandService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly string[] permittedExtensions = { ".jpeg", ".jpg", ".gif", ".png" };
		private readonly IImageService _imageService = imageService;
		private readonly ILogger<BrandService> _logger = logger;
		private readonly IStringLocalizer<BrandService> t = _t;
		private bool uploadIcons = true;

		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			 .Substring(0, AppDomain.CurrentDomain.BaseDirectory
			 .IndexOf("bin")), "wwwroot");

		// public functions
		public async Task<bool> IsBrandNameUnique(string name)
		{
			return (!await _context.Brands.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim()).AnyAsync());
		}

		public string GetBrandIconPath(int brandId, LogoSize size)
		{
			string applicationId = brandId.ToString();
			var filePath = string.Empty;
			var noLogo = "/img/no-icon.png";
			if(string.IsNullOrEmpty(applicationId)) { return noLogo; }
			filePath = size switch
			{
				LogoSize.png16px => Path.Combine("brands", applicationId, "icons", "icon-16x16.png"),
				LogoSize.png32px => Path.Combine("brands", applicationId, "icons", "icon-32x32.png"),
				LogoSize.png76px => Path.Combine("brands", applicationId, "icons", "icon-76x76.png"),
				LogoSize.png120px => Path.Combine("brands", applicationId, "icons", "icon-120x120.png"),
				LogoSize.png152px => Path.Combine("brands", applicationId, "icons", "icon-152x152.png"),
				_ => Path.Combine("brands", applicationId, "icons", "original-icon*.png"),
			};
			if(File.Exists(Path.Combine(appImgDirectory, filePath)))
			{
				if(size == LogoSize.pngOriginal)
				{
					return $"/brands/{applicationId}/icons/original-icon-*.png";
				}
				else
				{
					return $"/brands/{applicationId}/icons/icon-{size}x{size}.png";
				}
			}
			else
			{
				return noLogo;
			}
		}

		public string GetBrandIconPath(int brandId)
		{
			return GetBrandIconPath(brandId, LogoSize.png32px);
		}

		public string GetBrandImagePath(int brandId)
		{
			return GetBrandIconPath(brandId, LogoSize.pngOriginal);
		}

		// private functions

		public async Task<CreateBrandResponse> RegisterBrandAsync(CreateBrandModel Input)
		{
			CreateBrandResponse response = new CreateBrandResponse();
			if(Input is null)
			{
				_logger.LogError("Model is null");
				response.WhyCantCreate = t["Model is null"];
				return response;
			}

			if(string.IsNullOrWhiteSpace(Input.Name))
			{
				response.WhyCantCreate = t["Brand name is required"];
				return response;
			}
			if(Input.Name.Length > 50)
			{
				response.WhyCantCreate = t["Brand name should have between 2 and 50 characters."];
				return response;
			}
			if(Input.Name.Length < 2)
			{
				response.WhyCantCreate = t["Brand name should have between 2 and 50 characters."];
				return response;
			}

			if(string.IsNullOrWhiteSpace(Input.OwnerId))
			{
				response.WhyCantCreate = t["Owner not found"];
				return response;
			}

			if(!string.IsNullOrWhiteSpace(Input.Email))
			{
				if(!Tools.IsEmailValid(Input.Email))
				{
					response.WhyCantCreate = t["Invalid email address"];
					return response;
				}
			}
			if(!string.IsNullOrWhiteSpace(Input.Website))
			{
				if(!Tools.IsValidUrl(Input.Website))
				{
					response.WhyCantCreate = t["Website is not valid"];
					return response;
				}
			}
			if(Input.Icon == null)
			{
				response.WhyCantUploadImage = t["Icon is missing"];
				response.CanUploadImage = false;
			}
			if(Input.Icon != null)
			{
				if(!Tools.IsImage(Input.Icon))
				{
					response.WhyCantUploadImage = t["Logo is not valid"];
					response.CanUploadImage = false;
				}
			}

			// check if the brand name is unique
			if(!await IsBrandNameUnique(Input.Name))
			{
				response.WhyCantCreate = t["Brand name is already taken"];
				return response;
			}

			response.CanCreate = true;
			// create the brand

			User? owner = await _context.Users.FindAsync(Input.OwnerId);
			if(owner == null)
			{
				response.WhyCantCreate = t["Owner not found"];
				return response;
			}

			Brand brand = new Brand
			{
				Name = Input.Name,
				Description = Input.Description,
				Website = Input.Website,
				Email = Input.Email,
				Owner = owner,
				Logo = Input.Icon != null
			};

			// save the brand to the db
			try
			{
				_ = await _context.Brands.AddAsync(brand);
				_ = await _context.SaveChangesAsync();
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Error while saving the brand to the database");
				response.WhyCantCreate = t["Error while saving the brand to the database"];
				return response;
			}
			_logger.LogInformation("Brand {brandName} created by {ownerId} with id: {id}", brand.Name, brand.OwnerId, brand.Id);

			// if there is no icon, return the brand id,
			// if there is an icon, create set of application icons for the brand and store them to /brands/{brandId}/icons

			if(uploadIcons)
			{
				List<ApiResponse<string>> iconResponse = await _imageService.CreateBrandIcons(Input.Icon, brand.Id);
				if(iconResponse.Any(r => r.Successful == false))
				{
					response.WhyCantUploadImage = t["Error while saving the brand icon"];
					response.CanUploadImage = false;
				}
				else
				{
					response.CanUploadImage = true;
				}
			}

			return response;
		}
	}
}