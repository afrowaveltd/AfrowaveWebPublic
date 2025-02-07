using Id.Models.CommunicationModels;
using Id.Models.SettingsModels;
using System.ComponentModel.DataAnnotations;

namespace Id.Services
{
	public class UserService(ApplicationDbContext context, ISettingsService settingsService, IEncryptionService encryption) : IUserService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly ISettingsService _settingsService = settingsService;
		private readonly IEncryptionService _encryption = encryption;

		public async Task<bool> IsEmailUnique(string email)
		{
			return (!await _context.Users.Where(s => s.Email == email.ToLower().Trim()).AnyAsync());
		}

		public async Task<ApiResponse<List<string>>> CreateUserAsync(RegisterApplicationUserModel user)
		{
			ApiResponse<List<string>> response = new ApiResponse<List<string>>();
			ApiResponse<List<string>> formCheck = await CheckForm(user);
			if(!formCheck.Successful)
			{
				response = formCheck;
				return response;
			}
			var User = new User
			{
				Email = user.Email.ToLowerInvariant().Trim(),
				Firstname = user.FirstName.Trim(),
				Lastname = user.LastName.Trim(),
				DisplayName = user.DisplayedName == null ? user.FirstName.Trim() : user.DisplayedName.Trim(),
				Password = await _encryption.HashPasswordAsync(user.Password),
				BirthDate = DateOnly.FromDateTime(user.Birthdate ?? DateTime.UtcNow),
				AccessFailedCount = 0
			};
			_ = await _context.Users.AddAsync(User);
			_ = await _context.SaveChangesAsync();
			// Now we have the User - remains to check if we need to store profile picture and if we need to send email confirmation

			///Todo: Implement profile picture storage

			return response;
		}

		public async Task<ApiResponse<int>> CreateApplicationUserAsync(CreateApplicationUserModel user)
		{
			ApiResponse<int> response = new ApiResponse<int>();
			if(await _context.ApplicationUsers.Where(s => s.ApplicationId == user.ApplicationId && s.UserId == user.UserId).AnyAsync())
			{
				response.Message = "User already exists";
				response.Successful = true;
				response.Data = await _context.ApplicationUsers
					.Where(s => s.ApplicationId == user.ApplicationId && s.UserId == user.UserId)
					.Select(s => s.Id)
					.FirstOrDefaultAsync();
				return response;
			}
			else
			{
				if(!await _context.Applications.Where(s => s.Id == user.ApplicationId).AnyAsync())
				{
					response.Message = "Application not found";
					response.Successful = false;
					return response;
				}
				if(!await _context.Users.Where(s => s.Id == user.UserId).AnyAsync())
				{
					response.Message = "User not found";
					response.Successful = false;
					return response;
				}

				ApplicationUser applicationUser = new ApplicationUser
				{
					ApplicationId = user.ApplicationId,
					UserId = user.UserId,
					AgreedToCookies = user.AgreedToCookies,
					AgreedSharingUserDetails = user.AgreedSharingUserDetails,
					AgreedToTerms = user.AgreedToTerms,
					Application = await _context.Applications.FindAsync(user.ApplicationId),
					User = await _context.Users.FindAsync(user.UserId)
				};
				_ = await _context.ApplicationUsers.AddAsync(applicationUser);
				_ = await _context.SaveChangesAsync();
				response.Successful = true;
				response.Data = applicationUser.Id;
				return response;
			}
		}

		public async Task<ApiResponse<bool>> DeleteApplicationUserAsync(int id)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();

			return response;
		}

		public async Task<ApiResponse<List<ApplicationUser>>> GetApplicationUsersAsync(string applicationId)
		{
			ApiResponse<List<ApplicationUser>> response = new ApiResponse<List<ApplicationUser>>();
			return response;
		}

		public async Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, RegisterApplicationUserModel user)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();
			return response;
		}

		public async Task<ApiResponse<bool>> UpdateApplicationUserAsync(int id, CreateApplicationUserModel user)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();
			return response;
		}

		public async Task<RoleAssignResult> AsignUserToRoleAsync(string userId, int roleId)
		{
			RoleAssignResult response = new();
			User user = await _context.Users.FindAsync(userId);
			ApplicationRole role = await _context.ApplicationRoles.FindAsync(roleId);

			if(user == null || role == null)
			{
				response.Message = "User or Role not found";
				response.Successful = false;
				return response;
			}

			UserRole userRole = new UserRole
			{
				ApplicationRoleId = roleId,
				UserId = userId,
				User = user,
				ApplicationRole = role
			};
			if(await _context.UserRoles.Where(s => s.UserId == userId && s.ApplicationRoleId == roleId).AnyAsync())
			{
				response.Message = "User already has this role";
				response.Successful = true;
				response.NormalizedName = role.NormalizedName;
				response.RoleId = role.Id;
				response.RoleName = role.Name;
				return response;
			}

			_ = await _context.UserRoles.AddAsync(userRole);
			_ = await _context.SaveChangesAsync();
			response.Successful = true;
			response.UserId = user.Id;
			response.RoleId = role.Id;
			response.RoleName = role.Name;
			response.NormalizedName = role.NormalizedName;
			return response;
		}

		public async Task<ApiResponse<bool>> RemoveUserFromRoleAsync(string userId, int roleId)
		{
			ApiResponse<bool> response = new ApiResponse<bool>();
			UserRole userRole = await _context.UserRoles.Where(s => s.UserId == userId && s.ApplicationRoleId == roleId).FirstOrDefaultAsync();
			if(userRole == null)
			{
				response.Message = "User does not have this role";
				response.Successful = false;
				response.Data = false;
				return response;
			}
			_ = _context.UserRoles.Remove(userRole);
			_ = await _context.SaveChangesAsync();
			response.Successful = true;
			response.Data = true;
			return response;
		}

		public async Task<ApiResponse<List<ApplicationRole>>> GetUserRolesAsync(string userId)
		{
			ApiResponse<List<ApplicationRole>> response = new();
			response.Data = await _context.UserRoles.Where(s => s.UserId == userId).Select(s => s.ApplicationRole).ToListAsync() ?? [];
			return response;
		}

		public async Task<List<RoleAssignResult>> AsignUserToRolesAsync(string userId, List<int> roleIds)
		{
			List<RoleAssignResult> response = [];

			foreach(int roleId in roleIds)
			{
				response.Add(await AsignUserToRoleAsync(userId, roleId));
			}

			return response;
		}

		public async Task<List<RoleAssignResult>> AssignAllRolesToOwner(string userId, string applicationId)
		{
			List<RoleAssignResult> responses = [];
			List<ApplicationRole> roles = await _context.ApplicationRoles.Where(s => s.ApplicationId == applicationId).ToListAsync();
			foreach(ApplicationRole role in roles)
			{
				responses.Add(await AsignUserToRoleAsync(userId, role.Id));
			}
			return responses;
		}

		private async Task<ApiResponse<List<String>>> CheckForm(RegisterApplicationUserModel user)
		{
			ApiResponse<List<String>> response = new ApiResponse<List<String>>();
			List<String> errors = new List<String>();

			// Check if email is valid
			if(!new EmailAddressAttribute().IsValid(user.Email))
			{
				errors.Add("Invalid email");
				response.Successful = false;
			}

			// Check if email is unique
			if(!await IsEmailUnique(user.Email))
			{
				errors.Add("Email already exists");
				response.Successful = false;
			}
			// Check if passwords match
			if(user.Password != user.PasswordConfirm)
			{
				errors.Add("Passwords do not match");
				response.Successful = false;
			}
			// Password checks
			PasswordRules rules = await _settingsService.GetPasswordRulesAsync();

			// Check if password is not too short
			if(user.Password.Length < rules.MinimumLength)
			{
				errors.Add("Password is too short");
				response.Successful = false;
			}
			// Check if password is not too long
			if(user.Password.Length > rules.MaximumLength)
			{
				errors.Add("Password is too long");
				response.Successful = false;
			}
			// Check if password contains a number
			if(rules.RequireDigit && !user.Password.Any(char.IsDigit))
			{
				errors.Add("Password must contain a number");
				response.Successful = false;
			}
			// Check if password contains a lowercase letter
			if(rules.RequireLowercase && !user.Password.Any(char.IsLower))
			{
				errors.Add("Password must contain a lowercase letter");
				response.Successful = false;
			}
			// Check if password contains an uppercase letter
			if(rules.RequireUppercase && !user.Password.Any(char.IsUpper))
			{
				errors.Add("Password must contain an uppercase letter");
				response.Successful = false;
			}
			// Check if password contains a special character
			if(rules.RequireNonAlphanumeric && user.Password.All(char.IsLetterOrDigit))
			{
				errors.Add("Password must contain a special character");
				response.Successful = false;
			}

			// Check if user has accepted terms
			if(!user.AcceptTerms)
			{
				errors.Add("You must accept the terms");
				response.Successful = false;
			}
			// Check if user has accepted privacy policy
			if(!user.AcceptPrivacyPolicy)
			{
				errors.Add("You must accept the privacy policy");
				response.Successful = false;
			}
			// Check if user has accepted cookie policy
			if(!user.AcceptCookiePolicy)
			{
				errors.Add("You must accept the cookie policy");
				response.Successful = false;
			}
			if(errors.Count > 0)
			{
				response.Message = "Errors found";
				response.Data = errors;
			}
			return response;
		}
	}
}