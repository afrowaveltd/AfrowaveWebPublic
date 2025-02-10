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

		private bool uploadIcons = true;

		private string appImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
			.Substring(0, AppDomain.CurrentDomain.BaseDirectory
			.IndexOf("bin")), "wwwroot", "brands");

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
			}

			return result;
		}

		// private fuctions

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