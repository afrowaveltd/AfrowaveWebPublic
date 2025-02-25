/*
 *  UsersManager.cs
 *  Class that manages users in the system and their profile pictures. It is responsible for creating, updating, and deleting users, as well as uploading and deleting their profile pictures.
 *  The class is responsible for checking the input data, hashing passwords, and sending emails to users.
 */

using Id.Models.InputModels;
using Id.Models.ResultModels;
using Id.Models.SettingsModels;
using SharedTools.Services;
using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
	public class UsersManager(ILogger<UsersManager> logger,
		ApplicationDbContext dbContext,
		ISettingsService settingsService,
		IEncryptionService encryptionService,
		IEmailManager emailService,
		IImageService imageService,
		IStringLocalizer<UsersManager> t) : IUsersManager
	{
		// Initialization
		private readonly ILogger<UsersManager> _logger = logger;

		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IEncryptionService _encryptionService = encryptionService;
		private readonly IEmailManager _emailService = emailService;
		private readonly IImageService _imageService = imageService;
		private readonly IStringLocalizer<UsersManager> _t = t;

		// Private variables
		private readonly string userImgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "users");

		private readonly string webImgDirectory = "/users";

		// Public functions

		/// <summary>
		/// Gets the path of the user's profile icon (32x32 px).
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Path to the profile picture</returns>
		/// <example>
		/// Request:
		/// await GetIconPath("1234567890")
		/// Response:
		/// "/users/1234567890/profile-picture/1234567890-32x32.jpg"
		/// </example>
		public async Task<string> GetIconPath(string userId)
		{
			return await GetImagePath(userId, ProfilePictureSize.icon);
		}

		/// <summary>
		/// Gets the path of the user's medium profile image (52x52 px).
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Path to the profile picture</returns>
		/// <example>
		/// Request:
		/// await GetMediumImagePath("1234567890")
		/// Response:
		/// "/users/1234567890/profile-picture/1234567890-52x52.jpg"
		/// </example>
		public async Task<string> GetMediumImagePath(string userId)
		{
			return await GetImagePath(userId, ProfilePictureSize.small);
		}

		/// <summary>
		/// Gets the path of the user's large profile image (192x192 px).
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Path to the profile picture</returns>
		/// <example>
		/// Request:
		/// await GetBigImagePath("1234567890")
		/// Response:
		/// "/users/1234567890/profile-picture/1234567890-192x192.jpg"
		/// </example>
		public async Task<string> GetBigImagePath(string userId)
		{
			return await GetImagePath(userId, ProfilePictureSize.big);
		}

		/// <summary>
		/// Gets the path of the user's original profile image.
		/// </summary>
		/// <param name="userId">User ID</param>
		/// <returns>Path to the profile picture</returns>
		/// <example>
		/// Request:
		/// await GetOriginalImagePath("1234567890")
		/// Response:
		/// "/users/1234567890/profile-picture/1234567890.jpg"
		/// </example>
		public async Task<string> GetOriginalImagePath(string userId)
		{
			return await GetImagePath(userId, ProfilePictureSize.original);
		}

		/// <summary>
		/// Checks if the given email is available.
		/// </summary>
		/// <param name="email">Email to check</param>
		/// <returns>True if the email is free, otherwise false</returns>
		/// <example>
		/// Request:
		/// await IsEmailFreeAsync("user@example.com")
		/// Response:
		/// true
		/// </example>
		public async Task<bool> IsEmailFreeAsync(string email)
		{
			return !await _dbContext.Users.AnyAsync(u => u.Email == email);
		}

		/// <summary>
		/// Registers a new user with validation and optional profile picture upload.
		/// </summary>
		/// <param name="input">User registration input model</param>
		/// <returns>Result of the registration process</returns>
		/// <example>
		/// Request:
		/// await RegisterUserAsync(new RegisterUserInput { Email = "user@example.com", Password = "password123", ... })
		/// Response:
		/// { "UserCreated": true, "ProfilePictureUploaded": false, "Errors": [] }
		/// </example>
		public async Task<RegisterUserResult> RegisterUserAsync(RegisterUserInput input)
		{
			LoginRules loginRules = await _settingsService.GetLoginRulesAsync();

			RegisterUserResult result = new();
			// Check input
			if(input == null)
			{
				_logger.LogWarning("Input is null");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.Add(_t["Input is null"]);
				return result;
			}
			CheckInputResult checkInput = await CheckUserInputAsync(input);
			if(!checkInput.Success)
			{
				_logger.LogWarning("Input is not valid");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.AddRange(checkInput.Errors);
				return result;
			}
			CheckInputResult emailCheck = await CheckEmailAsync(input.Email);
			if(!emailCheck.Success)
			{
				_logger.LogWarning("Email is not valid");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.AddRange(emailCheck.Errors);
				return result;
			}
			CheckInputResult passwordCheck = await CheckPasswordsAsync(input.Password, input.PasswordConfirm);
			if(!passwordCheck.Success)
			{
				_logger.LogWarning("Password is not valid");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.AddRange(passwordCheck.Errors);
				return result;
			}
			CheckInputResult termsCheck = CheckTermsAndConditions(input);
			if(!termsCheck.Success)
			{
				_logger.LogWarning("Terms and conditions are not accepted");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.AddRange(termsCheck.Errors);
				return result;
			}
			// Create user
			User user = new User
			{
				Email = input.Email,
				Password = await _encryptionService.HashPasswordAsync(input.Password),
				Gender = input.Gender,
				Firstname = input.FirstName,
				Lastname = input.LastName,
				DisplayName = input.DisplayedName,
				BirthDate = DateOnly.FromDateTime(input.Birthdate ?? DateTime.UtcNow),
				EmailConfirmed = !loginRules.RequireConfirmedEmail
			};
			// Save user
			try
			{
				_ = await _dbContext.Users.AddAsync(user);
				_ = await _dbContext.SaveChangesAsync();
				result.UserCreated = true;
				result.UserId = user.Id;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error while saving user");
				result.UserCreated = false;
				result.ProfilePictureUploaded = false;
				result.Errors.Add(_t["Error while saving user"]);
				return result;
			}

			// Upload profile picture
			if(input.ProfilePicture != null)
			{
				ApiResponse<string> uploadResult = await _imageService.CreateUserProfileImages(input.ProfilePicture, user.Id);
				if(uploadResult.Successful)
				{
					user.ProfilePicture = uploadResult.Data;
					_ = await _dbContext.SaveChangesAsync();
					result.ProfilePictureUploaded = true;
				}
				else
				{
					result.ProfilePictureUploaded = false;
				}
			}

			Console.WriteLine(user.ToString());
			return result;
		}

		/// <summary>
		/// Updates an existing user's details.
		/// </summary>
		/// <param name="input">User update input model</param>
		/// <returns>Result of the update process</returns>
		/// <example>
		/// Request:
		/// await UpdateUserAsync(new UpdateUserInput { UserId = "123456", FirstName = "John" })
		/// Response:
		/// { "Success": true, "UpdatedValues": { "First name": "John" }, "Errors": [] }
		/// </example>
		public async Task<UpdateResult> UpdateUserAsync(UpdateUserInput input)
		{
			UpdateResult result = new();
			// Check input
			if(input == null)
			{
				_logger.LogWarning("Input is null");
				result.Success = false;
				result.Errors.Add(_t["Input is null"]);
				return result;
			}
			// Get user
			User user = await _dbContext.Users.FindAsync(input.UserId);
			if(user == null)
			{
				_logger.LogWarning("User not found");
				result.Success = false;
				result.Errors.Add(_t["User not found"]);
				return result;
			}

			if(user.Email != input.Email)
			{
				_logger.LogWarning("Email is not valid");
				result.Success = false;
				result.Errors.AddRange("Email doesn't belong to the user");
				return result;
			}
			CheckInputResult checkInput = await CheckUserInputAsync(input);
			if(!checkInput.Success)
			{
				_logger.LogWarning("Input is not valid");
				result.Success = false;
				result.Errors.AddRange(checkInput.Errors);
				return result;
			}
			// Update user
			if(input.FirstName != user.Firstname)
			{
				user.Firstname = input.FirstName;
				result.UpdatedValues["First name"] = input.FirstName;
			}
			if(input.LastName != user.Lastname)
			{
				user.Lastname = input.LastName;
				result.UpdatedValues["Last name"] = input.LastName;
			}
			if(input.DisplayedName != user.DisplayName)
			{
				user.DisplayName = input.DisplayedName;
				result.UpdatedValues["Displayed name"] = input.DisplayedName;
			}
			if(DateOnly.FromDateTime(input.Birthdate ?? DateTime.UtcNow) != user.BirthDate)
			{
				user.BirthDate = DateOnly.FromDateTime(input.Birthdate ?? DateTime.UtcNow);
				result.UpdatedValues["Birthdate"] = input.Birthdate.ToString();
			}
			if(input.ProfilePicture != null)
			{
				ApiResponse<string> uploadResult = await _imageService.CreateUserProfileImages(input.ProfilePicture, user.Id);
				if(uploadResult.Successful)
				{
					user.ProfilePicture = uploadResult.Data;
					_ = await _dbContext.SaveChangesAsync();
					result.UpdatedValues["Profile picture"] = uploadResult.Data;
				}
				else
				{
					result.Errors.Add(uploadResult.Message);
				}
			}
			result.Success = true;
			_ = await _dbContext.SaveChangesAsync();
			return result;
		}

		// Private functions
		private async Task<CheckInputResult> CheckPasswordsAsync(string password, string confirmPassword)
		{
			CheckInputResult checkResult = new();
			// Get password rules
			PasswordRules rules = await _settingsService.GetPasswordRulesAsync();

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
			CheckInputResult checkResult = new CheckInputResult();
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

		private async Task<CheckInputResult> CheckUserInputAsync<T>(T input) where T : IUserInput
		{
			CheckInputResult checkResult = new CheckInputResult();
			// Check email
			CheckInputResult emailCheck = await CheckEmailAsync(input.Email);
			if(!emailCheck.Success)
			{
				checkResult.Success = false;
				checkResult.Errors.AddRange(emailCheck.Errors);
			}
			// Check names
			if(string.IsNullOrEmpty(input.FirstName))
			{
				_logger.LogWarning("First name is null or empty");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["First name is null or empty"]);
			}
			if(string.IsNullOrEmpty(input.LastName))
			{
				_logger.LogWarning("Last name is null or empty");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Last name is null or empty"]);
			}
			if(input.FirstName.Length < 2)
			{
				_logger.LogWarning("First name is too short");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["First name is too short"]);
			}
			if(input.LastName.Length < 2)
			{
				_logger.LogWarning("Last name is too short");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Last name is too short"]);
			}
			if(input.FirstName.Length > 50)
			{
				_logger.LogWarning("First name is too long");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["First name is too long"]);
			}
			if(input.LastName.Length > 50)
			{
				_logger.LogWarning("Last name is too long");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Last name is too long"]);
			}
			// Check display name
			if(string.IsNullOrEmpty(input.DisplayedName))
			{
				input.DisplayedName = input.FirstName;
			}
			// Check if birthday is not empty
			if(input.Birthdate == null)
			{
				_logger.LogWarning("Birthdate is null");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Birthdate is null"]);
			}
			// Check if birthdate is in the future
			if(input.Birthdate.HasValue && input.Birthdate.Value > DateTime.UtcNow)
			{
				_logger.LogWarning("Birthdate is in the future");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Birthdate is in the future"]);
			}

			// Check birthdate for minimal age of 7 years
			if(input.Birthdate.HasValue && input.Birthdate.Value.AddYears(7) > DateTime.UtcNow)
			{
				_logger.LogWarning("User is too young");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["User is too young"]);
			}

			return checkResult;
		}

		private CheckInputResult CheckTermsAndConditions(RegisterUserInput input)
		{
			CheckInputResult checkResult = new CheckInputResult();
			// Check if terms and conditions are accepted
			if(!input.AcceptTerms)
			{
				_logger.LogWarning("Terms and conditions are not accepted");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Terms and conditions are not accepted"]);
			}
			// Check if privacy policy is accepted
			if(!input.AcceptPrivacyPolicy)
			{
				_logger.LogWarning("Privacy policy is not accepted");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Privacy policy is not accepted"]);
			}
			// Check if cookie policy is accepted
			if(!input.AcceptCookiePolicy)
			{
				_logger.LogWarning("Cookie policy is not accepted");
				checkResult.Success = false;
				checkResult.Errors.Add(_t["Cookie policy is not accepted"]);
			}
			return checkResult;
		}

		private async Task<string> GetImagePath(string userId, ProfilePictureSize size)
		{
			string pictureName = await _dbContext.Users.Where(s => s.Id == userId).Select(s => s.ProfilePicture).FirstOrDefaultAsync() ?? string.Empty;
			string nameWithoutEtension = string.Empty;
			string extension = string.Empty;
			bool nameExists = false;
			if(pictureName != string.Empty)
			{
				nameExists = true;
				nameWithoutEtension = Path.GetFileNameWithoutExtension(pictureName);
				extension = Path.GetExtension(pictureName).TrimStart('.').ToLower();
			}

			string path = size switch
			{
				ProfilePictureSize.icon =>
				(nameExists && File.Exists(Path.Combine(userImgDirectory, userId, "profile-picture", $"{pictureName}-32x32.{extension}")))
				? $"{webImgDirectory}/{userId}/profile-picture/{nameWithoutEtension}-32x32.{extension}"
				: "/img/no-icon_32.png",

				ProfilePictureSize.small =>
				(nameExists && File.Exists(Path.Combine(userImgDirectory, userId, "profile-picture", $"{pictureName}-52x52.{extension}")))
				? $"{webImgDirectory}/{userId}/profile-picture/{nameWithoutEtension}-52x52.{extension}"
				: "/img/no-icon_52.png",

				ProfilePictureSize.big =>
				(nameExists && File.Exists(Path.Combine(userImgDirectory, userId, "profile-picture", $"{pictureName}-192x192.{extension}")))
				? $"{webImgDirectory}/{userId}/profile-picture/{nameWithoutEtension}-192x192.{extension}"
				: "/img/no-icon_192.png",

				_ =>
				(nameExists && File.Exists(Path.Combine(userImgDirectory, userId, "profile-picture", $"{pictureName}.{extension}")))
				? $"{webImgDirectory}/{userId}/profile-picture/{nameWithoutEtension}.{extension}"
				: "/img/no-icon.png"
			};
			return path;
		}
	}
}