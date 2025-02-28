using Id.Models.InputModels;
using Id.Models.ResultModels;

namespace Id.Services
{
	/// <summary>
	/// Provides methods to manage roles.
	/// </summary>
	/// <param name="t">Localizer</param>
	/// <param name="logger">Logger</param>
	/// <param name="context">Entity framework</param>
	public class RolesManager(IStringLocalizer<RolesManager> t,
		ILogger<RolesManager> logger,
		ApplicationDbContext context) : IRolesManager
	{
		// Initialization
		private readonly ApplicationDbContext _context = context;

		private readonly ILogger<RolesManager> _logger = logger;
		private readonly IStringLocalizer<RolesManager> _t = t;

		// Public functions

		/// <summary>
		/// Creates a new role for a specific application.
		/// </summary>
		/// <param name="input">The role details including application ID, name, and permissions.</param>
		/// <returns>A result indicating success or failure, including error messages if applicable.</returns>
		/// <exception cref="Exception">Thrown if an error occurs while saving the role to the database.</exception>
		/// <example>
		/// Example request:
		/// {
		///     "ApplicationId": "123e4567-e89b-12d3-a456-426614174000",
		///     "Name": "Admin",
		///     "AllignToAll": false,
		///     "CanAdministerRoles": true
		/// }
		/// Example response:
		/// {
		///     "Success": true,
		///     "RoleId": "987e6543-e21b-34d5-a678-526614174999",
		///     "Errors": []
		/// }
		/// </example>
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
				result.RoleName = role.Name;
				result.NormalizedName = role.NormalizedName;
				result.AllignToAll = role.AllignToAll;
				result.CanAdministerRoles = role.CanAdministerRoles;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error creating role");
				result.Errors.Add(_t["Error creating role"]);
				result.Success = false;
			}
			return result;
		}

		/// <summary>
		/// Deletes a role from an application.
		/// </summary>
		/// <param name="roleId">The role ID to be deleted.</param>
		/// <returns>The result indicating success or failure, including error messages.</returns>
		/// <example>
		/// Example request:
		/// DELETE /api/roles/5
		/// Example response:
		/// {
		///     "Success": true,
		///     "DeletedId": 5
		/// }
		/// </example>
		/// <exception cref="Exception">Thrown if an error occurs while deleting the role.</exception>
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

		/// <summary>
		/// Gets all roles for a specific application.
		/// </summary>
		/// <param name="applicationId"></param>
		/// <returns>List of application roles and empty list if no roles were set</returns>
		/// <example>
		/// Request example:
		/// await GetAllApplicationRolesAsync("123e4567-e89b-12d3-a456-426614174000");
		/// Response example:
		/// [
		///		{
		///			Id: 1,
		///			ApplicationId: "123e4567-e89b-12d3-a456-426614174000",
		///			Name: "User",
		///			NormalizedName: "USER",
		///	      AllignToAll: true,
		///	      CanAdministerRoles: false
		///	    },
		///	    {
		///			Id: 2,
		///			ApplicationId: "123e4567-e89b-12d3-a456-426614174575",
		///			Name: "Admin",
		///			NormalizedName: "ADMIN",
		///			AllignToAll: false,
		///			CanAdministerRoles: true
		///		}
		///	 ]
		///	 </example>
		public async Task<List<ApplicationRole>> GetAllApplicationRolesAsync(string applicationId)
		{
			return await _context.ApplicationRoles.Where(x => x.ApplicationId == applicationId).ToListAsync();
		}

		/// <summary>
		/// Gets all user roles for a specific application user.
		/// </summary>
		/// <param name="applicationUserId"></param>
		/// <returns>List of RoleAssignResults</returns>
		/// <example>
		/// Request example:
		/// await GetApplicationUserRolesAsync(12);
		/// Response example:
		/// [
		///   {
		///			"RoleId": 1,
		///			"UserId": "abc123",
		///			"ApplicationUserId": 12,
		///			"ApplicationId": "123e4567-e89b-12d3-a456-426614174000",
		///			"RoleName": "User",
		///			"NormalizedName": "USER",
		///			"Successful": true,
		///			"Message": "Role assigned"
		///   }
		/// ]
		/// </example>
		public async Task<List<RoleAssignResult>> GetApplicationUserRolesAsync(int applicationUserId)
		{
			if(applicationUserId == 0)
			{ return []; }
			List<RoleAssignResult> results = [];
			List<UserRole> userRoles = await _context.UserRoles.Where(x => x.ApplicationUserId == applicationUserId).ToListAsync();
			foreach(UserRole userRole in userRoles)
			{
				ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == userRole.ApplicationRoleId);
				if(role != null)
				{
					ApplicationUser? user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == applicationUserId);
					if(user != null)
					{
						results.Add(new RoleAssignResult
						{
							ApplicationId = role.ApplicationId,
							ApplicationUserId = applicationUserId,
							NormalizedName = role.NormalizedName,
							RoleId = role.Id,
							RoleName = role.Name,
							UserId = user.UserId
						});
					}
				}
			}
			return results;
		}

		/// <summary>
		/// Assigns all roles of an application to a specific user.
		/// </summary>
		/// <param name="applicationId">The ID of the application.</param>
		/// <param name="userId">The user ID to which roles will be assigned.</param>
		/// <returns>A list of role assignment results.</returns>
		/// <example>
		/// Example request:
		/// {
		///     "ApplicationId": 1,
		///     "UserId": "abc123"
		/// }
		/// Example response:
		/// [
		///     {
		///         "Successful": true,
		///         "Message": "Role assigned",
		///         "RoleId": 3
		///     }
		/// ]
		/// </example>
		public async Task<List<RoleAssignResult>> SetAllRolesToOwner(string applicationId, string userId)
		{
			List<RoleAssignResult> results = [];
			if(applicationId == null || userId == null)
			{
				results.Add(new RoleAssignResult
				{
					Successful = false,
					Message = _t["Missing user data"]
				});
				return results;
			}
			ApplicationUser? user = await _context.ApplicationUsers
				.FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.UserId == userId);
			if(user == null)
			{
				results.Add(new RoleAssignResult
				{
					Successful = false,
					Message = _t["User not found"]
				});
				return results;
			}
			List<ApplicationRole> roles = await _context.ApplicationRoles
				.Where(x => x.ApplicationId == applicationId)
				.Where(x => x.AllignToAll)
				.ToListAsync();
			foreach(ApplicationRole role in roles)
			{
				UserRole? userRole = await _context.UserRoles
					.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.ApplicationRoleId == role.Id);
				if(userRole != null)
				{
					results.Add(new RoleAssignResult
					{
						Successful = true,
						Message = _t["User already has the role"],
						ApplicationId = role.ApplicationId,
						ApplicationUserId = user.Id,
						NormalizedName = role.NormalizedName,
						RoleId = role.Id,
						RoleName = role.Name,
						UserId = user.UserId
					});
				}
				else
				{
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
						results.Add(new RoleAssignResult
						{
							Successful = true,
							Message = _t["Role assigned"],
							ApplicationId = role.ApplicationId,
							ApplicationUserId = user.Id,
							NormalizedName = role.NormalizedName,
							RoleId = role.Id,
							RoleName = role.Name,
							UserId = user.UserId
						});
					}
					catch(Exception ex)
					{
						_logger.LogError(ex, "Error assigning role");
						results.Add(new RoleAssignResult
						{
							Successful = false,
							Message = _t["Error assigning role"]
						});
					}
				}
			}
			return results;
		}

		/// <summary>
		/// Assign default roles to newly created user.
		/// </summary>
		/// <param name="applicationUserId"></param>
		/// <returns>List of RoleAssignResult records</returns>
		/// <example>
		/// Example request:
		/// await SetApplicationUserDefaultRolesAsync(12);
		/// Example response:
		/// [
		///   {
		///			"RoleId": 1,
		///			"UserId": "abc123",
		///			"ApplicationUserId": 12,
		///			"ApplicationId": "123e4567-e89b-12d3-a456-426614174000",
		///			"RoleName": "User",
		///			"NormalizedName": "USER",
		///			"Successful": true,
		///			"Message": "Role assigned"
		///   }
		/// ]
		/// </example>
		public async Task<List<RoleAssignResult>> SetApplicationUserDefaultRolesAsync(int applicationUserId)
		{
			List<RoleAssignResult> results = new();
			ApplicationUser? user = await _context.ApplicationUsers
				.FirstOrDefaultAsync(x => x.Id == applicationUserId);
			if(user == null)
			{
				results.Add(new RoleAssignResult
				{
					Successful = false,
					Message = _t["User not found"]
				});
				return results;
			}
			List<ApplicationRole> roles = await _context.ApplicationRoles
				.Where(x => x.ApplicationId == user.ApplicationId)
				.Where(x => x.AllignToAll)
				.ToListAsync();

			foreach(ApplicationRole role in roles)
			{
				UserRole? userRole = await _context.UserRoles
					.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.ApplicationRoleId == role.Id);
				if(userRole != null)
				{
					results.Add(new RoleAssignResult
					{
						Successful = true,
						Message = _t["User already has the role"],
						ApplicationId = role.ApplicationId,
						ApplicationUserId = applicationUserId,
						NormalizedName = role.NormalizedName,
						RoleId = role.Id,
						RoleName = role.Name,
						UserId = user.UserId
					});
				}
				else
				{
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
						results.Add(new RoleAssignResult
						{
							Successful = true,
							Message = _t["Role assigned"],
							ApplicationId = role.ApplicationId,
							ApplicationUserId = applicationUserId,
							NormalizedName = role.NormalizedName,
							RoleId = role.Id,
							RoleName = role.Name,
							UserId = user.UserId
						});
					}
					catch(Exception ex)
					{
						_logger.LogError(ex, "Error assigning role");
						results.Add(new RoleAssignResult
						{
							Successful = false,
							Message = _t["Error assigning role"]
						});
					}
				}
			}
			return results;
		}

		/// <summary>
		/// Assigns a role to a user by role ID.
		/// </summary>
		/// <param name="applicationUserId">The ID of the application user.</param>
		/// <param name="roleId">The ID of the role to assign.</param>
		/// <returns>A result indicating whether the assignment was successful.</returns>
		/// <example>
		/// Example request:
		/// POST /api/roles/assign
		/// {
		///     "ApplicationUserId": 12,
		///     "RoleId": 5
		/// }
		/// Example response:
		/// {
		///     "Successful": true,
		///     "Message": "Role assigned"
		/// }
		/// </example>
		public async Task<RoleAssignResult> SetApplicationUserRoleAsync(int applicationUserId, int roleId)
		{
			RoleAssignResult result = new();
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

		/// <summary>
		/// Assigns a role to a user by role name.
		/// </summary>
		/// <param name="applicationUserId">The ID of the application user.</param>
		/// <param name="roleName">The name of the role to assign.</param>
		/// <returns>A result indicating whether the assignment was successful.</returns>
		/// <example>
		/// Example request:
		/// await SetApplicationUserRoleByNameAsync(12, "User");
		/// Example response:
		/// {
		///     "Successful": true,
		///     "Message": "Role assigned"
		/// }
		/// </example>
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

		/// <summary>
		/// Assigns a role to a user by role name.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="applicationId"></param>
		/// <param name="rolename"></param>
		/// <returns>RoleAssignResult instance.</returns>
		/// <example>
		/// Example request:
		/// await SetUserRoleByNameAsync("abc123", "123e4567-e89b-12d3-a456-426614174000", "User");
		/// Example response:
		/// {
		///		"Successful": true,
		///		"Message": "Role assigned"
		/// }
		/// </example>
		public async Task<RoleAssignResult> SetUserRoleByNameAsync(string userId, string applicationId, string rolename)
		{
			RoleAssignResult result = new();
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

		/// <summary>
		/// Assigns a role to a user by rolename
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="roleId"></param>
		/// <returns>RoleAssignResult instance</returns>
		/// <example>
		/// Example request:
		/// await SetUserRoleByNameAsync("123", "User");
		/// Example response:
		/// {
		///		"Successful": true,
		///		"Message": "Role assigned"
		///	 }
		///	 </example>
		public async Task<RoleAssignResult> SetUserRoleAsync(string userId, int roleId)
		{
			RoleAssignResult result = new();
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

		/// <summary>
		/// Updates application role
		/// </summary>
		/// <param name="input"></param>
		/// <returns>UpdateResult instance</returns>
		/// <example>
		/// Example request:
		/// await UpdateApplicationRoleAsync(new UpdateRoleInput
		/// {
		///		RoleId = 1,
		///		ApplicationId = "123",
		///		Name = "User",
		///		CanAdministerRoles = true,
		///		AllignToAll = false
		///	 });
		/// Example response:
		/// {
		///		"Success": true,
		///		"UpdatedValues": {
		///		"Name": "User",
		///		},
		///		"Errors": []
		///		}
		///	 }
		/// </example>
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

		/// <summary>
		/// Removes a user from a role by role name
		/// </summary>
		/// <param name="applicationUserId"></param>
		/// <param name="roleName"></param>
		/// <returns>true if role was removed, false in case of failure</returns>
		/// <example>
		/// Request example:
		/// await RemoveApplicationUserFromRoleAsync(12, "User");
		/// Response example:
		/// true
		/// </example>

		public async Task<bool> RemoveApplicationUserFromRoleAsync(int applicationUserId, string roleName)
		{
			ApplicationRole? role = await _context.ApplicationRoles.FirstOrDefaultAsync(x => x.NormalizedName == roleName.Trim().ToUpperInvariant());
			if(role == null)
			{
				return false;
			}
			UserRole? userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId && x.ApplicationRoleId == role.Id);
			if(userRole == null)
			{
				return false;
			}
			try
			{
				_ = _context.UserRoles.Remove(userRole);
				_ = await _context.SaveChangesAsync();
				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error removing user from role");
				return false;
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