using Id.Models.CommunicationModels;
using SharedTools.Services;

namespace Id.Services
{
    public class ApplicationService(ApplicationDbContext context,
      IImageService imageService,
      IEncryptionService encryptionService,

      ILogger<ApplicationService> logger) : IApplicationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IImageService _imageService = imageService;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ILogger<ApplicationService> _logger = logger;

        // public functions
        public async Task<bool> IsApplicationNameUnique(string name)
        {
            return (!await _context.Applications.Where(s => s.Name.ToLower().Trim() == name.ToLower().Trim()).AnyAsync());
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
                    RequireConsent = input.RequireConsent,
                    AllowRememberConsent = input.AllowRememberConsent,
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