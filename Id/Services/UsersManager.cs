using Id.Models.InputModels;
using Id.Models.ResultModels;
using SharedTools.Services;
using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
	public class UsersManager(ILogger<UsersManager> logger,
		ApplicationDbContext dbContext,
		ISettingsService settingsService,
		IEncryptionService encryptionService,
		IEmailService emailService,
		IImageService imageService,
		IStringLocalizer<UsersManager> t)
	{
		// Initialization
		private readonly ILogger<UsersManager> _logger = logger;

		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IEmailService _emailService = emailService;
		private readonly IImageService _imageService = imageService;
		private readonly IStringLocalizer<UsersManager> _t = t;

		// Private variables
		private string userImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "users");

		// Public functions
		public async Task<bool> IsEmailFreeAsync(string email)
		{
			return !await _dbContext.Users.AnyAsync(u => u.Email == email);
		}

		public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserInput input)
		{
			var result = new RegisterUserResult();
			return result;
		}

		// Private functions
		private async Task<CheckInputResult> CheckPasswordsAsync(string password, string confirmPassword)
		{
			var checkResult = new CheckInputResult();
			// Get password rules
			var rules = await _settingsService.GetPasswordRulesAsync();

			// Check if password is null or empty
			if(string.IsNullOrEmpty(password))
			{
				_logger.LogWarning("Password is null or empty");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password is null or empty"]);
				password = string.Empty;
			}
			// Check if passwords match
			if(password != confirmPassword)
			{
				_logger.LogWarning("Passwords do not match");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Passwords do not match"]);
			}
			// Check if password is too short
			if(password.Length < rules.MinimumLength)
			{
				_logger.LogWarning("Password is too short");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password is too short"]);
			}
			// Check if password is too long
			if(password.Length > rules.MaximumLength)
			{
				_logger.LogWarning("Password is too long");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password is too long"]);
			}
			// Check if password contains uppercase letters
			if(rules.RequireUppercase && !password.Any(char.IsUpper))
			{
				_logger.LogWarning("Password does not contain uppercase letters");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password does not contain uppercase letters"]);
			}
			// Check if password contains lowercase letters
			if(rules.RequireLowercase && !password.Any(char.IsLower))
			{
				_logger.LogWarning("Password does not contain lowercase letters");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password does not contain lowercase letters"]);
			}
			// Check if password contains digits
			if(rules.RequireDigit && !password.Any(char.IsDigit))
			{
				_logger.LogWarning("Password does not contain digits");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password does not contain digits"]);
			}
			// Check if password contains non-alphanumeric characters
			if(rules.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
			{
				_logger.LogWarning("Password does not contain non-alphanumeric characters");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Password does not contain non-alphanumeric characters"]);
			}

			return checkResult;
		}

		private async Task<CheckInputResult> CheckEmailAsync(string email)
		{
			var checkResult = new CheckInputResult();
			// Check if email is null or empty
			if(string.IsNullOrEmpty(email))
			{
				_logger.LogWarning("Email is null or empty");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Email is null or empty"]);
				email = string.Empty;
			}
			// Check if email is valid
			if(!new EmailAddressAttribute().IsValid(email))
			{
				_logger.LogWarning("Email is not valid");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Email is not valid"]);
			}
			// Check if email is already in use
			if(!await IsEmailFreeAsync(email))
			{
				_logger.LogWarning("Email is already in use");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Email is already in use"]);
			}
			return checkResult;
		}

		private async Task<CheckInputResult> CheckUserInputAsync(RegisterUserInput input)
		{
			var checkResult = new CheckInputResult();
			// Check email
			var emailCheck = await CheckEmailAsync(input.Email);
			if(!emailCheck.Success)
			{
				checkResult.Success = false;
				checkResult.Errors.AddRange(emailCheck.Errors);
			}
			// Check passwords
			var passwordCheck = await CheckPasswordsAsync(input.Password, input.PasswordConfirm);
			if(!passwordCheck.Success)
			{
				checkResult.Success = false;
				checkResult.Errors.AddRange(passwordCheck.Errors);
			}
			// Check if user accepted terms
			if(!input.AcceptTerms)
			{
				_logger.LogWarning("User did not accept terms");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["User did not accept terms"]);
			}
			// Check if user accepted privacy policy
			if(!input.AcceptPrivacyPolicy)
			{
				_logger.LogWarning("User did not accept privacy policy");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["User did not accept privacy policy"]);
			}
			// Check if user accepted cookie policy
			if(!input.AcceptCookiePolicy)
			{
				_logger.LogWarning("User did not accept cookie policy");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["User did not accept cookie policy"]);
			}
			return checkResult;
		}
	}
}