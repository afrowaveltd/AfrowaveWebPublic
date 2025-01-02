using Id.Models.CommunicationModels;
using System.ComponentModel.DataAnnotations;

namespace Id.Pages.Install
{
    public class BrandModel(ILogger<BrandModel> logger,
      ApplicationDbContext context,
      IInstallationStatusService installationStatus,
      IStringLocalizer<BrandModel> _t,
      IBrandService brandService)
     : PageModel
    {
        private readonly ILogger<BrandModel> _logger = logger;
        private readonly ApplicationDbContext _context = context;
        private readonly IInstallationStatusService _installationStatus = installationStatus;
        public readonly IStringLocalizer<BrandModel> t = _t;
        public readonly IBrandService _brandService = brandService;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ErrorMessage { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            public string OwnerId { get; set; } = string.Empty;

            public IFormFile? CompanyLogo { get; set; }
            public string? Website { get; set; }
            public string? Description { get; set; }
            public string? Email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if(!await _installationStatus.ProperInstallState(InstalationSteps.Brand))
            {
                _logger.LogWarning("Installation is not in the correct state");
                return RedirectToPage("/Index");
            }
            Input.OwnerId = await _context.Users.Select(u => u.Id).FirstOrDefaultAsync();
            if(Input.OwnerId == null)
            {
                _logger.LogError("No user found to be the owner of the brand");
                Exception ex = new Exception("No user found");
                throw ex;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!await _installationStatus.ProperInstallState(InstalationSteps.Brand))
            {
                _logger.LogWarning("Installation is not in the correct state");
                return RedirectToPage("/Index");
            }
            if(!ModelState.IsValid)
            {
                return Page();
            }
            CreateBrandModel newBrand = new()
            {
                Name = Input.Name,
                Description = Input.Description,
                Icon = Input.CompanyLogo,
                Website = Input.Website,
                Email = Input.Email,
                OwnerId = Input.OwnerId
            };
            CreateBrandResponse response = await _brandService.RegisterBrandAsync(newBrand);

            // Check all the fields
            if(response.CanCreate)
            {
                return RedirectToPage("/Install/Application");
            }
            else
            {
                ModelState.AddModelError("Input.Name", response.WhyCantCreate);
                return Page();
            }
        }
    }
}