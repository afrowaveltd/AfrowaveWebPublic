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

		public async Task<DeleteResult> DeleteApplicationRoleAsync(int roleId)
		{
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

		private bool RoleNameOK(string roleName)
		{
			return !string.IsNullOrWhiteSpace(roleName) && roleName.Length <= 64;
		}
	}
}