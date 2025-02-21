using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	public class RolesManager(IStringLocalizer<RolesManager> t,
		ILogger<RolesManager> logger,
		ApplicationDbContext context)
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly ILogger<RolesManager> _logger = logger;
		private readonly IStringLocalizer<RolesManager> _t = t;

		// Public functions
		public async Task<CreateRoleResult> CreateApplicationRoleAsync(CreateRoleInput input)
		{
			CreateRoleResult result = new CreateRoleResult();
			if(await RoleExistsAsync(input.ApplicationId, input.Name))
			{
				result.Errors.Add(_t["Role already exists"]);
				result.Success = false;
				return result;
			}

			if(!RoleNameOK(input.Name))
			{
				result.Errors.Add(_t["Role name is invalid"]);
				result.Success = false;
				return result;
			}
			ApplicationRole role = new()
			{
				ApplicationId = input.ApplicationId,
				Name = input.Name,
				NormalizedName = input.Name.Trim().ToUpperInvariant(),
				AllignToAll = input.AllignToAll,
				CanAdministerRoles = input.CanAdministerRoles
			};
			try
			{
				_ = _context.ApplicationRoles.Add(role);
				_ = await _context.SaveChangesAsync();
				result.Success = true;
				result.RoleId = role.Id;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error creating role");
				result.Errors.Add(_t["Error creating role"]);
				result.Success = false;
			}
			return result;
		}

		public async Task<DeleteResult<int>> DeleteApplicationRoleAsync(int roleId)
		{
			if(roleId == 0)
			{
				return new DeleteResult<int>
				{
					Success = false,
					ErrorMessage = _t["Role ID is required"]
				};
			}
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == roleId);
			if(role == null)
			{
				return new DeleteResult<int>
				{
					Success = false,
					ErrorMessage = _t["Role not found"]
				};
			}
			try
			{
				_ = _context.ApplicationRoles.Remove(role);
				_ = await _context.SaveChangesAsync();
				return new DeleteResult<int>
				{
					Success = true,
					DeletedId = roleId
				};
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error deleting role");
				return new DeleteResult<int>
				{
					Success = false,
					ErrorMessage = _t["Error deleting role"]
				};
			}
		}

		public async Task<RoleAssignResult> SetApplicationUserRoleAsync(int applicationUserId, int roleId)
		{
			RoleAssignResult result = new RoleAssignResult();
			if(applicationUserId == 0)
			{
				result.Successful = false;
				result.Message = _t["User ID is required"];
				return result;
			}
			if(roleId == 0)
			{
				result.Successful = false;
				result.Message = _t["Role ID is required"];
				return result;
			}
			UserRole? userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.ApplicationRoleId == roleId);
			if(userRole != null)
			{
				result.Successful = true;
				result.Message = _t["User already has the role"];
				return result;
			}
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == roleId);
			if(role == null)
			{
				result.Successful = false;
				result.Message = _t["Role not found"];
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == applicationUserId);
			if(user == null)
			{
				result.Successful = false;
				result.Message = _t["User not found"];
				return result;
			}
			UserRole newUserRole = new()
			{
				ApplicationRoleId = roleId,
				ApplicationUserId = applicationUserId,
				AddedToRole = DateTime.Now
			};
			try
			{
				_ = _context.UserRoles.Add(newUserRole);
				_ = await _context.SaveChangesAsync();
				Application? application = await _context.Applications.FirstOrDefaultAsync(x => x.Id == role.ApplicationId);
				result.Successful = true;
				result.Message = _t["Role assigned"];
				result.ApplicationId = role.ApplicationId;
				result.ApplicationUserId = applicationUserId;
				result.NormalizedName = role.NormalizedName;
				result.RoleId = roleId;
				result.RoleName = role.Name;
				result.UserId = user.UserId;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error assigning role");
				result.Successful = false;
				result.Message = _t["Error assigning role"];
			}

			return result;
		}

		public async Task<RoleAssignResult> SetApplicationUserRoleByNameAsync(int applicationUserId, string roleName)
		{
			RoleAssignResult result = new RoleAssignResult();
			if(applicationUserId == 0)
			{
				result.Successful = false;
				result.Message = _t["User ID is required"];
				return result;
			}
			if(string.IsNullOrWhiteSpace(roleName))
			{
				result.Successful = false;
				result.Message = _t["Role name is required"];
				return result;
			}
			string? applicationId = await _context.ApplicationUsers.Where(s => s.Id == applicationUserId).Select(x => x.ApplicationId).FirstOrDefaultAsync();
			ApplicationRole? role = await _context.ApplicationRoles.Where(s => s.ApplicationId == applicationId).FirstOrDefaultAsync(x => x.NormalizedName == roleName.Trim().ToUpperInvariant());
			if(role == null)
			{
				result.Successful = false;
				result.Message = _t["Role not found"];
				return result;
			}
			UserRole? userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.ApplicationRoleId == role.Id);
			if(userRole != null)
			{
				result.Successful = true;
				result.Message = _t["User already has the role"];
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == applicationUserId);
			if(user == null)
			{
				result.Successful = false;
				result.Message = _t["User not found"];
				return result;
			}
			UserRole newUserRole = new()
			{
				ApplicationRoleId = role.Id,
				ApplicationUserId = applicationUserId,
				AddedToRole = DateTime.Now
			};
			try
			{
				_ = _context.UserRoles.Add(newUserRole);
				_ = await _context.SaveChangesAsync();
				Application? application = await _context.Applications.FirstOrDefaultAsync(x => x.Id == role.ApplicationId);
				result.Successful = true;
				result.Message = _t["Role assigned"];
				result.ApplicationId = role.ApplicationId;
				result.ApplicationUserId = applicationUserId;
				result.NormalizedName = role.NormalizedName;
				result.RoleId = role.Id;
				result.RoleName = role.Name;
				result.UserId = user.UserId;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error assigning role");
				result.Successful = false;
				result.Message = _t["Error assigning role"];
			}
			return result;
		}

		public async Task<RoleAssignResult> SetUserRoleByNameAsync(string userId, string applicationId, string rolename)
		{
			RoleAssignResult result = new RoleAssignResult();
			if(string.IsNullOrWhiteSpace(userId))
			{
				result.Successful = false;
				result.Message = _t["User ID is required"];
				return result;
			}
			if(string.IsNullOrWhiteSpace(applicationId))
			{
				result.Successful = false;
				result.Message = _t["Application ID is required"];
				return result;
			}
			if(string.IsNullOrWhiteSpace(rolename))
			{
				result.Successful = false;
				result.Message = _t["Role name is required"];
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.Where(s => s.ApplicationId == applicationId).Where(s => s.UserId == userId).FirstOrDefaultAsync();
			if(user == null)
			{
				result.Successful = false;
				result.Message = _t["User not found"];
				return result;
			}
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.NormalizedName == rolename.Trim().ToUpperInvariant());
			if(role == null)
			{
				result.Successful = false;
				result.Message = _t["Role not found"];
				return result;
			}
			UserRole? userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.ApplicationRoleId == role.Id);
			if(userRole != null)
			{
				result.Successful = true;
				result.Message = _t["User already has the role"];
				return result;
			}
			UserRole newUserRole = new()
			{
				ApplicationRoleId = role.Id,
				ApplicationUserId = user.Id,
				AddedToRole = DateTime.Now
			};
			try
			{
				_ = _context.UserRoles.Add(newUserRole);
				_ = await _context.SaveChangesAsync();
				result.Successful = true;
				result.Message = _t["Role assigned"];
				result.ApplicationId = applicationId;
				result.ApplicationUserId = user.Id;
				result.NormalizedName = role.NormalizedName;
				result.RoleId = role.Id;
				result.RoleName = role.Name;
				result.UserId = userId;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error assigning role");
				result.Successful = false;
				result.Message = _t["Error assigning role"];
			}
			return result;
		}

		public async Task<RoleAssignResult> SetUserRoleAsync(string userId, int roleId)
		{
			RoleAssignResult result = new RoleAssignResult();
			if(string.IsNullOrWhiteSpace(userId))
			{
				result.Successful = false;
				result.Message = _t["User ID is required"];
				return result;
			}
			if(roleId == 0)
			{
				result.Successful = false;
				result.Message = _t["Role ID is required"];
				return result;
			}
			ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserId == userId);
			if(user == null)
			{
				result.Successful = false;
				result.Message = _t["User not found"];
				return result;
			}
			UserRole? userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.ApplicationRoleId == roleId);
			if(userRole != null)
			{
				result.Successful = true;
				result.Message = _t["User already has the role"];
				return result;
			}
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == roleId);
			if(role == null)
			{
				result.Successful = false;
				result.Message = _t["Role not found"];
				return result;
			}
			UserRole newUserRole = new()
			{
				ApplicationRoleId = roleId,
				ApplicationUserId = user.Id,
				AddedToRole = DateTime.Now
			};
			try
			{
				_ = _context.UserRoles.Add(newUserRole);
				_ = await _context.SaveChangesAsync();
				Application? application = await _context.Applications.FirstOrDefaultAsync(x => x.Id == role.ApplicationId);
				result.Successful = true;
				result.Message = _t["Role assigned"];
				result.ApplicationId = role.ApplicationId;
				result.ApplicationUserId = user.Id;
				result.NormalizedName = role.NormalizedName;
				result.RoleId = roleId;
				result.RoleName = role.Name;
				result.UserId = userId;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error assigning role");
				result.Successful = false;
				result.Message = _t["Error assigning role"];
			}

			return result;
		}

		public async Task<UpdateResult> UpdateApplicationRoleAsync(UpdateRoleInput input)
		{
			UpdateResult result = new()
			{
				Success = true
			};

			if(input.RoleId == 0)
			{
				result.Errors.Add(_t["Role ID is required"]);
				result.Success = false;
				return result;
			}

			ApplicationRole? role = await _context.ApplicationRoles.FirstAsync(x => x.Id == input.RoleId);
			if(role == null)
			{
				result.Errors.Add(_t["Role not found"]);
				result.Success = false;
				return result;
			}

			if(role.ApplicationId != input.ApplicationId)
			{
				result.Errors.Add(_t["Role doesn't match with the application"]);
				result.Success = false;
				return result;
			}

			if(!RoleNameOK(input.Name))
			{
				result.Errors.Add(_t["Role name is invalid"]);
				result.Success = false;
				return result;
			}

			if(input.Name != role.Name)
			{
				if(await RoleExistsAsync(input.ApplicationId, input.Name))
				{
					result.Errors.Add(_t["Role already exists"]);
					result.Success = false;
					return result;
				}
				else
				{
					role.Name = input.Name;
					role.NormalizedName = input.Name.Trim().ToUpperInvariant();
					result.UpdatedValues.Add("Name", input.Name);
				}
			}
			if(input.CanAdministerRoles != role.CanAdministerRoles)
			{
				role.CanAdministerRoles = input.CanAdministerRoles;
				result.UpdatedValues.Add("CanAdministerRoles", input.CanAdministerRoles.ToString());
			}
			if(input.AllignToAll != role.AllignToAll)
			{
				role.AllignToAll = input.AllignToAll;
				result.UpdatedValues.Add("AllignToAll", input.AllignToAll.ToString());
			}
			try
			{
				_ = await _context.SaveChangesAsync();
				return result;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error updating role");
				result.Errors.Add(_t["Error updating role"]);
				result.Success = false;
				return result;
			}
		}

		// Private functions

		private async Task<bool> RoleExistsAsync(string applicationId, string roleName)
		{
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.NormalizedName == roleName.Trim().ToUpperInvariant());
			return role != null;
		}

		private static bool RoleNameOK(string roleName)
		{
			return !string.IsNullOrWhiteSpace(roleName) && roleName.Length <= 64;
		}
	}
}